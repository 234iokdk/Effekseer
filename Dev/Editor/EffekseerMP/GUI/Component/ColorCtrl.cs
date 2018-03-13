﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effekseer.GUI.Component
{
	class ColorCtrl : IControl
	{
		string id = "";

		public string Label { get; set; } = string.Empty;

		Data.Value.Color binding = null;

		float[] internalValue = new float[] { 1.0f, 1.0f, 1.0f, 1.0f };

		public bool ShouldBeRemoved { get; private set; } = false;

		public bool EnableUndo { get; set; } = true;

		public Data.Value.Color Binding
		{
			get
			{
				return binding;
			}
			set
			{
				if (binding == value) return;

				binding = value;

				if (binding != null)
				{
					internalValue[0] = binding.R / 255.0f;
					internalValue[1] = binding.G / 255.0f;
					internalValue[2] = binding.B / 255.0f;
					internalValue[3] = binding.A / 255.0f;
				}
			}
		}

		public ColorCtrl(string label = null)
		{
			if (label != null)
			{
				Label = label;
			}

			var rand = new Random();
			id = "###" + Manager.GetUniqueID().ToString();
		}

		public void SetBinding(object o)
		{
			var o_ = o as Data.Value.Color;
			Binding = o_;
		}

		public void Update()
		{
			if (binding != null)
			{
				internalValue[0] = binding.R / 255.0f;
				internalValue[1] = binding.G / 255.0f;
				internalValue[2] = binding.B / 255.0f;
				internalValue[3] = binding.A / 255.0f;
			}

			if (Manager.NativeManager.ColorEdit4(Label + id, internalValue, swig.ColorEditFlags.None))
			{
				if (EnableUndo)
				{
					binding.SetValue(
						(int)(internalValue[0] * 255),
						(int)(internalValue[1] * 255),
						(int)(internalValue[2] * 255),
						(int)(internalValue[3] * 255));
				}
				else
				{
					binding.R.SetValueDirectly((int)(internalValue[0] * 255));
					binding.G.SetValueDirectly((int)(internalValue[1] * 255));
					binding.B.SetValueDirectly((int)(internalValue[2] * 255));
					binding.A.SetValueDirectly((int)(internalValue[3] * 255));
				}
			}
		}
	}
}
