using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using GenericBug.Enums;
using GenericBug.Properties;

namespace GenericBug.Core.UI.Developer
{
    public partial class InternalLogViewer : Form
	{
		private static bool initialized;
		private static bool closed;
		private static InternalLogViewer viewer;
		private static ManualResetEvent handleCreated;

        public InternalLogViewer()
		{
			this.InitializeComponent();
			this.Icon = Resources.icon1;
			this.notifyIcon.Icon = Resources.icon1;
		}

		public static void InitializeInternalLogViewer()
		{
			if (!initialized)
			{
				initialized = true;
				viewer = new InternalLogViewer();
				handleCreated = new ManualResetEvent(false);
				viewer.HandleCreated += (sender, e) => handleCreated.Set();
				Task.Factory.StartNew(() => Application.Run(viewer));
				handleCreated.WaitOne();
			}
		}

		public static void LogEntry(string message, LoggerCategory category)
		{
			InitializeInternalLogViewer();

			if (!closed)
			{
				viewer.Invoke((MethodInvoker)(() => viewer.InternalLogEntry(message, category)));
			}
		}

        public void InternalLogEntry(string message, LoggerCategory category)
		{
            //remove GenericBug from Category String before adding to list control
			this.loggerListView.Items.Add(new ListViewItem(new[] { category.ToString().Remove(0, 10), DateTime.Now.ToString("HH:mm:ss"), message }));
		}

		private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState = FormWindowState.Normal;
			}

			this.Show();
			this.Activate();
		}

		private void LoggerListView_Click(object sender, EventArgs e)
		{
			this.detailsTextBox.Text = this.loggerListView.SelectedItems[0].SubItems[2].Text;
		}

		private void QuitButton_Click(object sender, EventArgs e)
		{
			this.notifyIcon.Visible = false;
			this.notifyIcon.Dispose();
			this.notifyIcon = null;
			closed = true;
			this.Close();
		}

		private void HideButton_Click(object sender, EventArgs e)
		{
			this.Hide();
		}

		private void InternalLogViewer_Resize(object sender, EventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
			{
				this.Hide();
			}
		}
	}
}