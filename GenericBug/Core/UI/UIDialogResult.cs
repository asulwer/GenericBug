namespace GenericBug.Core.UI
{
    public enum SendReport
	{
		Send, 
		DoNotSend
	}

    public enum ExecutionFlow
	{
		/// <summary>
		/// This will handle all unhandled exceptions to be able to continue execution.
		/// </summary>
		ContinueExecution, 

		/// <summary>
		/// This will handle all unhandled exceptions and exit the application.
		/// </summary>
		BreakExecution, 
	}

    public struct UIDialogResult
	{
        public ExecutionFlow Execution;
        public SendReport Report;

        public UIDialogResult(ExecutionFlow execution, SendReport report)
		{
			this.Execution = execution;
			this.Report = report;
		}
	}
}
