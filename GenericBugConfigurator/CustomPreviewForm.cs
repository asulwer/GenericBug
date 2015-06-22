using System;
using System.Windows.Forms;

using GenericBug.Core.Reporting.Info;
using GenericBug.Core.UI;

namespace GenericBug.Configurator
{
	internal partial class CustomPreviewForm : Form
	{
		private UIDialogResult uiDialogResult;

		internal CustomPreviewForm()
		{
			this.InitializeComponent();
		}

		internal UIDialogResult ShowDialog(Report report)
		{
			this.Text = string.Format("{0} CustomUI {1}", report.GeneralInfo.HostApplication, Settings.Resources.UI_Dialog_Normal_Title);
			this.exceptionMessageLabel.Text = report.GeneralInfo.ExceptionMessage;

			this.ShowDialog();

			return this.uiDialogResult;
		}

		private void ContinueButton_Click(object sender, EventArgs e)
		{
			this.uiDialogResult = new UIDialogResult(ExecutionFlow.ContinueExecution, SendReport.Send);
			this.Close();
		}

		private void QuitButton_Click(object sender, EventArgs e)
		{
			this.uiDialogResult = new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.Send);
			this.Close();
		}
	}
}