﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effekseer.GUI.Component
{
	class Vector2DWithRandom : Control, IParameterControl
	{
		string id1 = "";
		string id2 = "";
		string id_r1 = "";
		string id_r2 = "";
		string id_c = "";

		bool isPopupShown = false;

		bool isActive = false;

		public string Label { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		Data.Value.Vector2DWithRandom binding = null;

		float[] internalValue1 = new float[] { 0.0f, 0.0f };
		float[] internalValue2 = new float[] { 0.0f, 0.0f };

		public bool EnableUndo { get; set; } = true;

		public Data.Value.Vector2DWithRandom Binding
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
					if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
					{
						internalValue1[0] = binding.X.Center;
						internalValue1[1] = binding.Y.Center;
						internalValue2[0] = binding.X.Amplitude;
						internalValue2[1] = binding.Y.Amplitude;
					}
					else
					{
						internalValue1[0] = binding.X.Min;
						internalValue1[1] = binding.Y.Min;
						internalValue2[0] = binding.X.Max;
						internalValue2[1] = binding.Y.Max;
					}
				}
			}
		}

		public Vector2DWithRandom(string label = null)
		{
			if (label != null)
			{
				Label = label;
			}

			id1 = "###" + Manager.GetUniqueID().ToString();
			id2 = "###" + Manager.GetUniqueID().ToString();
			id_r1 = "###" + Manager.GetUniqueID().ToString();
			id_r2 = "###" + Manager.GetUniqueID().ToString();
			id_c = "###" + Manager.GetUniqueID().ToString();
		}

		public void SetBinding(object o)
		{
			var o_ = o as Data.Value.Vector2DWithRandom;
			Binding = o_;
		}

		public void FixValue()
		{
			FixValueInternal(false);
		}

		void FixValueInternal(bool combined)
		{
			if (EnableUndo)
			{
				if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
				{
					binding.X.SetCenter(internalValue1[0], combined);
					binding.Y.SetCenter(internalValue1[1], combined);
				}
				else
				{
					binding.X.SetMin(internalValue1[0], combined);
					binding.Y.SetMin(internalValue1[1], combined);
				}
			}
			else
			{
				if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
				{
					binding.X.SetCenterDirectly(internalValue1[0]);
					binding.Y.SetCenterDirectly(internalValue1[1]);
				}
				else
				{
					binding.X.SetMinDirectly(internalValue1[0]);
					binding.Y.SetMinDirectly(internalValue1[1]);
				}
			}

			if (EnableUndo)
			{
				if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
				{
					binding.X.SetAmplitude(internalValue2[0], combined);
					binding.Y.SetAmplitude(internalValue2[1], combined);
				}
				else
				{
					binding.X.SetMax(internalValue2[0], combined);
					binding.Y.SetMax(internalValue2[1], combined);
				}
			}
			else
			{
				if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
				{
					binding.X.SetAmplitudeDirectly(internalValue2[0]);
					binding.Y.SetAmplitudeDirectly(internalValue2[1]);
				}
				else
				{
					binding.X.SetMaxDirectly(internalValue2[0]);
					binding.Y.SetMaxDirectly(internalValue2[1]);
				}
			}
		}

		public override void OnDisposed()
		{
			FixValueInternal(false);
		}

		public override void Update()
		{
			if (binding == null) return;
			isPopupShown = false;

			if (binding != null)
			{
				if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
				{
					internalValue1[0] = binding.X.Center;
					internalValue1[1] = binding.Y.Center;
					internalValue2[0] = binding.X.Amplitude;
					internalValue2[1] = binding.Y.Amplitude;
				}
				else
				{
					internalValue1[0] = binding.X.Min;
					internalValue1[1] = binding.Y.Min;
					internalValue2[0] = binding.X.Max;
					internalValue2[1] = binding.Y.Max;
				}
			}

			var txt_r1 = string.Empty;
			var txt_r2 = string.Empty;

			if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
			{
				txt_r1 = Resources.GetString("Mean");
				txt_r2 = Resources.GetString("Deviation");
			}
			else
			{
				txt_r1 = Resources.GetString("Max");
				txt_r2 = Resources.GetString("Min");
			}

			if (Manager.NativeManager.DragFloat2EfkEx(id1, internalValue1, 1, float.MinValue, float.MaxValue, txt_r1 + ":" + "%.3f", txt_r1 + ":" + "%.3f"))
			{
				if (EnableUndo)
				{
					if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
					{
						binding.X.SetCenter(internalValue1[0], isActive);
						binding.Y.SetCenter(internalValue1[1], isActive);
					}
					else
					{
						binding.X.SetMin(internalValue1[0], isActive);
						binding.Y.SetMin(internalValue1[1], isActive);
					}
				}
				else
				{
					if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
					{
						binding.X.SetCenterDirectly(internalValue1[0]);
						binding.Y.SetCenterDirectly(internalValue1[1]);
					}
					else
					{
						binding.X.SetMinDirectly(internalValue1[0]);
						binding.Y.SetMinDirectly(internalValue1[1]);
					}
				}
			}

			var isActive_Current = Manager.NativeManager.IsItemActive();

			Popup();

			if (Manager.NativeManager.DragFloat2EfkEx(id2, internalValue2, 1, float.MinValue, float.MaxValue, txt_r2 + ":" + "%.3f", txt_r2 + ":" + "%.3f"))
			{
				if (EnableUndo)
				{
					if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
					{
						binding.X.SetAmplitude(internalValue2[0], isActive);
						binding.Y.SetAmplitude(internalValue2[1], isActive);
					}
					else
					{
						binding.X.SetMax(internalValue2[0], isActive);
						binding.Y.SetMax(internalValue2[1], isActive);
					}
				}
				else
				{
					if (binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude)
					{
						binding.X.SetAmplitudeDirectly(internalValue2[0]);
						binding.Y.SetAmplitudeDirectly(internalValue2[1]);
					}
					else
					{
						binding.X.SetMaxDirectly(internalValue2[0]);
						binding.Y.SetMaxDirectly(internalValue2[1]);
					}
				}
			}

			isActive_Current |= Manager.NativeManager.IsItemActive();

			if (isActive && !isActive_Current)
			{
				FixValue();
			}

			isActive = isActive_Current;

			Popup();
		}

		void Popup()
		{
			if (isPopupShown) return;

			if (Manager.NativeManager.BeginPopupContextItem(id_c))
			{
				var txt_r_r1 = Resources.GetString("Gauss");
				var txt_r_r2 = Resources.GetString("Range");

				if (Manager.NativeManager.RadioButton(txt_r_r1 + id_r1, binding.DrawnAs == Data.DrawnAs.CenterAndAmplitude))
				{
					binding.DrawnAs = Data.DrawnAs.CenterAndAmplitude;
				}

				Manager.NativeManager.SameLine();

				if (Manager.NativeManager.RadioButton(txt_r_r2 + id_r2, binding.DrawnAs == Data.DrawnAs.MaxAndMin))
				{
					binding.DrawnAs = Data.DrawnAs.MaxAndMin;
				}

				Manager.NativeManager.EndPopup();
				isPopupShown = true;
			}
		}

	}
}
