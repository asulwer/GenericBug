using System;
using System.Diagnostics;
using System.Reflection;

using GenericBug.Core.Util.Serialization;

namespace GenericBug.Core.Reporting.Info
{
	[Serializable]
	public class GeneralInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GeneralInfo"/> class. This is the default constructor provided for XML
		/// serialization and de-serialization.
		/// </summary>
		public GeneralInfo()
		{
		}

		internal GeneralInfo(SerializableException serializableException)
		{
			this.HostApplication = Settings.EntryAssembly.GetLoadedModules()[0].Name;
            this.HostApplicationVersion = new ModuleVersion(FileVersionInfo.GetVersionInfo(Settings.EntryAssembly.Location).ProductVersion);
            this.GenericBugVersion = new ModuleVersion(FileVersionInfo.GetVersionInfo(Assembly.GetCallingAssembly().Location).ProductVersion);
            this.CLRVersion = new ModuleVersion(Environment.Version.ToString());
            this.DateTime = System.DateTime.UtcNow;

			if (serializableException != null)
			{
				this.ExceptionType = serializableException.Type;

				if (!string.IsNullOrEmpty(serializableException.TargetSite))
				{
					this.TargetSite = serializableException.TargetSite;
				}
				else if (serializableException.InnerException != null && !string.IsNullOrEmpty(serializableException.InnerException.TargetSite))
				{
					this.TargetSite = serializableException.InnerException.TargetSite;
				}

				this.ExceptionMessage = serializableException.Message;
			}
		}

		public ModuleVersion CLRVersion { get; set; }

		public DateTime DateTime { get; set; }

		public string ExceptionMessage { get; set; }

		public string ExceptionType { get; set; }

		public string HostApplication { get; set; }

		/// <summary>
		/// Gets or sets AssemblyFileVersion of host assembly.
		/// </summary>
		public ModuleVersion HostApplicationVersion { get; set; }

		/// <summary>
		/// Gets or sets AssemblyFileVersion of GenericBug.dll assembly.
		/// </summary>
		public ModuleVersion GenericBugVersion { get; set; }

		public string TargetSite { get; set; }

		public string UserDescription { get; set; }
	}
}
