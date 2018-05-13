﻿using System;

namespace Effekseer.GUI.Dialog
{
	class SaveOnDisposing : IRemovableControl
    {
		string title = string.Empty;
        string message = string.Empty;
        string id = "###saveOnDisposing";

        bool opened = true;

        bool isFirstUpdate = true;
              
		public bool ShouldBeRemoved { get; private set; } = false;

		Action disposed = null;

		public SaveOnDisposing(Action disposed)
		{
			title = "Warning";
			var format = Resources.GetString("ConfirmSaveChanged");
			message = string.Format(format, System.IO.Path.GetFileName(Core.FullPath));

			this.disposed = disposed;
			Manager.AddControl(this);
		}

		public void Update()
		{
			if (isFirstUpdate)
            {
                Manager.NativeManager.OpenPopup(id);
                isFirstUpdate = false;
            }

            if (Manager.NativeManager.BeginPopupModal(title + id, ref opened, swig.WindowFlags.AlwaysAutoResize))
            {
                Manager.NativeManager.Text(message);

                if (Manager.NativeManager.Button("OK"))
				{
					if(Commands.Overwrite())
					{
						ShouldBeRemoved = true;
                        disposed();
					}
                }

				if (Manager.NativeManager.Button("No"))
                {
                    ShouldBeRemoved = true;
					disposed();        
                }

				if (Manager.NativeManager.Button("Cancel"))
                {
                    ShouldBeRemoved = true;
                }

                Manager.NativeManager.EndPopup();
            }
            else
            {
                ShouldBeRemoved = true;
            }
		}
    }
}
