﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effekseer
{
	public class ErrorUtils
	{
		public static void ThrowFileNotfound()
		{
			if (Core.Option.GuiLanguage.Value == Language.Japanese)
			{
				throw new Exception("リソースファイルを更新してください。Script/setup.pyを呼ぶか、cmakeを使用し、ResourceDataをリビルドしてください。");
			}
			else
			{
				throw new Exception("Please update resource files!. call Script/setup.py or use cmake and rebuild ResourceData.");
			}
		}
	}

	/// <summary>
	/// Attribute for unique name
	/// </summary>
	[AttributeUsage(
	AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method,
	AllowMultiple = false,
	Inherited = false)]
	public class UniqueNameAttribute : Attribute
	{
		public UniqueNameAttribute()
		{
			value = string.Empty;
		}

		public string value
		{
			get;
			set;
		}

		/// <summary>
		/// Get unique name from attribute
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public static string GetUniqueName(object[] attributes)
		{
			if (attributes != null && attributes.Length > 0)
			{
				foreach (var attribute in attributes)
				{
					if (!(attribute is UniqueNameAttribute)) continue;

					return ((UniqueNameAttribute)attribute).value;
				}
			}

			return string.Empty;
		}
	}
}
