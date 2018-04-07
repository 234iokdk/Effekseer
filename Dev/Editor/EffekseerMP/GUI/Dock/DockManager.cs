﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Effekseer.GUI.Dock
{
	class DockManager : GroupControl, IRemovableControl, IDroppableControl
	{
		public string Label { get; set; } = string.Empty;

		string id = "";

		bool opened = true ;

		public DockManager()
		{
			id = "###" + Manager.GetUniqueID().ToString();
		}

		public override void Update()
		{
			if (opened)
			{
				if (Manager.NativeManager.Begin(Label + id, ref opened))
				{
					Manager.NativeManager.BeginDockspace();

					Controls.Lock();

					foreach (var c in Controls.Internal.OfType<DockPanel>())
					{
						c.Update();
					}

					Controls.Unlock();

					Manager.NativeManager.EndDockspace();
				}

				Manager.NativeManager.End();
			}
			else
			{
				ShouldBeRemoved = true;
			}
		}
	}
}
