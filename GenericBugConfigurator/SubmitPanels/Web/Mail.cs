using System;
using System.Windows.Forms;

namespace GenericBug.Configurator.SubmitPanels.Web
{
	public partial class Mail : UserControl, ISubmitPanel
	{
		public Mail()
		{
			InitializeComponent();
		}

		public string ConnectionString
		{
			get
			{
				// Check the mendatory fields
				if (this.toListBox.Items.Count == 0)
				{
					MessageBox.Show("Mandatory field \"" + toLabel.Name + "\" cannot be left blank.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return null;
				}

				var mail = new Core.Submission.Web.Mail
                {
                    CustomSubject = this.customSubjectTextBox.Text,
                    CustomBody = this.customBodyTextBox.Text,
                };

				foreach (var item in this.toListBox.Items)
				{
					mail.To += item + ",";
				}

				mail.To = mail.To.TrimEnd(new[] { ',' });
                mail.UseAttachment = this.useAttachmentCheckBox.Checked;
				
				return mail.ConnectionString;
			}

			set
			{
				var mail = new Core.Submission.Web.Mail(value);

				this.customSubjectTextBox.Text = mail.CustomSubject;
				this.customBodyTextBox.Text = mail.CustomBody;
				this.useAttachmentCheckBox.Checked = mail.UseAttachment;

				this.toListBox.Items.Clear();
				if (!string.IsNullOrEmpty(mail.To))
				{
					foreach (var to in mail.To.Split(','))
					{
						this.toListBox.Items.Add(to);
					}
				}
			}
		}

		private void ToAddButton_Click(object sender, EventArgs e)
		{
			this.toListBox.Items.Add(this.toTextBox.Text);
		}

		private void ToRemoveButton_Click(object sender, EventArgs e)
		{
			this.toListBox.Items.RemoveAt(this.toListBox.SelectedIndex);
		}		
	}
}
