using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GenericBug.SettingsCollection
{
    public class Element : ConfigurationElement, IConfigurationElementCollectionElement
    {
        public Element() : this("", "") { }

        public Element(string k, string v)
        {
            this.ElementKey = k;
            this.Value = v;
        }

        [ConfigurationProperty("name", IsKey = true, DefaultValue = "null")]
        public string ElementKey
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value")]
		public string Value
		{
            get { return (string)base["value"]; }
            set { base["value"] = value; }
		}
    }
}
