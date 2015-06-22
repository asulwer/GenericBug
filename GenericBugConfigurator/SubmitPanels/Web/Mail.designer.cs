namespace GenericBug.Configurator.SubmitPanels.Web
{
	partial class Mail
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.toLabel = new System.Windows.Forms.Label();
            this.toListBox = new System.Windows.Forms.ListBox();
            this.toAddButton = new System.Windows.Forms.Button();
            this.toRemoveButton = new System.Windows.Forms.Button();
            this.toTextBox = new System.Windows.Forms.TextBox();
            this.customSubjectTextBox = new System.Windows.Forms.TextBox();
            this.customSubjectLabel = new System.Windows.Forms.Label();
            this.customBodyTextBox = new System.Windows.Forms.TextBox();
            this.customBodyLabel = new System.Windows.Forms.Label();
            this.useAttachmentCheckBox = new System.Windows.Forms.CheckBox();
            this.noteLabel = new System.Windows.Forms.Label();
            this.mailGroupBox = new System.Windows.Forms.GroupBox();
            this.recepientsGroupBox = new System.Windows.Forms.GroupBox();
            this.attachmentsGroupBox = new System.Windows.Forms.GroupBox();
            this.mailGroupBox.SuspendLayout();
            this.recepientsGroupBox.SuspendLayout();
            this.attachmentsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // toLabel
            // 
            this.toLabel.AutoSize = true;
            this.toLabel.Location = new System.Drawing.Point(6, 24);
            this.toLabel.Name = "toLabel";
            this.toLabel.Size = new System.Drawing.Size(27, 13);
            this.toLabel.TabIndex = 4;
            this.toLabel.Text = "To*:";
            // 
            // toListBox
            // 
            this.toListBox.FormattingEnabled = true;
            this.toListBox.Location = new System.Drawing.Point(39, 47);
            this.toListBox.Name = "toListBox";
            this.toListBox.Size = new System.Drawing.Size(171, 82);
            this.toListBox.TabIndex = 5;
            // 
            // toAddButton
            // 
            this.toAddButton.Location = new System.Drawing.Point(216, 21);
            this.toAddButton.Name = "toAddButton";
            this.toAddButton.Size = new System.Drawing.Size(61, 20);
            this.toAddButton.TabIndex = 7;
            this.toAddButton.Text = "Add";
            this.toAddButton.UseVisualStyleBackColor = true;
            this.toAddButton.Click += new System.EventHandler(this.ToAddButton_Click);
            // 
            // toRemoveButton
            // 
            this.toRemoveButton.Location = new System.Drawing.Point(216, 84);
            this.toRemoveButton.Name = "toRemoveButton";
            this.toRemoveButton.Size = new System.Drawing.Size(61, 20);
            this.toRemoveButton.TabIndex = 8;
            this.toRemoveButton.Text = "Remove";
            this.toRemoveButton.UseVisualStyleBackColor = true;
            this.toRemoveButton.Click += new System.EventHandler(this.ToRemoveButton_Click);
            // 
            // toTextBox
            // 
            this.toTextBox.Location = new System.Drawing.Point(39, 21);
            this.toTextBox.Name = "toTextBox";
            this.toTextBox.Size = new System.Drawing.Size(171, 20);
            this.toTextBox.TabIndex = 9;
            // 
            // customSubjectTextBox
            // 
            this.customSubjectTextBox.Location = new System.Drawing.Point(6, 36);
            this.customSubjectTextBox.Name = "customSubjectTextBox";
            this.customSubjectTextBox.Size = new System.Drawing.Size(229, 20);
            this.customSubjectTextBox.TabIndex = 34;
            // 
            // customSubjectLabel
            // 
            this.customSubjectLabel.AutoSize = true;
            this.customSubjectLabel.Location = new System.Drawing.Point(6, 19);
            this.customSubjectLabel.Name = "customSubjectLabel";
            this.customSubjectLabel.Size = new System.Drawing.Size(84, 13);
            this.customSubjectLabel.TabIndex = 33;
            this.customSubjectLabel.Text = "Custom Subject:";
            // 
            // customBodyTextBox
            // 
            this.customBodyTextBox.Location = new System.Drawing.Point(6, 76);
            this.customBodyTextBox.Multiline = true;
            this.customBodyTextBox.Name = "customBodyTextBox";
            this.customBodyTextBox.Size = new System.Drawing.Size(229, 60);
            this.customBodyTextBox.TabIndex = 36;
            // 
            // customBodyLabel
            // 
            this.customBodyLabel.AutoSize = true;
            this.customBodyLabel.Location = new System.Drawing.Point(6, 59);
            this.customBodyLabel.Name = "customBodyLabel";
            this.customBodyLabel.Size = new System.Drawing.Size(72, 13);
            this.customBodyLabel.TabIndex = 35;
            this.customBodyLabel.Text = "Custom Body:";
            // 
            // useAttachmentCheckBox
            // 
            this.useAttachmentCheckBox.AutoSize = true;
            this.useAttachmentCheckBox.Checked = true;
            this.useAttachmentCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useAttachmentCheckBox.Location = new System.Drawing.Point(10, 24);
            this.useAttachmentCheckBox.Name = "useAttachmentCheckBox";
            this.useAttachmentCheckBox.Size = new System.Drawing.Size(231, 17);
            this.useAttachmentCheckBox.TabIndex = 39;
            this.useAttachmentCheckBox.Text = "Attach compressed error to generated email";
            this.useAttachmentCheckBox.UseVisualStyleBackColor = true;
            // 
            // noteLabel
            // 
            this.noteLabel.AutoSize = true;
            this.noteLabel.Location = new System.Drawing.Point(388, 155);
            this.noteLabel.Name = "noteLabel";
            this.noteLabel.Size = new System.Drawing.Size(162, 13);
            this.noteLabel.TabIndex = 40;
            this.noteLabel.Text = "Note: * Denotes mendatory fields";
            // 
            // mailGroupBox
            // 
            this.mailGroupBox.Controls.Add(this.customSubjectLabel);
            this.mailGroupBox.Controls.Add(this.customSubjectTextBox);
            this.mailGroupBox.Controls.Add(this.customBodyLabel);
            this.mailGroupBox.Controls.Add(this.customBodyTextBox);
            this.mailGroupBox.Location = new System.Drawing.Point(3, 3);
            this.mailGroupBox.Name = "mailGroupBox";
            this.mailGroupBox.Padding = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.mailGroupBox.Size = new System.Drawing.Size(249, 146);
            this.mailGroupBox.TabIndex = 42;
            this.mailGroupBox.TabStop = false;
            this.mailGroupBox.Text = "Mail";
            // 
            // recepientsGroupBox
            // 
            this.recepientsGroupBox.Controls.Add(this.toLabel);
            this.recepientsGroupBox.Controls.Add(this.toListBox);
            this.recepientsGroupBox.Controls.Add(this.toAddButton);
            this.recepientsGroupBox.Controls.Add(this.toRemoveButton);
            this.recepientsGroupBox.Controls.Add(this.toTextBox);
            this.recepientsGroupBox.Location = new System.Drawing.Point(258, 3);
            this.recepientsGroupBox.Name = "recepientsGroupBox";
            this.recepientsGroupBox.Size = new System.Drawing.Size(292, 146);
            this.recepientsGroupBox.TabIndex = 42;
            this.recepientsGroupBox.TabStop = false;
            this.recepientsGroupBox.Text = "Recepients";
            // 
            // attachmentsGroupBox
            // 
            this.attachmentsGroupBox.Controls.Add(this.useAttachmentCheckBox);
            this.attachmentsGroupBox.Location = new System.Drawing.Point(3, 155);
            this.attachmentsGroupBox.Name = "attachmentsGroupBox";
            this.attachmentsGroupBox.Size = new System.Drawing.Size(249, 59);
            this.attachmentsGroupBox.TabIndex = 43;
            this.attachmentsGroupBox.TabStop = false;
            this.attachmentsGroupBox.Text = "Attachments";
            // 
            // Mail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.attachmentsGroupBox);
            this.Controls.Add(this.mailGroupBox);
            this.Controls.Add(this.recepientsGroupBox);
            this.Controls.Add(this.noteLabel);
            this.Name = "Mail";
            this.Size = new System.Drawing.Size(558, 219);
            this.mailGroupBox.ResumeLayout(false);
            this.mailGroupBox.PerformLayout();
            this.recepientsGroupBox.ResumeLayout(false);
            this.recepientsGroupBox.PerformLayout();
            this.attachmentsGroupBox.ResumeLayout(false);
            this.attachmentsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label toLabel;
		private System.Windows.Forms.ListBox toListBox;
		private System.Windows.Forms.Button toAddButton;
        private System.Windows.Forms.Button toRemoveButton;
        private System.Windows.Forms.TextBox toTextBox;
		private System.Windows.Forms.TextBox customSubjectTextBox;
		private System.Windows.Forms.Label customSubjectLabel;
		private System.Windows.Forms.TextBox customBodyTextBox;
        private System.Windows.Forms.Label customBodyLabel;
		private System.Windows.Forms.CheckBox useAttachmentCheckBox;
        private System.Windows.Forms.Label noteLabel;
        private System.Windows.Forms.GroupBox mailGroupBox;
		private System.Windows.Forms.GroupBox recepientsGroupBox;
        private System.Windows.Forms.GroupBox attachmentsGroupBox;
	}
}
