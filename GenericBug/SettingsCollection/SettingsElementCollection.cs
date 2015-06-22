using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GenericBug.SettingsCollection
{
    //[ConfigurationCollection(typeof(SettingsElement), AddItemName = "setting", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    //public class SettingsElementCollection : ConfigurationElementCollection, IEnumerable<SettingsElement>
    //{
    //    public SettingsElementCollection() { }

    //    public override ConfigurationElementCollectionType CollectionType
    //    {
    //        get
    //        {
    //            return ConfigurationElementCollectionType.BasicMap;
    //        }
    //    }

    //    protected override string ElementName
    //    {
    //        get
    //        {
    //            return string.Empty;
    //        }
    //    }

    //    protected override bool IsElementName(string elementName)
    //    {
    //        return (elementName == "setting");
    //    }

    //    public SettingsElement this[int index]
    //    {
    //        get { return (SettingsElement)base.BaseGet(index); }
    //        set
    //        {
    //            if (base.BaseGet(index) != null)
    //                base.BaseRemoveAt(index);

    //            base.BaseAdd(index, value);
    //        }
    //    }

    //    public SettingsElement this[string name]
    //    {
    //        get { return (SettingsElement)base.BaseGet(name); }
    //        set
    //        {
    //            int index = base.BaseIndexOf(base.BaseGet(name));

    //            if (index != -1)
    //            {
    //                base.BaseRemoveAt(index);
    //                base.BaseAdd(index, value);
    //            }
    //            else
    //                base.BaseAdd(value, false);
    //        }
    //    }

    //    public void Add(SettingsElement se)
    //    {
    //        base.BaseAdd(se);
    //    }

    //    public void Remove(SettingsElement se)
    //    {
    //        base.BaseRemove(se);
    //    }

    //    public void RemoveAt(int index)
    //    {
    //        base.BaseRemoveAt(index);
    //    }

    //    public void Clear()
    //    {
    //        base.BaseClear();
    //    }

    //    public string GetKey(int index)
    //    {
    //        return (string)base.BaseGetKey(index);
    //    }

    //    protected override ConfigurationElement CreateNewElement()
    //    {
    //        return new SettingsElement();
    //    }

    //    protected override object GetElementKey(ConfigurationElement element)
    //    {
    //        return (element as SettingsElement).Name;
    //    }

    //    public new IEnumerator<SettingsElement> GetEnumerator()
    //    {
    //        int count = base.Count;
    //        for (int i = 0; i < count; i++)
    //            yield return base.BaseGet(i) as SettingsElement;
    //    }
    //}    

    [ConfigurationCollection(typeof(Element), AddItemName = CONST_ELEMENT_NAME, CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SettingsElementCollection : BaseConfigurationElementCollection<Element>
    {
        private const string CONST_ELEMENT_NAME = "setting";
        
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get { return CONST_ELEMENT_NAME; }
        }
    }
}
