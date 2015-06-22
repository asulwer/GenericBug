using System;
using System.IO;
using System.Linq.Expressions;
using System.Diagnostics;

using GenericBug.Core.UI.Developer;
using GenericBug.Core.Util.Exceptions;
using GenericBug.Enums;

namespace GenericBug.Core.Util.Logging
{
    /// <summary>
    /// Uses <see cref="System.Diagnostics.Trace.Write(string, string)"/> method to log important messages. Also provides a <see cref="LogWritten"/>
    /// event. If <see cref="GenericBug.Settings.WriteLogToDisk"/> is set to true, a default "GenericBug.log" file is written to disk.
    /// </summary>
    /// <example>
    /// A sample trace listener can easily be added to the current application with an app.config file looking as below:
    /// <code>
    /// {?xml version="1.0"?}
    /// {configuration}
    ///  {configSections}
    ///  {/configSections}
    ///  {system.diagnostics}
    ///    {trace autoflush="true" indentsize="2"}
    ///      {listeners}
    ///        {add name="testAppListener" type="System.Diagnostics.TextWriterTraceListener" initializeData="MyApplication.log" /}
    ///      {/listeners}
    ///    {/trace}
    ///  {/system.diagnostics}
    /// {/configuration}
    /// </code>
    /// </example>
    public static class Logger
    {
        [DebuggerStepThrough()]
        static Logger()
        {
            if (Settings.WriteLogToDisk)
            {
                LogWritten += (message, category) => File.AppendAllText(Path.Combine(Settings.GenericBugDirectory, "GenericBug.log"), category + ": " + message + Environment.NewLine);
            }
        }

        /// <summary>
        /// First parameters is message string, second one is the category.
        /// </summary>
        public static event Action<string, LoggerCategory> LogWritten;

        [DebuggerStepThrough()]
        public static void Trace(string message)
        {
            Write(message, LoggerCategory.GenericBugTrace);
        }

        [DebuggerStepThrough()]
        public static void Info(string message)
        {
            Write(message, LoggerCategory.GenericBugInfo);
        }

        [DebuggerStepThrough()]
        public static void Warning(string message)
        {
            Write(message, LoggerCategory.GenericBugWarning);
        }

        [DebuggerStepThrough()]
        public static void Error(string message)
        {
            Write(message, LoggerCategory.GenericBugError);

            if (Settings.DisplayDeveloperUI)
            {
                using (var viewer = new InternalExceptionViewer())
                {
                    viewer.ShowDialog(new GenericBugRuntimeException(message));
                }
            }

            if (Settings.ThrowExceptions)
            {
                throw new GenericBugRuntimeException(message);
            }
        }

        [DebuggerStepThrough()]
        public static void Error(string message, Exception exception)
        {
            Write(message + Environment.NewLine + "Exception: " + exception, LoggerCategory.GenericBugError);

            if (Settings.DisplayDeveloperUI)
            {
                using (var viewer = new InternalExceptionViewer())
                {
                    viewer.ShowDialog(exception);
                }
            }

            if (Settings.ThrowExceptions)
            {
                throw new GenericBugRuntimeException(message, exception);
            }
        }

        [DebuggerStepThrough()]
        public static void Error<T>(Expression<Func<T>> propertyExpression, string message)
        {
            Write(message + " Misconfigured Property: " + ((MemberExpression)propertyExpression.Body).Member.Name, LoggerCategory.GenericBugError);

            if (Settings.DisplayDeveloperUI)
            {
                using (var viewer = new InternalExceptionViewer())
                {
                    viewer.ShowDialog(GenericBugConfigurationException.Create(propertyExpression, message));
                }
            }

            if (Settings.ThrowExceptions)
            {
                throw GenericBugConfigurationException.Create(propertyExpression, message);
            }
        }

        [DebuggerStepThrough()]
        private static void Write(string message, LoggerCategory category)
        {
            System.Diagnostics.Trace.Write(message + Environment.NewLine, category.ToString());

            if (Settings.DisplayDeveloperUI)
            {
                InternalLogViewer.LogEntry(message, category);
            }

            var handler = LogWritten;
            if (handler != null)
            {
                handler(message, category);
            }
        }
    }
}
