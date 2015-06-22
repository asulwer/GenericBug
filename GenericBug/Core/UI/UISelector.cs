using System;

using GenericBug.Core.Reporting.Info;
using GenericBug.Core.UI.Console;
using GenericBug.Core.UI.Custom;
using GenericBug.Core.UI.WinForms;
using GenericBug.Core.UI.WPF;
using GenericBug.Core.Util;
using GenericBug.Core.Util.Exceptions;
using GenericBug.Core.Util.Serialization;
using GenericBug.Enums;

namespace GenericBug.Core.UI
{
	/// <summary>
	/// Initializes a new instance of the UISelector class which displays the user an appropriate user interface in the event of unhandled exceptions.
	/// </summary>
    public static class UISelector
	{
        public static UIDialogResult DisplayBugReportUI(ExceptionThread exceptionThread, SerializableException serializableException, Report report)
		{
			if (exceptionThread == ExceptionThread.Task)
			{
				// Do not interfere with the default behavior for continuation on background thread exceptions. Just log and send them (no UI...)
				return new UIDialogResult(ExecutionFlow.ContinueExecution, SendReport.Send);
			}
			else if (Settings.UIMode == UIMode.Auto)
			{
				// test to see if the call is from an UI thread and if so, use the same UI type (WinForms, WPF, etc.)
				if (exceptionThread == ExceptionThread.UI_WinForms)
				{
					return WinFormsUI.ShowDialog(UIMode.Minimal, serializableException, report);
				}
				else if (exceptionThread == ExceptionThread.UI_WPF)
				{
					return WPFUI.ShowDialog(UIMode.Minimal, serializableException, report);
				}
				else if (exceptionThread == ExceptionThread.Main)
				{
					// If the call is not from a non-UI thread like the main app thread, it may be from the current appdomain but
					// the application may still be using an UI. Or it may be coming from an exception filter where UI type is undefined yet.
					switch (DiscoverUI())
					{
						case UIProvider.WinForms:
							return WinFormsUI.ShowDialog(UIMode.Minimal, serializableException, report);
						case UIProvider.WPF:
							return WPFUI.ShowDialog(UIMode.Minimal, serializableException, report);
						case UIProvider.Console:
							return ConsoleUI.ShowDialog(UIMode.Minimal, serializableException, report);
                        case UIProvider.Custom:
                            return CustomUI.ShowDialog(UIMode.Minimal, serializableException, report);
                        default:
							throw new GenericBugRuntimeException("UISelector.DiscoverUI() returned an invalid UI type.");
					}
				}
				else
				{
					throw new GenericBugRuntimeException("Parameter supplied for '" + typeof(ExceptionThread).Name + "' is not valid.");
				}
			}
			else if (Settings.UIMode == UIMode.None)
			{
				// Do not display an UI for UIMode.None
				if (Settings.ExitApplicationImmediately)
				{
					return new UIDialogResult(ExecutionFlow.BreakExecution, SendReport.Send);
				}
				else
				{
					return new UIDialogResult(ExecutionFlow.ContinueExecution, SendReport.Send);
				}
			}
			else if (Settings.UIProvider == UIProvider.Console)
			{
				return ConsoleUI.ShowDialog(Settings.UIMode, serializableException, report);
			}
			else if (Settings.UIProvider == UIProvider.WinForms)
			{
				return WinFormsUI.ShowDialog(Settings.UIMode, serializableException, report);
			}
			else if (Settings.UIProvider == UIProvider.WPF)
			{
				return WPFUI.ShowDialog(Settings.UIMode, serializableException, report);
			}
            else if (Settings.UIProvider == UIProvider.Custom)
            {
                return CustomUI.ShowDialog(UIMode.Minimal, serializableException, report);
            }
            else if (Settings.UIProvider == UIProvider.Auto)
            {
                // In this case, UIProvider = Auto & UIMode != Auto so just discover the UI provider and use the selected UI mode
                switch (DiscoverUI())
                {
                    case UIProvider.WinForms:
                        return WinFormsUI.ShowDialog(Settings.UIMode, serializableException, report);
                    case UIProvider.WPF:
                        return WPFUI.ShowDialog(Settings.UIMode, serializableException, report);
                    case UIProvider.Console:
                        return ConsoleUI.ShowDialog(Settings.UIMode, serializableException, report);
                    case UIProvider.Custom:
                        return CustomUI.ShowDialog(UIMode.Minimal, serializableException, report);
                    default:
                        throw new GenericBugRuntimeException("UISelector.DiscoverUI() returned an invalid UI type.");
                }
            }
            else
            {
                throw GenericBugConfigurationException.Create(() => Settings.UIProvider, "Parameter supplied for settings property is invalid.");
            }
		}

		private static UIProvider DiscoverUI()
		{
			// First of search for loaded assemblies in the current domain
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				switch (assembly.GetName().Name)
				{
					case "System.Windows.Forms":
						return UIProvider.WinForms;
					case "PresentationFramework":
						return UIProvider.WPF;
				}
			}

			// Even though UI assemblies may not be loaded, they may still be referenced. Search for them for a second time
			foreach (var assembly in Settings.EntryAssembly.GetReferencedAssemblies())
			{
				switch (assembly.Name)
				{
					case "System.Windows.Forms":
						return UIProvider.WinForms;
					case "PresentationFramework":
						return UIProvider.WPF;
				}
			}

			// If there is no known UI assembly loaded or referenced, the application is probably a console app
			return UIProvider.Console;
		}
	}
}
