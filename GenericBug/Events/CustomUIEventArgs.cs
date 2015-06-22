using System;

using GenericBug.Core.Reporting.Info;
using GenericBug.Core.UI;
using GenericBug.Core.Util.Serialization;
using GenericBug.Enums;

namespace GenericBug.Events
{
    public class CustomUIEventArgs : EventArgs
    {
        internal CustomUIEventArgs(UIMode uiMode, SerializableException exception, Report report)
        {
            this.UIMode = uiMode;
            this.Report = report;
            this.Exception = exception;
            this.Result = new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.DoNotSend);
        }

        public SerializableException Exception { get; private set; }

        public Report Report { get; private set; }

        public UIDialogResult Result { get; set; }

        public UIMode UIMode { get; private set; }
    }
}