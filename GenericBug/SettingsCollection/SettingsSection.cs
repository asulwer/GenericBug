using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GenericBug.SettingsCollection
{
    public class SettingsSection : ConfigurationSection
    {
        private static Configuration _Config;

        public SettingsSection() { }

        public SettingsSection(string fn)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap() { ExeConfigFilename = fn };
            _Config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            SettingsSection ss = (SettingsSection)_Config.GetSection("SettingsSection");
            this.Settings = ss.Settings;
            this.Nested = ss.Nested;
        }
        
        [ConfigurationProperty("Settings")]
        public SettingsElementCollection Settings
        {
            get { return (SettingsElementCollection)base["Settings"]; }
            set { base["Settings"] = value; }
        }

        [ConfigurationProperty("Nested")]
        public NestedElementCollection Nested
        {
            get { return (NestedElementCollection)base["Nested"]; }
            set { base["Nested"] = value; }
        }

        public static void Save()
        {
            _Config.Save();
        }
    }
}
