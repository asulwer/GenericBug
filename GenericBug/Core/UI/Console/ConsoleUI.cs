using GenericBug.Core.Reporting.Info;
using GenericBug.Core.Util.Exceptions;
using GenericBug.Core.Util.Serialization;
using GenericBug.Enums;

namespace GenericBug.Core.UI.Console
{
    public static class ConsoleUI
	{
        public static UIDialogResult ShowDialog(UIMode uiMode, SerializableException exception, Report report)
		{
			if (uiMode == UIMode.Minimal)
			{
				// Do not interact with the user
                System.Console.WriteLine(System.Environment.NewLine + Settings.Resources.UI_Console_Minimal_Message);
				return new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.Send);
			}
			else if (uiMode == UIMode.Normal)
			{
				// ToDo: Create normal console UI
                System.Console.WriteLine(System.Environment.NewLine + Settings.Resources.UI_Console_Normal_Message);
				return new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.Send);
			}
			else if (uiMode == UIMode.Full)
			{
				// ToDo: Create full console UI
                System.Console.WriteLine(System.Environment.NewLine + Settings.Resources.UI_Console_Full_Message);
				return new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.Send);
			}
			else
			{
				throw GenericBugConfigurationException.Create(() => Settings.UIMode, "Parameter supplied for settings property is invalid.");
			}
		}
	}
}
