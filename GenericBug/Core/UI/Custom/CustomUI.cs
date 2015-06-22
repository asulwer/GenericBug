using GenericBug.Core.Reporting.Info;
using GenericBug.Core.Util.Exceptions;
using GenericBug.Core.Util.Serialization;
using GenericBug.Enums;
using GenericBug.Events;

namespace GenericBug.Core.UI.Custom
{
	/// <summary>
	/// This class is used to prevent statically referencing any WPF dlls from the UISelector.cs thus prevents
	/// any unnecessary assembly from getting loaded into the memory.
	/// </summary>
	internal static class CustomUI
	{
		internal static UIDialogResult ShowDialog(UIMode uiMode, SerializableException exception, Report report)
		{
			if (Settings.CustomUIHandle != null)
			{
				var e = new CustomUIEventArgs(uiMode, exception, report);
				Settings.CustomUIHandle.DynamicInvoke(null, e);
				return e.Result;
			}
			else
			{
				throw GenericBugConfigurationException.Create(() => Settings.UIMode, "Parameter supplied for settings property is invalid.");
			}
		}
	}
}