using System;
using System.Windows.Forms;

using GenericBug.Core.Submission.Web;
using GenericBug.Core.Util;

namespace GenericBug.Configurator.SubmitPanels
{
	public partial class PanelLoader : UserControl
	{
		public string connString;
		public string settingsLoadedProtocolType;

        public event EventHandler RemoveDestination;

		public PanelLoader()
		{
			InitializeComponent();
			this.submitComboBox.SelectedIndex = 0;
		}
        
		public void LoadPanel(string connectionString)
		{
			this.connString = connectionString;
            var protocol = ConnectionStringParser.Parse(connectionString)["Type"];

            if (protocol == typeof(Mail).Name)
            {
                this.submitComboBox.SelectedItem = "E-Mail";
            }
            else if (protocol == typeof(Ftp).Name)
            {
                this.submitComboBox.SelectedItem = "FTP";
            }
            else if (protocol == typeof(Http).Name)
            {
                this.submitComboBox.SelectedItem = "HTTP";
            }
            else if (protocol == typeof(Custom.Custom).Name)
            {
                this.submitComboBox.SelectedItem = "Custom";
            }
            else
            {
                MessageBox.Show("Undefined protocol type was selected. This is an internal error, please notify the developers.");
            }

			this.settingsLoadedProtocolType = this.submitComboBox.Text;

			if (this.Controls.Count == 2)
			{
				((ISubmitPanel)this.Controls[0]).ConnectionString = connectionString;
			}
		}

		public void UnloadPanel()
		{
			this.submitComboBox.SelectedItem = "None";
		}

		private void SubmitComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.Controls.Count == 2)
			{
				this.Controls.RemoveAt(0);
			}

			switch (this.submitComboBox.SelectedItem.ToString())
			{
				case "E-Mail":
					this.Controls.Add(new Web.Mail());
					break;
				case "FTP":
					this.Controls.Add(new Web.Ftp());
					break;
				case "HTTP":
					this.Controls.Add(new Web.Http());
					break;
                case "Custom":
                    this.Controls.Add(new Custom.Custom());
                    break;
			}

			if (this.Controls.Count == 2)
			{
				this.Controls[1].Dock = DockStyle.Top;
				this.Controls[1].BringToFront(); // Note that this swaps Controls[1] -> Controls[0] so submit panel is 0 now!

				if (this.submitComboBox.SelectedItem.ToString() == this.settingsLoadedProtocolType)
				{
					((ISubmitPanel)this.Controls[0]).ConnectionString = this.connString;
				}
			}
		}

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.RemoveDestination.DynamicInvoke(this, new EventArgs());
        }
	}
}
