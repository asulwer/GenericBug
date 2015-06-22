using GenericBug.Core.UI.WinForms.Panels;

namespace GenericBug.Core.UI.Developer
{
	partial class InternalExceptionViewer
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.quitButton = new System.Windows.Forms.Button();
            this.debugButton = new System.Windows.Forms.Button();
            this.bugReportButton = new System.Windows.Forms.Button();
            this.messageLabel = new System.Windows.Forms.Label();
            this.exceptionLabel = new System.Windows.Forms.Label();
            this.exceptionTextBox = new System.Windows.Forms.TextBox();
            this.invalidSettingLabel = new System.Windows.Forms.Label();
            this.invalidSettingTextBox = new System.Windows.Forms.TextBox();
            this.targetSiteTextBox = new System.Windows.Forms.TextBox();
            this.targetSiteLabel = new System.Windows.Forms.Label();
            this.exceptionMessageTextBox = new System.Windows.Forms.TextBox();
            this.exceptionStackGroupBox = new System.Windows.Forms.GroupBox();
            this.exceptionDetails = new GenericBug.Core.UI.WinForms.Panels.ExceptionDetails();
            this.warningPictureBox = new System.Windows.Forms.PictureBox();
            this.exceptionStackGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // quitButton
            // 
            this.quitButton.Location = new System.Drawing.Point(454, 507);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(75, 23);
            this.quitButton.TabIndex = 0;
            this.quitButton.Text = "&Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // debugButton
            // 
            this.debugButton.Image = global::GenericBug.Properties.Resources.VS2010;
            this.debugButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.debugButton.Location = new System.Drawing.Point(363, 507);
            this.debugButton.Name = "debugButton";
            this.debugButton.Size = new System.Drawing.Size(85, 23);
            this.debugButton.TabIndex = 1;
            this.debugButton.Text = "  &Debug";
            this.debugButton.UseVisualStyleBackColor = true;
            this.debugButton.Click += new System.EventHandler(this.DebugButton_Click);
            // 
            // bugReportButton
            // 
            this.bugReportButton.Enabled = false;
            this.bugReportButton.Image = global::GenericBug.Properties.Resources.Send;
            this.bugReportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bugReportButton.Location = new System.Drawing.Point(229, 507);
            this.bugReportButton.Name = "bugReportButton";
            this.bugReportButton.Size = new System.Drawing.Size(128, 23);
            this.bugReportButton.TabIndex = 2;
            this.bugReportButton.Text = "    &Send Bug Report";
            this.bugReportButton.UseVisualStyleBackColor = true;
            this.bugReportButton.Click += new System.EventHandler(this.BugReportButton_Click);
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(50, 9);
            this.messageLabel.MaximumSize = new System.Drawing.Size(465, 39);
            this.messageLabel.MinimumSize = new System.Drawing.Size(465, 39);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(465, 39);
            this.messageLabel.TabIndex = 3;
            this.messageLabel.Text = "A configuration or runtime exception has occured.";
            // 
            // exceptionLabel
            // 
            this.exceptionLabel.AutoSize = true;
            this.exceptionLabel.Location = new System.Drawing.Point(10, 73);
            this.exceptionLabel.Name = "exceptionLabel";
            this.exceptionLabel.Size = new System.Drawing.Size(57, 13);
            this.exceptionLabel.TabIndex = 6;
            this.exceptionLabel.Text = "Exception:";
            // 
            // exceptionTextBox
            // 
            this.exceptionTextBox.Location = new System.Drawing.Point(73, 70);
            this.exceptionTextBox.Name = "exceptionTextBox";
            this.exceptionTextBox.Size = new System.Drawing.Size(226, 20);
            this.exceptionTextBox.TabIndex = 7;
            // 
            // invalidSettingLabel
            // 
            this.invalidSettingLabel.AutoSize = true;
            this.invalidSettingLabel.Enabled = false;
            this.invalidSettingLabel.Location = new System.Drawing.Point(319, 73);
            this.invalidSettingLabel.Name = "invalidSettingLabel";
            this.invalidSettingLabel.Size = new System.Drawing.Size(77, 13);
            this.invalidSettingLabel.TabIndex = 8;
            this.invalidSettingLabel.Text = "Invalid Setting:";
            // 
            // invalidSettingTextBox
            // 
            this.invalidSettingTextBox.Enabled = false;
            this.invalidSettingTextBox.Location = new System.Drawing.Point(402, 70);
            this.invalidSettingTextBox.Name = "invalidSettingTextBox";
            this.invalidSettingTextBox.Size = new System.Drawing.Size(121, 20);
            this.invalidSettingTextBox.TabIndex = 9;
            // 
            // targetSiteTextBox
            // 
            this.targetSiteTextBox.Location = new System.Drawing.Point(73, 99);
            this.targetSiteTextBox.Name = "targetSiteTextBox";
            this.targetSiteTextBox.Size = new System.Drawing.Size(450, 20);
            this.targetSiteTextBox.TabIndex = 11;
            // 
            // targetSiteLabel
            // 
            this.targetSiteLabel.AutoSize = true;
            this.targetSiteLabel.Location = new System.Drawing.Point(9, 102);
            this.targetSiteLabel.Name = "targetSiteLabel";
            this.targetSiteLabel.Size = new System.Drawing.Size(62, 13);
            this.targetSiteLabel.TabIndex = 10;
            this.targetSiteLabel.Text = "Target Site:";
            // 
            // exceptionMessageTextBox
            // 
            this.exceptionMessageTextBox.Location = new System.Drawing.Point(14, 125);
            this.exceptionMessageTextBox.Multiline = true;
            this.exceptionMessageTextBox.Name = "exceptionMessageTextBox";
            this.exceptionMessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.exceptionMessageTextBox.Size = new System.Drawing.Size(510, 61);
            this.exceptionMessageTextBox.TabIndex = 12;
            // 
            // exceptionStackGroupBox
            // 
            this.exceptionStackGroupBox.Controls.Add(this.exceptionDetails);
            this.exceptionStackGroupBox.Location = new System.Drawing.Point(14, 198);
            this.exceptionStackGroupBox.Name = "exceptionStackGroupBox";
            this.exceptionStackGroupBox.Size = new System.Drawing.Size(515, 303);
            this.exceptionStackGroupBox.TabIndex = 13;
            this.exceptionStackGroupBox.TabStop = false;
            this.exceptionStackGroupBox.Text = "Full Exception Stack";
            // 
            // exceptionDetails
            // 
            this.exceptionDetails.InformationColumnWidth = 350;
            this.exceptionDetails.Location = new System.Drawing.Point(6, 16);
            this.exceptionDetails.Name = "exceptionDetails";
            this.exceptionDetails.PropertyColumnWidth = 144;
            this.exceptionDetails.Size = new System.Drawing.Size(504, 281);
            this.exceptionDetails.TabIndex = 0;
            // 
            // warningPictureBox
            // 
            this.warningPictureBox.Location = new System.Drawing.Point(12, 12);
            this.warningPictureBox.Name = "warningPictureBox";
            this.warningPictureBox.Size = new System.Drawing.Size(32, 32);
            this.warningPictureBox.TabIndex = 14;
            this.warningPictureBox.TabStop = false;
            // 
            // InternalExceptionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 542);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.bugReportButton);
            this.Controls.Add(this.warningPictureBox);
            this.Controls.Add(this.debugButton);
            this.Controls.Add(this.exceptionStackGroupBox);
            this.Controls.Add(this.exceptionMessageTextBox);
            this.Controls.Add(this.targetSiteTextBox);
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.targetSiteLabel);
            this.Controls.Add(this.invalidSettingLabel);
            this.Controls.Add(this.exceptionLabel);
            this.Controls.Add(this.exceptionTextBox);
            this.Controls.Add(this.invalidSettingTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InternalExceptionViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GenericBug Internal Exception Viewer";
            this.TopMost = true;
            this.exceptionStackGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.warningPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button quitButton;
		private System.Windows.Forms.Button debugButton;
		private System.Windows.Forms.Button bugReportButton;
        private System.Windows.Forms.Label messageLabel;
		private System.Windows.Forms.Label exceptionLabel;
		private System.Windows.Forms.TextBox exceptionTextBox;
		private System.Windows.Forms.Label invalidSettingLabel;
		private System.Windows.Forms.TextBox invalidSettingTextBox;
		private System.Windows.Forms.TextBox targetSiteTextBox;
		private System.Windows.Forms.Label targetSiteLabel;
		private System.Windows.Forms.TextBox exceptionMessageTextBox;
		private System.Windows.Forms.GroupBox exceptionStackGroupBox;
		private ExceptionDetails exceptionDetails;
		private System.Windows.Forms.PictureBox warningPictureBox;
	}
}