using System;
using System.IO;
using System.Xml.Serialization;

using GenericBug.Core.Reporting.Info;
using GenericBug.Core.Reporting.MiniDump;
using GenericBug.Core.UI;
using GenericBug.Core.Util;
using GenericBug.Core.Util.Logging;
using GenericBug.Core.Util.Serialization;
using GenericBug.Core.Util.Storage;
using GenericBug.Core.Util.Storage.ZipStorage.Zip;

namespace GenericBug.Core.Reporting
{
	internal class BugReport
	{
		/// <summary>
		/// First parameters is the serializable exception object that is about to be processed, second parameter is any custom data
		/// object that the user wants to include in the report.
		/// </summary>
		internal static event Action<Exception, Report> ProcessingException;

		internal ExecutionFlow Report(Exception exception, ExceptionThread exceptionThread)
		{
			try
			{
				Logger.Trace("Starting to generate a bug report for the exception.");
				var serializableException = new SerializableException(exception);
				var report = new Report(serializableException);

				var handler = ProcessingException;
				if (handler != null)
				{
					Logger.Trace("Notifying the user before handling the exception.");

					// Allowing user to add any custom information to the report
					handler(exception, report);
				}
				
				var uiDialogResult = UISelector.DisplayBugReportUI(exceptionThread, serializableException, report);
				if (uiDialogResult.Report == SendReport.Send)
				{
					this.CreateReportZip(serializableException, report);
				}

				return uiDialogResult.Execution;
			}
			catch (Exception ex)
			{
				Logger.Error("An exception occurred during bug report generation process. See the inner exception for details.", ex);
				return ExecutionFlow.BreakExecution; // Since an internal exception occurred
			}
		}
		
        private void CreateReportZip(SerializableException serializableException, Report report)
        {
            // Test if it has NOT been more than x many days since entry assembly was last modified)
            if (Settings.StopReportingAfter < 0 || File.GetLastWriteTime(Settings.EntryAssembly.Location).AddDays(Settings.StopReportingAfter).CompareTo(DateTime.Now) > 0)
            {
                // Test if there is already more than enough queued report files
                if (Settings.MaxQueuedReports < 0 || Storer.GetReportCount() < Settings.MaxQueuedReports)
                {
                    var ReportName = "Exception_" + DateTime.UtcNow.Ticks + ".zip";
                    var minidumpFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DateTime.UtcNow.Ticks + ".mdmp");

                    using (var storer = new Storer())
                    using (Stream s = storer.CreateReportFile(ReportName))
                    using (ZipFile zp = new ZipFile())
                    {
                        MemoryStream ms1 = new MemoryStream();

                        // Store the exception
                        var sException = new XmlSerializer(typeof(SerializableException));
                        sException.Serialize(ms1, serializableException);
                        ms1.Position = 0;
                        zp.AddEntry(StoredItemFile.Exception, ms1);
                        
                        MemoryStream ms2 = new MemoryStream();

                        // Store the report
                        var sReport = new XmlSerializer(typeof(Report));
                        sReport.Serialize(ms2, report);
                        ms2.Position = 0;
                        zp.AddEntry(StoredItemFile.Report, ms2);
                        
                        // Add the memory mini dump to the report file (only if configured so)
                        if (DumpWriter.Write(minidumpFilePath))
                            zp.AddFile(minidumpFilePath, "Files");
                        
                        //add extra files
                        foreach (string sf in Settings.AdditionalReportFiles)
                            zp.AddFiles(Directory.GetFiles(Settings.GenericBugDirectory, sf), "Files");

                        zp.Save(s);
                        File.Delete(minidumpFilePath);
                        ms1.Close();
                        ms2.Close();
                    }                             

                    Logger.Trace("Created a new report file. Currently the number of report files queued to be send is: " + Storer.GetReportCount());
                }
                else
                {
                    Logger.Trace("Current report count is at its limit as per 'Settings.MaxQueuedReports (" + Settings.MaxQueuedReports + ")' setting: Skipping bug report generation.");
                }
            }
            else
            {
                Logger.Trace("As per setting 'Settings.StopReportingAfter(" + Settings.StopReportingAfter + ")', bug reporting feature was enabled for a certain amount of time which has now expired: Bug reporting is now disabled.");

                // ToDo: Completely eliminate this with SettingsOverride.DisableReporting = true; since enumerating file system adds overhead);
                if (Storer.GetReportCount() > 0)
                {
                    Logger.Trace("As per setting 'Settings.StopReportingAfter(" + Settings.StopReportingAfter + ")', bug reporting feature was enabled for a certain amount of time which has now expired: Truncating all expired bug reports.");
                    Storer.TruncateReportFiles(0);
                }
            }
        }
	}
}