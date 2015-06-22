using System;
using System.IO;

using GenericBug.Core.Reporting.Info;
using GenericBug.Core.Util.Serialization;

namespace GenericBug.Events
{    
    public class CustomSubmissionEventArgs : EventArgs
    {
        internal CustomSubmissionEventArgs(string fileName, Stream file, Report report, SerializableException exception)
        {
            this.FileName = fileName;
            this.File = file;
            this.Report = report;
            this.Exception = exception;
            this.Result = false;
        }

        public SerializableException Exception { get; private set; }

        public Stream File { get; private set; }

        public string FileName { get; private set; }

        public Report Report { get; private set; }

        public bool Result { get; set; }
    }
}