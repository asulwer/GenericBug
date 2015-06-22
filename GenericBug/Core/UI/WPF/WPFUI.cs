using GenericBug.Core.Reporting.Info;
using GenericBug.Core.UI.WinForms;
using GenericBug.Core.Util.Exceptions;
using GenericBug.Core.Util.Serialization;
using GenericBug.Enums;

namespace GenericBug.Core.UI.WPF
{
	/// <summary>
	/// This class is used to prevent statically referencing any WPF dlls from the UISelector.cs thus prevents
	/// any unnecessary assembly from getting loaded into the memory.
	/// </summary>
    public static class WPFUI
	{
        public static UIDialogResult ShowDialog(UIMode uiMode, SerializableException exception, Report report)
		{
			if (uiMode == UIMode.Minimal)
			{
				// ToDo:  Create WPF dialogs
				return new Minimal().ShowDialog(report);
			}
			else if (uiMode == UIMode.Normal)
			{
				// ToDo:  Create WPF dialogs
				using (var ui = new Normal())
				{
					return ui.ShowDialog(report);
				}
			}
			else if (uiMode == UIMode.Full)
			{
				// ToDo:  Create WPF dialogs
				using (var ui = new Full())
				{
					return ui.ShowDialog(exception, report);
				}
			}
			else
			{
				throw GenericBugConfigurationException.Create(() => Settings.UIMode, "Parameter supplied for settings property is invalid.");
			}
		}
	}
}
