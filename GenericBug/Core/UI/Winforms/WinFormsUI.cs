using GenericBug.Core.Reporting.Info;
using GenericBug.Core.Util.Exceptions;
using GenericBug.Core.Util.Serialization;
using GenericBug.Enums;

namespace GenericBug.Core.UI.WinForms
{
	/// <summary>
	/// This class is used to prevent statically referencing any WinForms dll from the UISelector.cs thus prevents
	/// any unnecessary assembly from getting loaded into the memory.
	/// </summary>
    public static class WinFormsUI
	{
        public static UIDialogResult ShowDialog(UIMode uiMode, SerializableException exception, Report report)
		{
			if (uiMode == UIMode.Minimal)
			{
				return new Minimal().ShowDialog(report);
			}
			else if (uiMode == UIMode.Normal)
			{
				using (var ui = new Normal())
				{
					return ui.ShowDialog(report);
				}
			}
			else if (uiMode == UIMode.Full)
			{
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
