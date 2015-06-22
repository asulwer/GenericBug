using System.Windows.Forms;

using GenericBug.Core.Reporting.Info;

namespace GenericBug.Core.UI.WinForms
{
    public class Minimal
	{
        public UIDialogResult ShowDialog(Report report)
		{
			MessageBox.Show(
				new Form { TopMost = true },
				Settings.Resources.UI_Dialog_Minimal_Message,
				report.GeneralInfo.HostApplication + " Error",
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning);

			return new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.Send);
		}
	}
}
