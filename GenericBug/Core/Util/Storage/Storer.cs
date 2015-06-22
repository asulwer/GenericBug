using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;

using GenericBug.Core.Util.Exceptions;
using GenericBug.Core.Util.Logging;

namespace GenericBug.Core.Util.Storage
{
    /// <summary>
    /// Initializes a new instance of the Storage class. This class implements <see cref="IDisposable"/> interface
    /// so it is better used with a using {...} statement.
    /// </summary>
    /// <remarks>This class should be instantiated to work with a single file at once.</remarks>
    public class Storer : IDisposable
    {
        private bool disposed;

        private IsolatedStorageFile isoStore;

        private Stream stream;

        public string FileName { get; set; }

        public string FilePath { get; set; }

        ~Storer()
        {
            this.Dispose(false);
        }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    if (this.stream != null)
                    {
                        this.stream.Close();
                    }

                    if (this.isoStore != null)
                    {
                        this.isoStore.Close();
                    }
                }

                // Clean up unmanaged resources
            }

            this.disposed = true;
        }

        public static int GetReportCount()
        {
            if (Settings.StoragePath == Enums.StoragePath.WindowsTemp)
            {
                var path = Path.Combine(new[] { System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Settings.EntryAssembly.GetName().Name });
                return Directory.Exists(path) ? Directory.EnumerateFiles(path, "Exception_*.zip").Count() : 0;
            }
            else if (Settings.StoragePath == Enums.StoragePath.CurrentDirectory)
            {
                return Directory.Exists(Settings.GenericBugDirectory) ? Directory.EnumerateFiles(Settings.GenericBugDirectory, "Exception_*.zip").Count() : 0;
            }
            else if (Settings.StoragePath == Enums.StoragePath.IsolatedStorage)
            {
                using (var isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null))
                {
                    return isoFile.GetFileNames("Exception_*.zip").Count();
                }
            }
            else if (Settings.StoragePath == Enums.StoragePath.Custom)
            {
                var path = Path.GetFullPath(Settings.StoragePath);
                return Directory.Exists(path) ? Directory.EnumerateFiles(path, "Exception_*.zip").Count() : 0;
            }
            else
            {
                throw GenericBugConfigurationException.Create(() => Settings.StoragePath, "Parameter supplied for settings property is invalid.");
            }
        }

        /// <summary>
        /// This function will get rid of the oldest files first.
        /// </summary>
        public static void TruncateReportFiles()
        {
            TruncateReportFiles(Settings.MaxQueuedReports);
        }

        /// <summary>
        /// This function will get rid of the oldest files first.
        /// </summary>
        /// <param name="maxQueuedReports">Maximum number of queued files to be stored. Setting this to 0 deletes all files. Settings this
        /// to anything less than zero will store infinite number of files.</param>
        public static void TruncateReportFiles(int maxQueuedReports)
        {
            if (maxQueuedReports < 0)
            {
                return;
            }
            else if (Settings.StoragePath == Enums.StoragePath.WindowsTemp)
            {
                var path = Path.Combine(new[] { System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Settings.EntryAssembly.GetName().Name });
                TruncateFiles(path, maxQueuedReports);
            }
            else if (Settings.StoragePath == Enums.StoragePath.CurrentDirectory)
            {
                TruncateFiles(Settings.GenericBugDirectory, maxQueuedReports);
            }
            else if (Settings.StoragePath == Enums.StoragePath.IsolatedStorage)
            {
                using (IsolatedStorageFile isoFile = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null))
                {
                    int reportCount = isoFile.GetFileNames("Exception_*.zip").Count();

                    if (reportCount > maxQueuedReports)
                    {
                        int extraCount = reportCount - maxQueuedReports;

                        if (maxQueuedReports == 0)
                        {
                            Logger.Trace("Truncating all report files from the isolated storage.");
                        }
                        else
                        {
                            Logger.Trace("Truncating extra " + extraCount + " report files from the isolates storage.");
                        }

                        foreach (var file in isoFile.GetFileNames("Exception_*.zip"))
                        {
                            extraCount--;
                            File.Delete(file);

                            if (extraCount == 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            else if (Settings.StoragePath == Enums.StoragePath.Custom)
            {
                string path = Path.GetFullPath(Settings.StoragePath);
                TruncateFiles(path, maxQueuedReports);
            }
            else
            {
                throw GenericBugConfigurationException.Create(() => Settings.StoragePath, "Parameter supplied for settings property is invalid.");
            }
        }

        public Stream CreateReportFile(string reportFileName)
        {
            this.FileName = reportFileName;

            if (Settings.StoragePath == Enums.StoragePath.WindowsTemp)
            {
                string directoryPath = Path.Combine(new[] { System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Settings.EntryAssembly.GetName().Name });

                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                this.FilePath = Path.Combine(directoryPath, this.FileName);
                Logger.Trace("Creating report file to Windows temp path: " + this.FilePath);
                return this.stream = new FileStream(this.FilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            else if (Settings.StoragePath == Enums.StoragePath.CurrentDirectory)
            {
                this.FilePath = Path.Combine(Settings.GenericBugDirectory, this.FileName);
                Logger.Trace("Creating report file to entry assembly directory path: " + this.FilePath);
                return this.stream = new FileStream(this.FilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            else if (Settings.StoragePath == Enums.StoragePath.IsolatedStorage)
            {
                this.FilePath = null;
                Logger.Trace("Creating report file to isolated storage path: [Isolated Storage Directory]\\" + this.FileName);
                return this.stream = new IsolatedStorageFileStream(this.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            else if (Settings.StoragePath == Enums.StoragePath.Custom)
            {
                string directoryPath = Path.GetFullPath(Settings.StoragePath); // In case this is a relative path

                if (Directory.Exists(directoryPath) == false)
                {
                    Directory.CreateDirectory(directoryPath);
                }

                this.FilePath = Path.Combine(directoryPath, this.FileName);
                Logger.Trace("Creating report file to custom path: " + this.FilePath);
                return this.stream = new FileStream(this.FilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            }
            else
            {
                throw GenericBugConfigurationException.Create(() => Settings.StoragePath, "Parameter supplied for settings property is invalid.");
            }
        }

        public void DeleteCurrentReportFile()
        {
            this.stream.Close();

            if (this.stream is IsolatedStorageFileStream)
            {
                Logger.Trace("Deleting report file from isolated storage: " + this.FileName);
                this.isoStore.DeleteFile(this.FileName);
            }
            else
            {
                Logger.Trace("Deleting report file from path: " + this.FilePath);
                File.Delete(this.FilePath);
            }
        }

        public Storer OpenReportFile(Stream s)
        {
            if (Settings.StoragePath == Enums.StoragePath.WindowsTemp)
            {
                string path = Path.Combine(new[] { System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Settings.EntryAssembly.GetName().Name });

                if (Directory.Exists(path) && Directory.EnumerateFiles(path, "Exception_*.zip").Count() > 0)
                {
                    try
                    {
                        this.FilePath = Directory.EnumerateFiles(path, "Exception_*.zip").First();
                        this.FileName = Path.GetFileName(this.FilePath);
                        this.stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    catch (IOException exception)
                    {
                        Logger.Error("Cannot access the report file at Windows temp directory (it is probably locked, see the inner exception): " + this.FilePath, exception);
                    }
                }
            }
            else if (Settings.StoragePath == Enums.StoragePath.CurrentDirectory)
            {
                string path = Settings.GenericBugDirectory;

                if (path != null && Directory.Exists(path) && Directory.EnumerateFiles(path, "Exception_*.zip").Count() > 0)
                {
                    try
                    {
                        this.FilePath = Directory.EnumerateFiles(path, "Exception_*.zip").First();
                        this.FileName = Path.GetFileName(this.FilePath);
                        this.stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    catch (IOException exception)
                    {
                        Logger.Error("Cannot access the report file at entry assembly directory (it is probably locked, see the inner exception): " + this.FilePath, exception);
                    }
                }
            }
            else if (Settings.StoragePath == Enums.StoragePath.IsolatedStorage)
            {
                this.isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);

                if (this.isoStore.GetFileNames("Exception_*.zip").Count() > 0)
                {
                    try
                    {
                        this.FilePath = null;
                        this.FileName = this.isoStore.GetFileNames("Exception_*.zip").First();
                        this.stream = new IsolatedStorageFileStream(this.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    catch (IOException exception)
                    {
                        Logger.Error("Cannot access the report file at isolated storage (it is probably locked, see the inner exception): [Isolated Storage Directory]\\" + this.FileName, exception);
                    }
                }
            }
            else if (Settings.StoragePath == Enums.StoragePath.Custom)
            {
                string path = Path.GetFullPath(Settings.StoragePath);

                if (Directory.Exists(path) && Directory.EnumerateFiles(path, "Exception_*.zip").Count() > 0)
                {
                    try
                    {
                        this.FilePath = Directory.EnumerateFiles(path, "Exception_*.zip").First();
                        this.FileName = Path.GetFileName(this.FilePath);
                        this.stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    catch (IOException exception)
                    {
                        Logger.Error("Cannot access the report file from the given custom path (it is probably locked, see the inner exception): " + this.FilePath, exception);
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw GenericBugConfigurationException.Create(() => Settings.StoragePath, "Parameter supplied for settings property is invalid.");
            }            

            return new Storer() { FileName = this.FileName, FilePath=this.FilePath, isoStore=this.isoStore, stream=this.stream };
        }

        /// <summary>
        /// Returns the first-in-queue report file. If there are no files queued, returns <see langword="null"/>.
        /// </summary>
        /// <returns>Report file stream.</returns>
        public Stream GetFirstReportFile()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("GenericBug.Utils.Storage was already destroyed and out of scope when accessed.");
            }

            if (Settings.StoragePath == Enums.StoragePath.WindowsTemp)
            {
                string path = Path.Combine(new[] { System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Settings.EntryAssembly.GetName().Name });

                if (Directory.Exists(path) && Directory.EnumerateFiles(path, "Exception_*.zip").Count() > 0)
                {
                    try
                    {
                        this.FilePath = Directory.EnumerateFiles(path, "Exception_*.zip").First();
                        this.FileName = Path.GetFileName(this.FilePath);
                        return this.stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    catch (IOException exception)
                    {
                        // If the file is locked (as per IOException), then probably another instance of the library is already accessing
                        // the file so let the other instance handle the file
                        Logger.Error("Cannot access the report file at Windows temp directory (it is probably locked, see the inner exception): " + this.FilePath, exception);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else if (Settings.StoragePath == Enums.StoragePath.CurrentDirectory)
            {
                string path = Settings.GenericBugDirectory;

                if (path != null && Directory.Exists(path) && Directory.EnumerateFiles(path, "Exception_*.zip").Count() > 0)
                {
                    try
                    {
                        this.FilePath = Directory.EnumerateFiles(path, "Exception_*.zip").First();
                        this.FileName = Path.GetFileName(this.FilePath);
                        return this.stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    catch (IOException exception)
                    {
                        // If the file is locked (as per IOException), then probably another instance of the library is already accessing
                        // the file so let the other instance handle the file
                        Logger.Error("Cannot access the report file at entry assembly directory (it is probably locked, see the inner exception): " + this.FilePath, exception);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else if (Settings.StoragePath == Enums.StoragePath.IsolatedStorage)
            {
                this.isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain, null, null);

                if (this.isoStore.GetFileNames("Exception_*.zip").Count() > 0)
                {
                    try
                    {
                        this.FilePath = null;
                        this.FileName = this.isoStore.GetFileNames("Exception_*.zip").First();
                        this.stream = new IsolatedStorageFileStream(this.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                        return this.stream;
                    }
                    catch (IOException exception)
                    {
                        // If the file is locked (as per IOException), then probably another instance of the library is already accessing
                        // the file so let the other instance handle the file
                        Logger.Error("Cannot access the report file at isolated storage (it is probably locked, see the inner exception): [Isolated Storage Directory]\\" + this.FileName, exception);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else if (Settings.StoragePath == Enums.StoragePath.Custom)
            {
                string path = Path.GetFullPath(Settings.StoragePath);

                if (Directory.Exists(path) && Directory.EnumerateFiles(path, "Exception_*.zip").Count() > 0)
                {
                    try
                    {
                        this.FilePath = Directory.EnumerateFiles(path, "Exception_*.zip").First();
                        this.FileName = Path.GetFileName(this.FilePath);
                        return this.stream = new FileStream(this.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    }
                    catch (IOException exception)
                    {
                        // If the file is locked (as per IOException), then probably another instance of the library is already accessing
                        // the file so let the other instance handle the file
                        Logger.Error("Cannot access the report file from the given custom path (it is probably locked, see the inner exception): " + this.FilePath, exception);
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw GenericBugConfigurationException.Create(() => Settings.StoragePath, "Parameter supplied for settings property is invalid.");
            }
        }

        private static void TruncateFiles(string path, int maxQueuedReports)
        {
            if (Directory.Exists(path))
            {
                int reportCount = Directory.EnumerateFiles(path, "Exception_*.zip").Count();

                if (reportCount > maxQueuedReports)
                {
                    int extraCount = reportCount - maxQueuedReports;

                    if (maxQueuedReports == 0)
                    {
                        Logger.Trace("Truncating all report files from: " + path);
                    }
                    else
                    {
                        Logger.Trace("Truncating extra " + extraCount + " report files from: " + path);
                    }

                    foreach (var file in Directory.EnumerateFiles(path, "Exception_*.zip"))
                    {
                        extraCount--;
                        File.Delete(file);

                        if (extraCount == 0)
                        {
                            break;
                        }
                    }
                }
            }
        }        
    }
}
