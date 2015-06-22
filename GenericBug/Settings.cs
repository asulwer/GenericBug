using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using GenericBug.Core.Reporting;
using GenericBug.Core.Reporting.Info;
using GenericBug.Core.Submission;
using GenericBug.Core.Util;
using GenericBug.Core.Util.Exceptions;
using GenericBug.Core.Util.Logging;
using GenericBug.Enums;
using GenericBug.Events;
using GenericBug.Properties;
using GenericBug.SettingsCollection;

using StoragePath = GenericBug.Core.Util.Storage.StoragePath;

namespace GenericBug
{
    public static class Settings
    {
        static Settings()
        {
            if (SettingsSection == null)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                SettingsSection = (SettingsSection)config.GetSection("SettingsSection");
            }

            _AdditionalReportFiles = new List<string>();
            _Destinations = new List<IProtocol>();

            // Crucial startup settings
            Resources = new PublicResources();
            EntryAssembly = (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()) ?? Assembly.GetCallingAssembly();
            GenericBugDirectory = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) ?? Environment.CurrentDirectory;
            
            // Default to developer mode settings. Setting this now so that any exception below will be handled with correct settings
            ReleaseMode = false; // ToDo: This results initial config loading always setup to ThrowExceptions = true;
        }
        
        #region Events & Delagates

        /// <summary>
        /// Gets or sets an event for a CustomSubmission.
        /// </summary>
        internal static Delegate CustomSubmissionHandle;

        /// <summary>
        /// Gets or sets an event for a CustomUI.
        /// </summary>
        internal static Delegate CustomUIHandle;

        public static event EventHandler<CustomSubmissionEventArgs> CustomSubmissionEvent
        {
            add
            {
                CustomSubmissionHandle = Delegate.Combine(CustomSubmissionHandle, value);
            }

            remove
            {
                CustomSubmissionHandle = Delegate.Remove(CustomSubmissionHandle, value);
            }
        }

        public static event EventHandler<CustomUIEventArgs> CustomUIEvent
        {
            add
            {
                CustomUIHandle = Delegate.Combine(CustomUIHandle, value);
            }

            remove
            {
                CustomUIHandle = Delegate.Remove(CustomUIHandle, value);
            }
        }

        /// <summary>
        /// The internal logger write event for getting notifications for all internal GenericBug loggers. Using this event, you can attach internal GenericBug
        /// logs to your applications own logging facility (i.e. log4net, NLog, etc.)
        /// </summary>
        /// <param name="string">message string</param>
        /// <param name="LoggerCategory">log category (info,warning,error,etc)</param>
        public static event Action<string, LoggerCategory> InternalLogWritten
        {
            add
            {
                Logger.LogWritten += value;
            }

            remove
            {
                Logger.LogWritten -= value;
            }
        }

        /// <summary>
        /// This event is fired just before any caught exception is processed, to make them into an orderly bug report. Parameters passed with
        /// this event can be inspected for some internal decision making or to add more information to the bug report. Supplied parameters are:
        /// </summary>
        /// <param name="System.Exception">This is the actual exception object that is caught to be report as a bug. This object
        /// is processed to extract standard information from it but it can still carry some custom data that you may want to use so it is supplied
        /// as a parameter of this event for your convenience.</param>
        /// <param name="System.Object">This is any XML serializable object which can carry any additional information to be
        /// embedded in the actual bug report. For instance you may capture  more information about the system than GenericBug does for you, so you can put
        /// all those new information in a user defined type and pass it here. You can also pass in any system type that is serializable. Make sure
        /// that passed objects are XML serializable or the information will not appear in the report. See the sample usage for proper usage if this
        /// event.</param>
        /// <example>A sample code demonstrating the proper use of this event:
        /// <code>
        /// GenericBug.Settings.ProcessingException += (exception, report) =>
        ///	{
        ///		report.CustomInfo = new MyCusomSystemInformation { UtcTime = DateTime.UtcNow, AdditionalData = RubyExceptionData.GetInstance(exception) };
        ///	};
        /// </code>
        /// </example>
        public static event Action<Exception, Report> ProcessingException
        {
            add
            {
                BugReport.ProcessingException += value;
            }

            remove
            {
                BugReport.ProcessingException -= value;
            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Lookup for quickly finding the type to instantiate for a given connection string type.
        ///
        /// By making this lazy we don't do the lookup until we know we have to, as
        /// reflection against all assemblies can be slow.
        /// </summary>
        private static readonly Lazy<IDictionary<string, IProtocolFactory>> _availableProtocols = new Lazy<IDictionary<string, IProtocolFactory>>(() =>
        {
            // find all concrete implementations of IProtocolFactory
            var type = typeof(IProtocolFactory);
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(type.IsAssignableFrom)
                .Where(t => t.IsClass)
                .Where(t => !t.IsAbstract)
                .Select(t => (IProtocolFactory)Activator.CreateInstance(t))
                .ToDictionary(f => f.SupportedType);
        });

        private static List<IProtocol> _Destinations;

        private static List<string> _AdditionalReportFiles;

        #endregion

        #region Public Members

        /// <summary>
        /// Gets or sets a list of IProtocols, what to do with the zipped exception
        /// </summary>
        public static List<IProtocol> Destinations
        {
            get
            {
                _Destinations = new List<IProtocol>();

                foreach (string cs in (from Element ne in SettingsSection.Nested where ne.ElementKey.Contains("destination") select Decrypt(ne.Value)).ToArray())
                    _Destinations.Add(AddDestinationFromConnectionString(cs));

                return _Destinations;
            }
            set
            {
                _Destinations = value;

                Element[] E = (from Element ne in SettingsSection.Nested where ne.ElementKey.Contains("destination") select ne).ToArray();
                foreach (Element e in E)
                    SettingsSection.Nested.Remove(e);

                int i = 0;
                foreach (IProtocol ip in _Destinations)
                {
                    SettingsSection.Nested.Add(new Element("destination_" + i, Encrypt(ip.ConnectionString)));
                    i++;
                }
            }
        }

        /// <summary>
        /// Gets or sets a list of additional files to be added to the report zip. The files can use * or ? in the same way as DOS modifiers.
        /// </summary>
        public static List<string> AdditionalReportFiles
        {
            get
            {
                _AdditionalReportFiles = new List<string>();

                foreach (string cs in (from Element ne in SettingsSection.Nested where ne.ElementKey.Contains("additional") select ne.Value).ToArray())
                    _AdditionalReportFiles.Add(cs);

                return _AdditionalReportFiles;
            }
            set
            {
                _AdditionalReportFiles = value;

                Element[] E = (from Element ne in SettingsSection.Nested where ne.ElementKey.Contains("additional") select ne).ToArray();
                foreach (Element e in E)
                    SettingsSection.Nested.Remove(e);

                int i = 0;
                foreach (IProtocol ip in _Destinations)
                {
                    SettingsSection.Nested.Add(new Element("additional", Encrypt(ip.ConnectionString)));
                    i++;
                }
            }
        }

        /// <summary>
        /// Gets or sets the UI mode.
        /// Default value is Auto.
        /// </summary> 
        public static UIMode UIMode
        {
            get { return (UIMode)Enum.Parse(typeof(UIMode),SettingsSection.Settings["UIMode"].Value); }
            set { SettingsSection.Settings["UIMode"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the UI provider.
        /// Default value is Auto.
        /// </summary>
        public static UIProvider UIProvider
        {
            get { return (UIProvider)Enum.Parse(typeof(UIProvider), SettingsSection.Settings["UIProvider"].Value); }
            set { SettingsSection.Settings["UIProvider"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the bug report items storage path. After and unhandled exception occurs, the bug reports are created and queued for submission
        /// on the next application startup. Until then, the reports will be stored in this location. Default value is the application executable directory.
        /// This setting can either be assigned a full path string or a value from <see cref="GenericBug.Enums.StoragePath"/> enumeration.
        /// Default value CurrentDirectory
        /// </summary>
        public static Core.Util.Storage.StoragePath StoragePath
        {
            get { return (Enums.StoragePath)Enum.Parse(typeof(Enums.StoragePath), SettingsSection.Settings["StoragePath"].Value); }
            set { SettingsSection.Settings["StoragePath"].Value = ((Enums.StoragePath)value).ToString(); }
        }

        /// <summary>
        /// Gets or sets the memory dump type. Memory dumps are quite useful for replicating the exact conditions that the application crashed (i.e.
        /// getting the stack trace, local variables, etc.) but they take up a great deal of space, so choose wisely. Options are:
        /// None: No memory dump is generated.
        /// Tiny: Dump size ~200KB compressed.
        /// Normal: Dump size ~20MB compressed.
        /// Full: Dump size ~100MB compressed.
        /// Default value is Tiny.
        /// </summary>
        public static MiniDumpType MiniDumpType
        {
            get { return (MiniDumpType)Enum.Parse(typeof(MiniDumpType),SettingsSection.Settings["MiniDumpType"].Value); }
            set { SettingsSection.Settings["MiniDumpType"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the time in seconds that report dispatcher waits before starting to submit queued bug reports. Dispatcher initializes as
        /// soon as the application is run but waits for a given number of seconds so that it won't slow down the application startup.
        /// Default value is 10 seconds.
        /// </summary>
        public static int SleepBeforeSend
        {
            get { return Convert.ToInt32(SettingsSection.Settings["SleepBeforeSend"].Value); }
            set { SettingsSection.Settings["SleepBeforeSend"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the number of bug reports that can be queued for submission. Each time an unhandled exception occurs, the bug report is prepared to
        /// be sent at the next application startup. If submission fails (i.e. there is no Internet connection), the queue grows with each additional
        /// unhandled exception and resulting bug reports. This limits the max number of queued reports to limit the disk space usage.
        /// Default value is 5.
        /// </summary>
        public static int MaxQueuedReports
        {
            get { return Convert.ToInt32(SettingsSection.Settings["MaxQueuedReports"].Value); }
            set { SettingsSection.Settings["MaxQueuedReports"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets the number of days that GenericBug will be collecting bug reports for the application. Most of the time, 30 to 60 days after the
        /// release, there will be a new release and the current one will be obsolete. Due to this, it is not logical to continue to create and submit
        /// bug reports after a given number of days. After the predefined no of days, the user will still get to see the bug report UI but the reports
        /// will not actually be submitted.
        /// Default value is 30 days.
        /// </summary>
        public static int StopReportingAfter
        {
            get { return Convert.ToInt32(SettingsSection.Settings["StopReportingAfter"].Value); }
            set { SettingsSection.Settings["StopReportingAfter"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to write "GenericBug.log" file to disk. Otherwise, you can subscribe to log events through the 
        /// <see cref="InternalLogWritten"/> event. All the logging is done through System.Diagnostics.Trace.Write() function so you can also get
        /// the log with any trace listener.
        /// Default value is true.
        /// </summary>
        public static bool WriteLogToDisk
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["WriteLogToDisk"].Value); }
            set { SettingsSection.Settings["WriteLogToDisk"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating if application exits after handling an unhandled exception or sending error report. This value
        /// is disregarded for anything but UIMode.None. For UIMode.None, you can choose not to exit application which will result in
        /// 'Windows Error Reporting' (aka Dr. Watson) window to kick in. One reason to do so would be to keep in line with Windows 7 Logo requirements,
        /// which is a corner case. This may also be helpful in using GenericBug library as a simple unhandled exception logger facility, just to log and submit
        /// exceptions but does not interfere with the application execution flow.
        /// Default value is true.
        /// </summary>
        public static bool ExitApplicationImmediately
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["ExitApplicationImmediately"].Value); }
            set { SettingsSection.Settings["ExitApplicationImmediately"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to handle exceptions even in a corrupted process through the 'HandleProcessCorruptedStateExceptions'
        /// flag. The default value for this is false since generating bug reports for a corrupted process may not be successful so use with caution.
        /// </summary>
        public static bool HandleProcessCorruptedStateExceptions 
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["HandleProcessCorruptedStateExceptions"].Value); }
            set { SettingsSection.Settings["HandleProcessCorruptedStateExceptions"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable release mode for GenericBug library. In release mode, internal developer UI is not displayed and
        /// unhandled exceptions are only handled if there is no debugger attached to the proce Once properly configured and verified to be working
        /// as intended, GenericBug release mode should be enabled to properly use Visual Studio debugger, without GenericBug trying to handle exceptions
        /// before Visual Studio does.
        /// Default value is false.
        /// </summary>
        public static bool ReleaseMode
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["ReleaseMode"].Value); }

            set
            {
                SettingsSection.Settings["ReleaseMode"].Value = value.ToString();

                if (Convert.ToBoolean(SettingsSection.Settings["ReleaseMode"].Value))
                {
                    ThrowExceptions = false;
                    HandleExceptions = !Debugger.IsAttached;
                    DispatcherIsAsynchronous = true;
                    SkipDispatching = false;
                    RemoveThreadSleep = false;
                }
                else
                {
                    // If developer mode is on (default)
                    ThrowExceptions = true;
                    HandleExceptions = true;
                    DispatcherIsAsynchronous = false;
                    SkipDispatching = false;
                    RemoveThreadSleep = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether internal <see cref="GenericBugException"/> derived types are thrown or swallowed.
        /// Exceptions are NOT thrown by default except for debug builds. Note that exceptions are caught and re-thrown by the
        /// Logger.Error() method with added information so stack trace is reset. The inner exceptions should be inspected to get
        /// the actual stack trace.
        /// </summary>
        public static bool ThrowExceptions
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["ThrowExceptions"].Value); }
            set { SettingsSection.Settings["ThrowExceptions"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to enable developer user interface facilities which enable easier diagnosis of
        /// configuration and other internal errors.
        /// Default value is false
        /// </summary>
        public static bool DisplayDeveloperUI
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["DisplayDeveloperUI"].Value); }
            set { SettingsSection.Settings["DisplayDeveloperUI"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the unhandled exception handlers in GenericBug.Handler class actually handle exceptions.
        /// Exceptions will not be handled if the application is in release mode via <see cref="Settings.ReleaseMode"/> and a debugger
        /// is attached to the proce This enables proper debugging of normal exceptions even in the presence of GenericBug.
        /// </summary>
        public static bool HandleExceptions 
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["HandleExceptions"].Value); }
            set { SettingsSection.Settings["HandleExceptions"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the dispatcher, the class dealing with sending of reports to their destinations (like mail
        /// address or an issue tracker) run asynchronously (in a background worker thread as a <see cref="System.Threading.Tasks.Task"/>).
        /// By default dispatcher runs on a background thread except for debug builds, where it blocks the UI and runs in a synchronous manner.
        /// This is made to prevent any exceptions thrown by the dispatcher from being swallowed by the CLR since background thread exceptions
        /// are ignored in most cases, which is not desirable during development (i.e. in a debug build).
        /// </summary>
        public static bool DispatcherIsAsynchronous
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["DispatcherIsAsynchronous"].Value); }
            set { SettingsSection.Settings["DispatcherIsAsynchronous"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to skip the report dispatching process altogether.
        /// </summary>
        public static bool SkipDispatching
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["SkipDispatching"].Value); }
            set { SettingsSection.Settings["SkipDispatching"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to remove all <see cref="System.Threading.Thread.Sleep(int)"/> statements from 
        /// the thread executions. Some thread sleep statements are placed around to increase the host application performance i.e. the 
        /// <see cref="Settings.SleepBeforeSend"/> halts the execution of <see cref="Dispatcher.Dispatch()"/> for a given number of
        /// seconds to let host application initialize properly.
        /// </summary>
        public static bool RemoveThreadSleep
        {
            get { return Convert.ToBoolean(SettingsSection.Settings["RemoveThreadSleep"].Value); }
            set { SettingsSection.Settings["RemoveThreadSleep"].Value = value.ToString(); }
        }

        /// <summary>
        /// Gets the Cipher text used for encrypting connection strings before saving to disk. This is automatically generated when the 
        /// method <see cref="SaveCustomSettings(Stream, bool)"/> method is called with encryption set to true.
        /// </summary>
        public static byte[] Cipher
        {
            get { return Convert.FromBase64String(SettingsSection.Settings["Cipher"].Value); }
            set { SettingsSection.Settings["Cipher"].Value = (value == null) ? Convert.ToBase64String(new byte[0]) : Convert.ToBase64String(value); }
        }

        /// <summary>
        /// Gets or sets the entry assembly which hosts the GenericBug assembly. It is used for retrieving the version and the full name
        /// of the host application. i.e. Settings.EntryAssembly.GetLoadedModules()[0].Name; @ Info\General.cs
        /// </summary>
        public static Assembly EntryAssembly { get; set; }

        /// <summary>
        /// Gets or sets the absolute path to the directory that GenericBug.dll assembly currently resides. This is used in place of CWD
        /// throughout this assembly to prevent the library from getting affected of CWD changes that happens with Directory.SetCurrentDirectory().
        /// </summary>
        public static string GenericBugDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable network tracing and write the network trace log to "GenericBug.Network.log" file.
        /// This should only be used for diagnostics, debugging purposes as it slows down network connections considerably.
        /// Network tracing is disabled by default.
        /// </summary>
        public static bool? EnableNetworkTrace { get; set; }

        /// <summary>
        /// Gets or sets a public resources object which provides programmatic access to all string resources used in GenericBug. You can
        /// programmatically appoint new values for all the UI texts using this property. You can also localize all the
        /// dialog text, if there is no default localization provided for your language.
        /// </summary>
        public static PublicResources Resources { get; set; }

        public static SettingsSection SettingsSection { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a destination based on a connection string.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns>The protocol that was created and added. Null if empty connection string.</returns>
        /// <exception cref="System.ArgumentException">The protocol corresponding to the Type parameter in the connection string was not found.</exception>
        public static IProtocol AddDestinationFromConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }

            var connectionStringParts = ConnectionStringParser.Parse(connectionString);
            var type = connectionStringParts[@"Type"];
            if (!_availableProtocols.Value.ContainsKey(type))
            {
                throw new ArgumentException(string.Format("No protocol factory found for type '{0}'.", type), "connectionString");
            }

            var factory = _availableProtocols.Value[type];
            var protocol = factory.FromConnectionString(connectionString);
            return protocol;
        }

        public static byte[] GenerateKey()
        {
            using (var encryptor = new AesCryptoServiceProvider())
            {
                encryptor.GenerateKey();
                return encryptor.Key;
            }
        }

        #endregion

        #region Private Methods

        private static string Decrypt(string connectionString)
        {
            if (Cipher == null || Cipher.Length == 0)
            {
                return connectionString;
            }
            else
            {
                // Preserve FIPS compliance
                SHA512 hashProvider;
                if (Environment.OSVersion.Version.Major > 5 || (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 2))
                {
                    hashProvider = new SHA512CryptoServiceProvider();
                }
                else
                {
                    hashProvider = new SHA512Managed();
                }

                using (var hash = hashProvider)
                using (var cipher = new Rfc2898DeriveBytes(Cipher, hash.ComputeHash(Cipher), 3))
                using (var decryptor = new AesCryptoServiceProvider())
                {
                    var key = cipher.GetBytes(decryptor.KeySize / 8);
                    var iv = cipher.GetBytes(decryptor.BlockSize / 8);
                    var dec = decryptor.CreateDecryptor(key, iv);

                    var connectionStringBytes = Convert.FromBase64String(connectionString); // Reading from config file is always in Base64
                    // ToDo: check here for wrong password w/ System.Security.Cryptography.HMAC (catch the exception and display a meaningful message to the developer - or swallow the error)
                    var decryptedBytes = dec.TransformFinalBlock(connectionStringBytes, 0, connectionStringBytes.Length);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        private static string Encrypt(string connectionString)
        {
            if (Cipher == null || Cipher.Length == 0)
            {
                return connectionString;
            }
            else
            {
                // Preserve FIPS compliance via using xxxCryptoServiceProvider classes where possible
                SHA512 hashProvider;
                if (Environment.OSVersion.Version.Major > 5 || (Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 2))
                {
                    hashProvider = new SHA512CryptoServiceProvider();
                }
                else
                {
                    hashProvider = new SHA512Managed();
                }

                using (var hash = hashProvider)
                using (var cipher = new Rfc2898DeriveBytes(Cipher, hash.ComputeHash(Cipher), 3))
                using (var encryptor = new AesCryptoServiceProvider())
                {
                    var key = cipher.GetBytes(encryptor.KeySize / 8);
                    var iv = cipher.GetBytes(encryptor.BlockSize / 8);
                    var enc = encryptor.CreateEncryptor(key, iv);

                    var connectionStringBytes = Encoding.UTF8.GetBytes(connectionString);
                    var encryptedBytes = enc.TransformFinalBlock(connectionStringBytes, 0, connectionStringBytes.Length);

                    return Convert.ToBase64String(encryptedBytes); // Writing to config file is always in Base64
                }
            }
        }

        #endregion
    }
}