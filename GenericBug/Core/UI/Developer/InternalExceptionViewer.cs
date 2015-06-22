using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using GenericBug.Core.Util.Exceptions;
using GenericBug.Core.Util.Serialization;
using GenericBug.Properties;

namespace GenericBug.Core.UI.Developer
{
    public partial class InternalExceptionViewer : Form
	{
        public InternalExceptionViewer()
		{
			this.InitializeComponent();
			this.Icon = Resources.icon1;
			this.warningPictureBox.Image = SystemIcons.Warning.ToBitmap();
		}

        public void ShowDialog(Exception exception)
		{
			if (exception is GenericBugConfigurationException)
			{
				this.ShowDialog(exception as GenericBugConfigurationException);
			}
			else if (exception is GenericBugRuntimeException)
			{
				this.ShowDialog(exception as GenericBugRuntimeException);
			}
			else
			{
				this.messageLabel.Text = "An internal runtime exception has occurred. This maybe due to a configuration failure or an internal bug. You may choose to debug the exception or send a bug report to GenericBug developers. You may also use discussion forum to get help.";
				this.bugReportButton.Enabled = true;
				this.DisplayExceptionDetails(exception);
			}
		}

        public void ShowDialog(GenericBugConfigurationException configurationException)
		{
			this.messageLabel.Text = "An internal configuration exception has occurred. Please correct the invalid configuration regarding the information below. You may also use discussion forum to get help or read the online documentation's configuration section.";
			this.invalidSettingLabel.Enabled = true;
			this.invalidSettingTextBox.Enabled = true;
			this.invalidSettingTextBox.Text = configurationException.MisconfiguredProperty;
			this.DisplayExceptionDetails(configurationException);
		}

        public void ShowDialog(GenericBugRuntimeException runtimeException)
		{
			this.messageLabel.Text = "An internal runtime exception has occurred. This maybe due to a configuration failure or an internal bug. You may choose to debug the exception or send a bug report to GenericBug developers. You may also use discussion forum to get help.";
			this.bugReportButton.Enabled = true;
			this.DisplayExceptionDetails(runtimeException);
		}

		private void BugReportButton_Click(object sender, EventArgs e)
		{
			// ToDo: Activate internal bug reporting feature (and add some integrations tests for it)
			/*new BugReport();
			new Dispatcher(false);
			MessageBox.Show("Successfully sent bug report to GenericBug developer community.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
			MessageBox.Show(
				"Internal bug reporting feature is not implemented yet but you can still manually submit a bug report using the bug tracker.", 
				"Information", 
				MessageBoxButtons.OK, 
				MessageBoxIcon.Information);
			this.bugReportButton.Enabled = false;
		}

		private void DebugButton_Click(object sender, EventArgs e)
		{
			// Let the exception propagate down to SEH
			this.Close();
		}

		private void DisplayExceptionDetails(Exception exception)
		{
			this.exceptionTextBox.Text = exception.GetType().ToString();
			this.exceptionMessageTextBox.Text = exception.Message;

			if (exception.TargetSite != null)
			{
				this.targetSiteTextBox.Text = exception.TargetSite.ToString();
			}
			else if (exception.InnerException != null && exception.InnerException.TargetSite != null)
			{
				this.targetSiteTextBox.Text = exception.InnerException.TargetSite.ToString();
			}

			this.exceptionDetails.Initialize(new SerializableException(exception));
			this.ShowDialog();
		}

		private void QuitButton_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}
	}
}
