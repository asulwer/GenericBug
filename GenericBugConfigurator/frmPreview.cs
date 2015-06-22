using System;
using System.IO;
using System.Windows.Forms;

using GenericBug.Core.Reporting.Info;
using GenericBug.Core.UI.Console;
using GenericBug.Core.UI.WinForms;
using GenericBug.Core.UI.WPF;
using GenericBug.Core.Util.Serialization;
using GenericBug.Enums;

namespace GenericBug.Configurator
{
	internal partial class PreviewForm : Form
	{
		public PreviewForm()
		{
			InitializeComponent();
		}

		internal void ShowDialog(UIMode uiMode, UIProvider uiProvider)
		{
			var exception = new SerializableException(new ArgumentException("Argument exception preview.", new Exception("Inner exception for argument exception.")));
			var report = new Report(exception);

			var consoleOut = new StringWriter();
			Console.SetOut(consoleOut);

			if (uiProvider == UIProvider.Console)
			{
				ConsoleUI.ShowDialog(uiMode, exception, report);
				this.consoleOutputTextBox.Text = consoleOut.ToString();
				this.ShowDialog();
			}
			else if (uiProvider == UIProvider.WinForms)
			{
				WinFormsUI.ShowDialog(uiMode, exception, report);
				this.Close();
			}
			else if (uiProvider == UIProvider.WPF)
			{
				WPFUI.ShowDialog(uiMode, exception, report);
				this.Close();
			}
			else
			{
				throw new ArgumentException("Parameter supplied for UIProvider argument is invalid.");
			}
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
