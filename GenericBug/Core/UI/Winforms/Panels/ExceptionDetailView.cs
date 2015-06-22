using System;
using System.Windows.Forms;

namespace GenericBug.Core.UI.WinForms.Panels
{
    public partial class ExceptionDetailView : Form
	{
		public ExceptionDetailView()
		{
			this.InitializeComponent();
		}

        public void ShowDialog(string property, string info)
		{
			this.propertyTextBox.Text = property;
			this.propertyInformationTextBox.Text = info;
			this.ShowDialog();
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
