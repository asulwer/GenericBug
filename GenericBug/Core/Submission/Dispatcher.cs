using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using GenericBug.Core.Reporting.Info;
using GenericBug.Core.Util.Logging;
using GenericBug.Core.Util.Serialization;
using GenericBug.Core.Util.Storage;
using GenericBug.Core.Util.Storage.ZipStorage.Zip;

namespace GenericBug.Core.Submission
{
	internal class Dispatcher
	{
		/// <summary>
		/// Initializes a new instance of the Dispatcher class to send queued reports.
		/// </summary>
		/// <param name="isAsynchronous">
		/// Decides whether to start the dispatching process asynchronously on a background thread.
		/// </param>
		internal Dispatcher(bool isAsynchronous)
		{
			// Test if it has NOT been more than x many days since entry assembly was last modified)
			// This is the exact verifier code in the BugReport.cs of CreateReportZip() function
			if (Settings.StopReportingAfter < 0 || File.GetLastWriteTime(Settings.EntryAssembly.Location).AddDays(Settings.StopReportingAfter).CompareTo(DateTime.Now) > 0)
			{
				if (isAsynchronous)
				{
					// Log and swallow GenericBug's internal exceptions by default
					Task.Factory.StartNew(this.Dispatch).ContinueWith(
						t => Logger.Error("An exception occurred while dispatching bug report. Check the inner exception for details", t.Exception),
						TaskContinuationOptions.OnlyOnFaulted);
				}
				else
				{
					try
					{
						this.Dispatch();
					}
					catch (Exception exception)
					{
						Logger.Error("An exception occurred while dispatching bug report. Check the inner exception for details.", exception);
					}
				}
			}
			else
			{
				// ToDo: Cleanout all the remaining report files and disable GenericBug completely
			}
		}

		private void Dispatch()
		{
			// Make sure that we are not interfering with the crucial startup work);
			if (!Settings.RemoveThreadSleep)
				System.Threading.Thread.Sleep(Settings.SleepBeforeSend * 1000);
			
			// Truncate extra report files and try to send the first one in the queue
			Storer.TruncateReportFiles();

			// Now go through configured destinations and submit to all automatically
			for (bool hasReport = true; hasReport;)
			{
                using (Storer storer = new Storer())
                using (Stream s = storer.GetFirstReportFile())
                {
                    if (s != null)
                    {
                        // Extract crash/exception report data from the zip file. Delete the zip file if no data can be retrieved (i.e. corrupt file)
                        ExceptionData exceptionData;
                        try
                        {
                            exceptionData = this.GetDataFromZip(s);
                        }
                        catch (Exception ex)
                        {
                            storer.DeleteCurrentReportFile();
                            Logger.Error("An exception occurred while extraction report data from zip file. Check the inner exception for details.", ex);
                            return;
                        }

                        //submit the report file to all configured bug report submission targets
                        if (this.EnumerateDestinations(s, exceptionData) == false)
                            break;

                        // Delete the file after it was sent
                        storer.DeleteCurrentReportFile();
                    }
                    else
                    {
                        hasReport = false;
                    }
                }
			}
		}

		/// <summary>
		/// Enumerate all protocols to see if they are properly configured and send using the ones that are configured 
		/// as many times as necessary.
		/// </summary>
		/// <param name="reportFile">The file to read the report from.</param>
		/// <returns>Returns <see langword="true"/> if the sending was successful. 
		/// Returns <see langword="true"/> if the report was submitted to at least one destination.</returns>
        private bool EnumerateDestinations(Stream reportFile, ExceptionData exceptionData)
		{
			bool sentSuccessfullyAtLeastOnce = false;

            foreach (var destination in Settings.Destinations)
            {
                try
                {
                    Logger.Trace(string.Format("Submitting bug report via {0}.", destination.GetType().Name));
                    if (destination.Send(((FileStream)reportFile).Name, reportFile, exceptionData.Report, exceptionData.Exception))
                    {
                        sentSuccessfullyAtLeastOnce = true;
                    }
                }
                catch (Exception exception)
                {
                    Logger.Error(string.Format("An exception occurred while submitting bug report with {0}. Check the inner exception for details.", destination.GetType().Name), exception);
                }
            }

			return sentSuccessfullyAtLeastOnce;
		}

        private ExceptionData GetDataFromZip(Stream s)
        {
            var results = new ExceptionData();
            using (ZipFile zp = ZipFile.Read(s))
            {
                ZipEntry zeException = zp["Exception.xml"];
                using (MemoryStream ms = new MemoryStream())
                {
                    var dException = new XmlSerializer(typeof(SerializableException));
                    zeException.Extract(ms);
                    ms.Position = 0;
                    results.Exception = (SerializableException)dException.Deserialize(ms);                    
                    ms.Position = 0;
                }

                ZipEntry zeReport = zp["Report.xml"];
                using (MemoryStream ms = new MemoryStream())
                {
                    var dReport = new XmlSerializer(typeof(Report));
                    zeReport.Extract(ms);
                    ms.Position = 0;
                    results.Report = (Report)dReport.Deserialize(ms);
                    ms.Position = 0;
                }
            }

            return results;
        }

		private class ExceptionData
        {
            public SerializableException Exception { get; set; }

            public Report Report { get; set; }
        }
	}
}
