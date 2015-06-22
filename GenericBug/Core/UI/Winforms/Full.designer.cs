namespace GenericBug.Core.UI.WinForms
{
	using GenericBug.Core.UI.WinForms.Panels;

	partial class Full
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Full));
            this.mainTabs = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.warningLabel = new System.Windows.Forms.Label();
            this.exceptionTypeLabel = new System.Windows.Forms.Label();
            this.exceptionTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.errorDescriptionLabel = new System.Windows.Forms.Label();
            this.clrTextBox = new System.Windows.Forms.TextBox();
            this.clrLabel = new System.Windows.Forms.Label();
            this.dateTimeTextBox = new System.Windows.Forms.TextBox();
            this.dateTimeLabel = new System.Windows.Forms.Label();
            this.GenericBugTextBox = new System.Windows.Forms.TextBox();
            this.GenericBugLabel = new System.Windows.Forms.Label();
            this.applicationTextBox = new System.Windows.Forms.TextBox();
            this.applicationLabel = new System.Windows.Forms.Label();
            this.targetSiteTextBox = new System.Windows.Forms.TextBox();
            this.targetSiteLabel = new System.Windows.Forms.Label();
            this.exceptionMessageTextBox = new System.Windows.Forms.TextBox();
            this.warningPictureBox = new System.Windows.Forms.PictureBox();
            this.exceptionTabPage = new System.Windows.Forms.TabPage();
            this.exceptionDetails = new GenericBug.Core.UI.WinForms.Panels.ExceptionDetails();
            this.sendAndQuitButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.mainTabs.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningPictureBox)).BeginInit();
            this.exceptionTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.generalTabPage);
            this.mainTabs.Controls.Add(this.exceptionTabPage);
            this.mainTabs.Location = new System.Drawing.Point(9, 6);
            this.mainTabs.Margin = new System.Windows.Forms.Padding(0);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.SelectedIndex = 0;
            this.mainTabs.Size = new System.Drawing.Size(475, 361);
            this.mainTabs.TabIndex = 0;
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.warningLabel);
            this.generalTabPage.Controls.Add(this.exceptionTypeLabel);
            this.generalTabPage.Controls.Add(this.exceptionTextBox);
            this.generalTabPage.Controls.Add(this.descriptionTextBox);
            this.generalTabPage.Controls.Add(this.errorDescriptionLabel);
            this.generalTabPage.Controls.Add(this.clrTextBox);
            this.generalTabPage.Controls.Add(this.clrLabel);
            this.generalTabPage.Controls.Add(this.dateTimeTextBox);
            this.generalTabPage.Controls.Add(this.dateTimeLabel);
            this.generalTabPage.Controls.Add(this.GenericBugTextBox);
            this.generalTabPage.Controls.Add(this.GenericBugLabel);
            this.generalTabPage.Controls.Add(this.applicationTextBox);
            this.generalTabPage.Controls.Add(this.applicationLabel);
            this.generalTabPage.Controls.Add(this.targetSiteTextBox);
            this.generalTabPage.Controls.Add(this.targetSiteLabel);
            this.generalTabPage.Controls.Add(this.exceptionMessageTextBox);
            this.generalTabPage.Controls.Add(this.warningPictureBox);
            this.generalTabPage.Location = new System.Drawing.Point(4, 22);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalTabPage.Size = new System.Drawing.Size(467, 335);
            this.generalTabPage.TabIndex = 0;
            this.generalTabPage.Text = "General";
            this.generalTabPage.UseVisualStyleBackColor = true;
            // 
            // warningLabel
            // 
            this.warningLabel.Location = new System.Drawing.Point(64, 12);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(388, 43);
            this.warningLabel.TabIndex = 18;
            this.warningLabel.Text = resources.GetString("warningLabel.Text");
            // 
            // exceptionTypeLabel
            // 
            this.exceptionTypeLabel.Image = global::GenericBug.Properties.Resources.Icon;
            this.exceptionTypeLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.exceptionTypeLabel.Location = new System.Drawing.Point(21, 69);
            this.exceptionTypeLabel.Name = "exceptionTypeLabel";
            this.exceptionTypeLabel.Size = new System.Drawing.Size(106, 16);
            this.exceptionTypeLabel.TabIndex = 17;
            this.exceptionTypeLabel.Text = "Exception Type:";
            this.exceptionTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // exceptionTextBox
            // 
            this.exceptionTextBox.Location = new System.Drawing.Point(135, 68);
            this.exceptionTextBox.Name = "exceptionTextBox";
            this.exceptionTextBox.Size = new System.Drawing.Size(317, 20);
            this.exceptionTextBox.TabIndex = 2;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(13, 267);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(439, 60);
            this.descriptionTextBox.TabIndex = 15;
            // 
            // errorDescriptionLabel
            // 
            this.errorDescriptionLabel.AutoSize = true;
            this.errorDescriptionLabel.Location = new System.Drawing.Point(10, 251);
            this.errorDescriptionLabel.Name = "errorDescriptionLabel";
            this.errorDescriptionLabel.Size = new System.Drawing.Size(315, 13);
            this.errorDescriptionLabel.TabIndex = 14;
            this.errorDescriptionLabel.Text = "Please add a brief description of how we can reproduce the error:";
            // 
            // clrTextBox
            // 
            this.clrTextBox.Location = new System.Drawing.Point(307, 216);
            this.clrTextBox.Name = "clrTextBox";
            this.clrTextBox.Size = new System.Drawing.Size(145, 20);
            this.clrTextBox.TabIndex = 13;
            // 
            // clrLabel
            // 
            this.clrLabel.AutoSize = true;
            this.clrLabel.Location = new System.Drawing.Point(259, 219);
            this.clrLabel.Name = "clrLabel";
            this.clrLabel.Size = new System.Drawing.Size(31, 13);
            this.clrLabel.TabIndex = 12;
            this.clrLabel.Text = "CLR:";
            // 
            // dateTimeTextBox
            // 
            this.dateTimeTextBox.Location = new System.Drawing.Point(78, 216);
            this.dateTimeTextBox.Name = "dateTimeTextBox";
            this.dateTimeTextBox.Size = new System.Drawing.Size(145, 20);
            this.dateTimeTextBox.TabIndex = 11;
            // 
            // dateTimeLabel
            // 
            this.dateTimeLabel.AutoSize = true;
            this.dateTimeLabel.Location = new System.Drawing.Point(10, 219);
            this.dateTimeLabel.Name = "dateTimeLabel";
            this.dateTimeLabel.Size = new System.Drawing.Size(61, 13);
            this.dateTimeLabel.TabIndex = 10;
            this.dateTimeLabel.Text = "Date/Time:";
            // 
            // GenericBugTextBox
            // 
            this.GenericBugTextBox.Location = new System.Drawing.Point(307, 182);
            this.GenericBugTextBox.Name = "GenericBugTextBox";
            this.GenericBugTextBox.Size = new System.Drawing.Size(145, 20);
            this.GenericBugTextBox.TabIndex = 9;
            // 
            // GenericBugLabel
            // 
            this.GenericBugLabel.AutoSize = true;
            this.GenericBugLabel.Location = new System.Drawing.Point(235, 185);
            this.GenericBugLabel.Name = "GenericBugLabel";
            this.GenericBugLabel.Size = new System.Drawing.Size(66, 13);
            this.GenericBugLabel.TabIndex = 8;
            this.GenericBugLabel.Text = "GenericBug:";
            // 
            // applicationTextBox
            // 
            this.applicationTextBox.Location = new System.Drawing.Point(78, 182);
            this.applicationTextBox.Name = "applicationTextBox";
            this.applicationTextBox.Size = new System.Drawing.Size(145, 20);
            this.applicationTextBox.TabIndex = 7;
            // 
            // applicationLabel
            // 
            this.applicationLabel.AutoSize = true;
            this.applicationLabel.Location = new System.Drawing.Point(10, 185);
            this.applicationLabel.Name = "applicationLabel";
            this.applicationLabel.Size = new System.Drawing.Size(62, 13);
            this.applicationLabel.TabIndex = 6;
            this.applicationLabel.Text = "Application:";
            // 
            // targetSiteTextBox
            // 
            this.targetSiteTextBox.Location = new System.Drawing.Point(78, 148);
            this.targetSiteTextBox.Name = "targetSiteTextBox";
            this.targetSiteTextBox.Size = new System.Drawing.Size(374, 20);
            this.targetSiteTextBox.TabIndex = 5;
            // 
            // targetSiteLabel
            // 
            this.targetSiteLabel.AutoSize = true;
            this.targetSiteLabel.Location = new System.Drawing.Point(10, 151);
            this.targetSiteLabel.Name = "targetSiteLabel";
            this.targetSiteLabel.Size = new System.Drawing.Size(62, 13);
            this.targetSiteLabel.TabIndex = 4;
            this.targetSiteLabel.Text = "Target Site:";
            // 
            // exceptionMessageTextBox
            // 
            this.exceptionMessageTextBox.Location = new System.Drawing.Point(13, 98);
            this.exceptionMessageTextBox.Multiline = true;
            this.exceptionMessageTextBox.Name = "exceptionMessageTextBox";
            this.exceptionMessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.exceptionMessageTextBox.Size = new System.Drawing.Size(439, 35);
            this.exceptionMessageTextBox.TabIndex = 3;
            // 
            // warningPictureBox
            // 
            this.warningPictureBox.Location = new System.Drawing.Point(16, 15);
            this.warningPictureBox.Name = "warningPictureBox";
            this.warningPictureBox.Size = new System.Drawing.Size(32, 32);
            this.warningPictureBox.TabIndex = 1;
            this.warningPictureBox.TabStop = false;
            // 
            // exceptionTabPage
            // 
            this.exceptionTabPage.Controls.Add(this.exceptionDetails);
            this.exceptionTabPage.Location = new System.Drawing.Point(4, 22);
            this.exceptionTabPage.Name = "exceptionTabPage";
            this.exceptionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.exceptionTabPage.Size = new System.Drawing.Size(467, 335);
            this.exceptionTabPage.TabIndex = 2;
            this.exceptionTabPage.Text = "Exception";
            this.exceptionTabPage.UseVisualStyleBackColor = true;
            // 
            // exceptionDetails
            // 
            this.exceptionDetails.InformationColumnWidth = 350;
            this.exceptionDetails.Location = new System.Drawing.Point(3, 3);
            this.exceptionDetails.Name = "exceptionDetails";
            this.exceptionDetails.PropertyColumnWidth = 101;
            this.exceptionDetails.Size = new System.Drawing.Size(461, 330);
            this.exceptionDetails.TabIndex = 0;
            // 
            // sendAndQuitButton
            // 
            this.sendAndQuitButton.Image = global::GenericBug.Properties.Resources.Send;
            this.sendAndQuitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sendAndQuitButton.Location = new System.Drawing.Point(382, 374);
            this.sendAndQuitButton.Name = "sendAndQuitButton";
            this.sendAndQuitButton.Size = new System.Drawing.Size(102, 23);
            this.sendAndQuitButton.TabIndex = 1;
            this.sendAndQuitButton.Text = "&Send and Quit";
            this.sendAndQuitButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sendAndQuitButton.UseVisualStyleBackColor = true;
            this.sendAndQuitButton.Click += new System.EventHandler(this.SendAndQuitButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.quitButton.Location = new System.Drawing.Point(296, 374);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(75, 23);
            this.quitButton.TabIndex = 2;
            this.quitButton.Text = "&Quit";
            this.quitButton.UseVisualStyleBackColor = true;
            this.quitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 100;
            this.toolTip.UseAnimation = false;
            this.toolTip.UseFading = false;
            // 
            // Full
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.quitButton;
            this.ClientSize = new System.Drawing.Size(494, 403);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.sendAndQuitButton);
            this.Controls.Add(this.mainTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Full";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "{HostApplication} Error";
            this.TopMost = true;
            this.mainTabs.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.generalTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningPictureBox)).EndInit();
            this.exceptionTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl mainTabs;
		private System.Windows.Forms.TabPage generalTabPage;
		private System.Windows.Forms.Button sendAndQuitButton;
		private System.Windows.Forms.Button quitButton;
		private System.Windows.Forms.TabPage exceptionTabPage;
		private System.Windows.Forms.PictureBox warningPictureBox;
		private System.Windows.Forms.TextBox exceptionMessageTextBox;
		private System.Windows.Forms.TextBox exceptionTextBox;
		private System.Windows.Forms.TextBox targetSiteTextBox;
		private System.Windows.Forms.Label targetSiteLabel;
		private System.Windows.Forms.TextBox GenericBugTextBox;
		private System.Windows.Forms.Label GenericBugLabel;
		private System.Windows.Forms.TextBox applicationTextBox;
		private System.Windows.Forms.Label applicationLabel;
		private System.Windows.Forms.TextBox descriptionTextBox;
		private System.Windows.Forms.Label errorDescriptionLabel;
		private System.Windows.Forms.TextBox clrTextBox;
		private System.Windows.Forms.Label clrLabel;
		private System.Windows.Forms.TextBox dateTimeTextBox;
        private System.Windows.Forms.Label dateTimeLabel;
		private System.Windows.Forms.Label exceptionTypeLabel;
		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label warningLabel;
        private ExceptionDetails exceptionDetails;
	}
}