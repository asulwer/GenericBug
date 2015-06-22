using System;
using System.Drawing;
using System.Windows.Forms;

using GenericBug.Core.Reporting.Info;
using GenericBug.Core.Util.Serialization;

namespace GenericBug.Core.UI.WinForms
{
    public partial class Full : Form
	{
		private UIDialogResult uiDialogResult;

        public Full()
		{
			InitializeComponent();
			this.Icon = Properties.Resources.icon1;
			this.warningLabel.Text = Settings.Resources.UI_Dialog_Full_Message;
			this.generalTabPage.Text = Settings.Resources.UI_Dialog_Full_General_Tab;
			this.exceptionTabPage.Text = Settings.Resources.UI_Dialog_Full_Exception_Tab;
			this.errorDescriptionLabel.Text = Settings.Resources.UI_Dialog_Full_How_to_Reproduce_the_Error_Notification;
			this.quitButton.Text = Settings.Resources.UI_Dialog_Full_Quit_Button;
			this.sendAndQuitButton.Text = Settings.Resources.UI_Dialog_Full_Send_and_Quit_Button;
		}

        public UIDialogResult ShowDialog(SerializableException exception, Report report)
		{
			this.Text = string.Format("{0} {1}", report.GeneralInfo.HostApplication, Settings.Resources.UI_Dialog_Full_Title);
			
			// Fill in the 'General' tab
			warningPictureBox.Image = SystemIcons.Warning.ToBitmap();
			this.exceptionTextBox.Text = exception.Type;
			this.exceptionMessageTextBox.Text = exception.Message;
			this.targetSiteTextBox.Text = exception.TargetSite;
			this.applicationTextBox.Text = report.GeneralInfo.HostApplication + " [" + report.GeneralInfo.HostApplicationVersion + "]";
			this.GenericBugTextBox.Text = report.GeneralInfo.GenericBugVersion.ToString();
			this.dateTimeTextBox.Text = report.GeneralInfo.DateTime.ToString();
			this.clrTextBox.Text = report.GeneralInfo.CLRVersion.ToString();
			
			// Fill in the 'Exception' tab
			this.exceptionDetails.Initialize(exception);
			
			// ToDo: Fill in the 'Report Contents' tab);

			this.ShowDialog();

			// Write back the user description (as we passed 'report' as a reference since it is a refence object anyway)
			report.GeneralInfo.UserDescription = this.descriptionTextBox.Text;
			return this.uiDialogResult;
		}

		private void SendAndQuitButton_Click(object sender, EventArgs e)
		{
			this.uiDialogResult = new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.Send);
			this.Close();
		}

		private void QuitButton_Click(object sender, EventArgs e)
		{
			this.uiDialogResult = new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.DoNotSend);
			this.Close();
		}
	}
}
