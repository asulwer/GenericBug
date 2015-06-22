using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Configuration;

using GenericBug.Configurator.SubmitPanels;
using GenericBug.Core.Submission;
using GenericBug.Core.Submission.Web;
using GenericBug.Core.Util;
using GenericBug.Enums;
using GenericBug.Events;
using GenericBug.Properties;
using GenericBug.SettingsCollection;

namespace GenericBug.Configurator
{	
	public partial class frmMain : Form
	{
        private string fileName;

        private readonly ICollection<PanelLoader> panelLoaders = new Collection<PanelLoader>();

		private void InitializeControls()
        {
            this.uiModeComboBox.Items.Clear();
            foreach (UIMode value in Enum.GetValues(typeof(UIMode)))
            {
                this.uiModeComboBox.Items.Add(value);
            }

            this.uiProviderComboBox.Items.Clear();
            foreach (UIProvider value in Enum.GetValues(typeof(UIProvider)))
            {
                this.uiProviderComboBox.Items.Add(value);
            }

            this.miniDumpTypeComboBox.Items.Clear();
            foreach (MiniDumpType value in Enum.GetValues(typeof(MiniDumpType)))
            {
                this.miniDumpTypeComboBox.Items.Add(value);
            }

            this.storagePathComboBox.Items.Clear();
            foreach (StoragePath value in Enum.GetValues(typeof(StoragePath)))
            {
                this.storagePathComboBox.Items.Add(value);
            }

            foreach (var loader in this.panelLoaders)
            {
                loader.UnloadPanel();
            }

            this.sleepBeforeSendNumericUpDown.Maximum = decimal.MaxValue;
            this.maxQueuedReportsNumericUpDown.Maximum = decimal.MaxValue;
            this.stopReportingAfterNumericUpDown.Maximum = decimal.MaxValue;

            this.mainTabs.Enabled = true;
            this.mainTabs.SelectedIndex = 0;
            this.saveButton.Enabled = true;
            this.addDestinationButton.Enabled = true;
        }
		
        public frmMain()
		{
			InitializeComponent();

            this.openFileDialog.InitialDirectory = Environment.CurrentDirectory;
			this.createFileDialog.InitialDirectory = Environment.CurrentDirectory;
		}

		private void LoadSettingsFile(bool createNew)
		{
			this.InitializeControls();

            fileName = createNew == false ? this.openFileDialog.FileName : this.createFileDialog.FileName;

            this.fileTextBox.Text = fileName;
            SettingsSection ss = new SettingsSection(fileName);
            Settings.SettingsSection = ss;

            Settings.CustomUIEvent += this.Settings_CustomUIEvent;

			// Read application settings
			this.uiProviderComboBox.SelectedItem = Settings.UIProvider;
			this.uiModeComboBox.SelectedItem = Settings.UIMode; // Should come after uiProviderComboBox = ...
			this.miniDumpTypeComboBox.SelectedItem = Settings.MiniDumpType;
			this.sleepBeforeSendNumericUpDown.Value = Settings.SleepBeforeSend;
			this.maxQueuedReportsNumericUpDown.Value = Settings.MaxQueuedReports;
			this.stopReportingAfterNumericUpDown.Value = Settings.StopReportingAfter;
			this.writeLogToDiskCheckBox.Checked = Settings.WriteLogToDisk;
			this.exitApplicationImmediatelyCheckBox.Checked = Settings.ExitApplicationImmediately;
			this.handleProcessCorruptedStateExceptionsCheckBox.Checked = Settings.HandleProcessCorruptedStateExceptions;
			this.releaseModeCheckBox.Checked = Settings.ReleaseMode;
            this.cbDisplayDeveloperUI.Checked = Settings.DisplayDeveloperUI;

			if (Settings.StoragePath == StoragePath.Custom)
			{
				this.storagePathComboBox.SelectedItem = StoragePath.Custom;
				this.customStoragePathTextBox.Text = Settings.StoragePath;
			}
			else
			{
				// Make sure that we're getting the enum value
				this.storagePathComboBox.SelectedItem = (StoragePath)Settings.StoragePath;
			}

			if (Settings.Cipher != null && Settings.Cipher.Length != 0)
			{
				this.cbEncryptConnectionStrings.Checked = true;
			}

			if (this.fileName.EndsWith("app.config"))
			{
				this.writeNetworkTraceToFileCheckBox.Enabled = true;
				this.networkTraceWarningLabel.Enabled = true;

				if (Settings.EnableNetworkTrace.HasValue)
				{
					this.writeNetworkTraceToFileCheckBox.Checked = Settings.EnableNetworkTrace.Value;
				}
			}

            while (this.mainTabs.TabPages.Count > 2)
            {
                this.mainTabs.TabPages.RemoveAt(2);
            }

            this.panelLoaders.Clear();

            // Read connection strings
            foreach (var destination in Settings.Destinations)
            {
                if (destination != null)
                {
                    var loader = new PanelLoader();
                    loader.RemoveDestination += this.loader_RemoveDestination;
                    loader.LoadPanel(destination.ConnectionString);
                    this.panelLoaders.Add(loader);
                    var tabPage = new TabPage(string.Format("Submit #{0}", this.panelLoaders.Count));
                    tabPage.Controls.Add(loader);
                    this.mainTabs.TabPages.Add(tabPage);
                }
            }
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
            // Validate user provide settings
			if (string.IsNullOrEmpty(this.uiProviderComboBox.Text))
			{
				MessageBox.Show(
					"The 'User Interface > UI Provider' selection should not be left blank. Please select a value for the provider or set the UI Mode to Auto.",
					"User Interface Provider is Left Blank",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				this.uiProviderComboBox.Focus();
				return;
			}

            Settings.EnableNetworkTrace = this.writeNetworkTraceToFileCheckBox.Checked; // This should come before settings app config
            
			// Save application settings
            Settings.UIMode = (UIMode)this.uiModeComboBox.SelectedItem;
            Settings.UIProvider = (UIProvider)this.uiProviderComboBox.SelectedItem;
            Settings.MiniDumpType = (MiniDumpType)this.miniDumpTypeComboBox.SelectedItem;
            Settings.SleepBeforeSend = Convert.ToInt32(this.sleepBeforeSendNumericUpDown.Value);
            Settings.MaxQueuedReports = Convert.ToInt32(this.maxQueuedReportsNumericUpDown.Value);
            Settings.StopReportingAfter = Convert.ToInt32(this.stopReportingAfterNumericUpDown.Value);
            Settings.WriteLogToDisk = this.writeLogToDiskCheckBox.Checked;
            Settings.HandleProcessCorruptedStateExceptions = this.handleProcessCorruptedStateExceptionsCheckBox.Checked;
            Settings.ReleaseMode = this.releaseModeCheckBox.Checked;
            Settings.DisplayDeveloperUI = this.cbDisplayDeveloperUI.Checked;

			if ((UIMode)this.uiModeComboBox.SelectedItem == UIMode.None)
			{
                Settings.ExitApplicationImmediately = this.exitApplicationImmediatelyCheckBox.Checked;
			}

            if (this.cbEncryptConnectionStrings.Checked)
            {
                //create if does not exist yet
                if (Settings.Cipher.Length == 0)
                    Settings.Cipher = Settings.GenerateKey();
            }
            else
                Settings.Cipher = null;

            Settings.StoragePath = this.storagePathComboBox.Text == "Custom" ? this.customStoragePathTextBox.Text : this.storagePathComboBox.Text;

            List<IProtocol> tempDestinations = new List<IProtocol>();
            foreach (var connectionString in this.panelLoaders.Where(p => p.Controls.Count == 2).Select(p => ((ISubmitPanel)p.Controls[0]).ConnectionString).Where(s => !string.IsNullOrEmpty(s)))
            {
                tempDestinations.Add(Settings.AddDestinationFromConnectionString(connectionString));
            }
            Settings.Destinations = tempDestinations;

            SettingsSection.Save(); //save configuration back
            this.status.Text = "Configuration file successfully saved. Please test your configuration.";
		}

        private void addDestinationButton_Click(object sender, EventArgs e)
        {
            var loader = new PanelLoader();
            loader.RemoveDestination += this.loader_RemoveDestination;
            this.panelLoaders.Add(loader);
            var tabPage = new TabPage(string.Format("Submit #{0}", this.panelLoaders.Count));
            tabPage.Controls.Add(loader);
            this.mainTabs.TabPages.Add(tabPage);
        }  

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void StoragePathComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			if ((StoragePath)this.storagePathComboBox.SelectedItem == StoragePath.Custom)
			{
				this.customStoragePathTextBox.Enabled = true;
			}
			else
			{
				this.customStoragePathTextBox.Enabled = false;
				this.customStoragePathTextBox.Text = string.Empty;
			}
		}

		private void CreateButton_Click(object sender, EventArgs e)
		{
			if (this.createFileDialog.ShowDialog() == DialogResult.OK)
			{
				this.fileName = this.createFileDialog.FileName;
				this.LoadSettingsFile(true);
			}
		}

		private void OpenButton_Click(object sender, EventArgs e)
		{
			if (this.openFileDialog.ShowDialog() == DialogResult.OK)
			{
				File.SetAttributes(this.openFileDialog.FileName, FileAttributes.Normal);
				this.fileName = this.openFileDialog.FileName;
				this.LoadSettingsFile(false);
			}
		}

		private void ExternalToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.createFileDialog.FileName = "Configurator.config";
			this.CreateButton_Click(this, null);
		}

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var loader = new PanelLoader();
            loader.RemoveDestination += this.loader_RemoveDestination;
            this.panelLoaders.Add(loader);
            var tabPage = new TabPage(string.Format("Submit #{0}", this.panelLoaders.Count));
            tabPage.Controls.Add(loader);
            this.mainTabs.TabPages.Add(tabPage);
        }

		private void EmbeddedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.createFileDialog.FileName = "app.config";
			this.CreateButton_Click(this, null);
		}

		private void UIModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if ((UIMode)this.uiModeComboBox.SelectedItem == UIMode.Auto)
			{
				this.uiProviderComboBox.Enabled = false;

				if (!this.uiProviderComboBox.Items.Contains(UIProvider.Auto))
				{
					this.uiProviderComboBox.Items.Add(UIProvider.Auto);
				}

				this.uiProviderComboBox.SelectedItem = UIProvider.Auto;

				// Revert back the settings for the "Handle Exceptions" check box
				this.exitApplicationImmediatelyCheckBox.Checked = Settings.ExitApplicationImmediately;
				this.exitApplicationImmediatelyCheckBox.Enabled = false;
				this.exitApplicationImmediatelyWarningLabel.Enabled = false;
			}
			else if ((UIMode)this.uiModeComboBox.SelectedItem == UIMode.None)
			{
				this.uiProviderComboBox.SelectedItem = null;
				this.previewButton.Enabled = false;

				// Enable the "Handle Exceptions" check box as it is valid for UIMode.None
				this.exitApplicationImmediatelyCheckBox.Enabled = true;
				this.exitApplicationImmediatelyWarningLabel.Enabled = true;
			}
			else
			{
				this.uiProviderComboBox.Enabled = true;
				this.uiProviderComboBox.Items.Remove(UIProvider.Auto);

				// Revert back the settings for the "Handle Exceptions" check box
				this.exitApplicationImmediatelyCheckBox.Checked = Settings.ExitApplicationImmediately;
				this.exitApplicationImmediatelyCheckBox.Enabled = false;
				this.exitApplicationImmediatelyWarningLabel.Enabled = false;
			}
		}

		private void PreviewButton_Click(object sender, EventArgs e)
		{
			using (var preview = new PreviewForm())
			{
				preview.ShowDialog((UIMode)this.uiModeComboBox.SelectedItem, (UIProvider)this.uiProviderComboBox.SelectedItem);
			}
		}

		private void UIProviderComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.uiProviderComboBox.SelectedItem == null)
			{
				this.previewButton.Enabled = false;
			}
			else if ((UIProvider)this.uiProviderComboBox.SelectedItem == UIProvider.Auto)
			{
				this.previewButton.Enabled = false;
			}
			else
			{
				this.previewButton.Enabled = true;
			}
		}

        private void loader_RemoveDestination(object sender, EventArgs e)
        {
            var loader = sender as PanelLoader;
            var idx = 2;
            idx += this.panelLoaders.ToList().IndexOf(loader);

            if (!string.IsNullOrEmpty(loader.connString))
            {
                var protocol = ConnectionStringParser.Parse(loader.connString)["Type"];
                IProtocol dest = null;

                if (protocol == typeof(Mail).Name || protocol.ToLower() == "email" || protocol.ToLower() == "e-mail")
                {
                    dest = new Mail(loader.connString);
                }
                else if (protocol == typeof(Ftp).Name)
                {
                    dest = new Ftp(loader.connString);
                }
                else if (protocol == typeof(Http).Name)
                {
                    dest = new Http(loader.connString);
                }

                if (dest != null)
                {
                    Settings.Destinations.Remove(dest);
                }
            }

            this.panelLoaders.Remove(loader);
            this.mainTabs.TabPages.RemoveAt(idx);
            return;
        }

        private void Settings_CustomUIEvent(object sender, CustomUIEventArgs e)
        {
            var Form = new CustomPreviewForm();
            e.Result = Form.ShowDialog(e.Report);
        }
	}
}
