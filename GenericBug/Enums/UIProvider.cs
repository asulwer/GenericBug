namespace GenericBug.Enums
{
    public enum UIProvider
    {
        /// <summary>
        /// Automatic provider selection is the default setting.
        /// </summary>
        Auto,

        /// <summary>
        /// Only the console is used to display the interface and interact with the user if necessary.
        /// </summary>
        Console,

        /// <summary>
        /// Windows Forms interface is used for all UI displayed to the user.
        /// </summary>
        WinForms,

        /// <summary>
        /// Windows Presentation Foundation interface is used for all UI displayed to the user.
        /// </summary>
        WPF,

        /// <summary>
		/// Custom UIProvider.
		/// </summary>
		Custom
    }
}
