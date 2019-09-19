﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effekseer.IO
{
	/// <summary>
	/// It should be made as unit test
	/// </summary>
	public class ChunkTest
	{
		public static void Test()
		{
			// a function to generate dummy data for testing
			Func<int, byte[]> generateDummyData = (int n) =>
			{
				var ret = new byte[n];
				var rand = new Random();

				for (int i = 0; i < n; i++)
				{
					ret[i] = (byte)rand.Next(255);
				}

				return ret;
			};

			var src = new Chunk();
			src.AddChunk("tes1", generateDummyData(25));
			src.AddChunk("tes2", generateDummyData(255));
			src.AddChunk("tes3", generateDummyData(256));
			var saved = src.Save();

			var dst = new Chunk();
			dst.Load(saved);

			if (!(src.Blocks[0].Chunk == dst.Blocks[0].Chunk)) throw new Exception();
			if (!(src.Blocks[1].Chunk == dst.Blocks[1].Chunk)) throw new Exception();
			if (!(src.Blocks[2].Chunk == dst.Blocks[2].Chunk)) throw new Exception();

			if (!(src.Blocks[0].Buffer.Length == dst.Blocks[0].Buffer.Length)) throw new Exception();
			if (!(src.Blocks[1].Buffer.Length == dst.Blocks[1].Buffer.Length)) throw new Exception();
			if (!(src.Blocks[2].Buffer.Length == dst.Blocks[2].Buffer.Length)) throw new Exception();

			if (!src.Blocks[0].Buffer.SequenceEqual(dst.Blocks[0].Buffer)) throw new Exception();
			if (!src.Blocks[1].Buffer.SequenceEqual(dst.Blocks[1].Buffer)) throw new Exception();
			if (!src.Blocks[2].Buffer.SequenceEqual(dst.Blocks[2].Buffer)) throw new Exception();
		}
	}

	/// <summary>
	/// Chunck to save or load
	/// TODO create new file
	/// </summary>
	class Chunk
	{
		public List<ChunkBlock> Blocks = new List<ChunkBlock>();

		public void AddChunk(string chunk, byte[] buffer)
		{
			var block = new ChunkBlock();
			block.Chunk = chunk;
			block.Buffer = buffer;
			Blocks.Add(block);
		}

		public byte[] Save()
		{
			List<byte[]> data = new List<byte[]>();

			foreach (var binary in Blocks)
			{
				var chunk = new byte[4] { 0, 0, 0, 0 };
				var chararray = Encoding.UTF8.GetBytes(binary.Chunk);
				for (int i = 0; i < Math.Min(chunk.Length, chararray.Length); i++)
				{
					chunk[i] = chararray[i];
				}

				data.Add(chunk);
				data.Add(BitConverter.GetBytes((UInt32)binary.Buffer.Length));
				data.Add(binary.Buffer);
			}

			return data.SelectMany(_ => _).ToArray();
		}

		public bool Load(byte[] data)
		{
			Blocks.Clear();

			int pos = 0;

			while (pos < data.Length)
			{
				var chunkdata = data.Skip(pos).Take(4).ToArray();
				var chunk = Encoding.UTF8.GetString(chunkdata, 0, 4);
				pos += 4;

				var size = BitConverter.ToInt32(data, pos);
				pos += 4;

				var buffer = data.Skip(pos).Take(size).ToArray();
				pos += size;

				var binary = new ChunkBlock();
				binary.Chunk = chunk;
				binary.Buffer = buffer;
				Blocks.Add(binary);
			}

			if (pos != data.Length) return false;

			return true;
		}
	}

	class ChunkBlock
	{
		public string Chunk;

		public byte[] Buffer;
	}

	class EfkEfc
	{
		const int Version = 0;


		class Element
		{
			public string Name = null;
			public UInt16 NameIndex = 0;
			public object Value = null;
			public List<Element> Children = null;
		}

		byte[] Compress(System.Xml.XmlDocument xml)
		{
			// visit all nodes
			Func<System.Xml.XmlNodeList, List<Element>> visit = null;
			visit = (System.Xml.XmlNodeList xmlNodes) =>
			{
				List<Element> elements = new List<Element>();

				for (int i = 0; i < xmlNodes.Count; i++)
				{
					Element element = new Element();

					element.Name = xml.ChildNodes[i].Name;

					if (xml.ChildNodes[i].ChildNodes.Count == 1 &&
					xml.ChildNodes[i].ChildNodes[0].ChildNodes.Count == 0)
					{
						var value = xml.ChildNodes[i].ChildNodes[0].Value;
						element.Value = value;
					}
					else
					{
						element.Children = visit(xmlNodes[i].ChildNodes);
					}

					elements.Add(element);
				}

				return elements;
			};

			var visits = visit(xml.ChildNodes);

			// compress...

			return Encoding.UTF8.GetBytes(xml.InnerXml);
		}

		string Decompress(byte[] buffer)
		{
			return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
		}

		byte[] GetBinaryStr(string str)
		{
			List<byte[]> data = new List<byte[]>();

			var path = Encoding.Unicode.GetBytes(str);
			data.Add(BitConverter.GetBytes((path.Count() + 2) / 2));
			data.Add(path);
			data.Add(new byte[] { 0, 0 });

			return data.SelectMany(_ => _).ToArray();
		}

		public bool Save(string path)
		{
			// editor data
			var editorData = Core.SaveAsXmlDocument(path);

			// binary data
			var binaryExporter = new Binary.Exporter();
			var binaryData = binaryExporter.Export(1);  // TODO change magnification

			// info data
			byte[] infoData = null;
			{
				var data = new List<byte[]>();
				Action<HashSet<string>> exportStrs = (HashSet<string> strs) =>
				{
					data.Add(BitConverter.GetBytes(strs.Count));
					foreach (var exportedPath in strs)
					{
						data.Add(GetBinaryStr(exportedPath));
					}
				};

				exportStrs(binaryExporter.UsedTextures);
				exportStrs(binaryExporter.UsedNormalTextures);
				exportStrs(binaryExporter.UsedDistortionTextures);
				exportStrs(binaryExporter.Models);
				exportStrs(binaryExporter.Sounds);

				infoData = data.SelectMany(_ => _).ToArray();
			}

			// header
			byte[] headerData = null;
			{
				var data = new List<byte[]>();
				data.Add(Encoding.UTF8.GetBytes("PKFE"));
				data.Add(BitConverter.GetBytes(Version));

				headerData = data.SelectMany(_ => _).ToArray();
			}

			Chunk chunk = new Chunk();
			chunk.AddChunk("INFO", infoData);
			chunk.AddChunk("EDIT", Compress(editorData));
			chunk.AddChunk("BIN_", binaryData);

			var chunkData = chunk.Save();

			var allData = headerData.Concat(chunkData).ToArray();

			try
			{
				System.IO.File.WriteAllBytes(path, allData);
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool Load(string path)
		{
			byte[] allData = null;
			try
			{
				allData = System.IO.File.ReadAllBytes(path);
			}
			catch
			{
				return false;
			}

			if (allData.Length < 24) return false;

			if (allData[0] != 'E' ||
	allData[1] != 'F' ||
	allData[2] != 'K' ||
	allData[3] != 'P')
			{
				return false;
			}

			var version = BitConverter.ToInt32(allData, 4);

			var chunkData = allData.Skip(8).ToArray();

			var chunk = new Chunk();
			chunk.Load(chunkData);

			var editBlock = chunk.Blocks.FirstOrDefault(_ => _.Chunk == "EDIT");
			if(editBlock == null)
			{
				return false;
			}

			return Core.LoadFromXml(Decompress(editBlock.Buffer), path);
		}
	}
}
