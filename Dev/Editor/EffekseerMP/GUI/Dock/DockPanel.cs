﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effekseer.GUI.Dock
{
    class DockPanel : GroupControl, IRemovableControl, IDroppableControl
    {
		public string Label { get; set; } = string.Empty;

		string id = "";

		bool opened = true;

		internal swig.DockSlot InitialDockSlot = swig.DockSlot.Float;

		internal swig.Vec2 InitialDockSize = new swig.Vec2(0, 0);

		internal float InitialDockRate = 0.5f;

		internal bool InitialDockReset = false;

		public DockPanel()
		{
			id = "###" + Manager.GetUniqueID().ToString();
		}

		public override void Update()
		{
			if(opened)
			{
				if (Manager.IsDockMode())
				{
					Manager.NativeManager.SetNextDock(InitialDockSlot);
					Manager.NativeManager.SetNextDockRate(InitialDockRate);
					if(InitialDockReset)
					{
						Manager.NativeManager.ResetNextParentDock();
					}

					if (Manager.NativeManager.BeginDock(Label + id, ref opened, swig.WindowFlags.None, InitialDockSize))
					{
						UpdateInternal();


						Controls.Lock();

						foreach (var c in Controls.Internal)
						{
							c.Update();
						}

						Controls.Unlock();
					}

					Manager.NativeManager.EndDock();
				}
				else
				{
					if (Manager.NativeManager.Begin(Label + id, ref opened))
					{
						UpdateInternal();


						Controls.Lock();

						foreach (var c in Controls.Internal)
						{
							c.Update();
						}

						Controls.Unlock();
					}

					Manager.NativeManager.End();
				}
			}
			else
			{
				ShouldBeRemoved = true;
			}
		}

        protected virtual void UpdateInternal()
        {
        }
    }
}
