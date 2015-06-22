using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GenericBug.SettingsCollection
{
    //[ConfigurationCollection(typeof(NestedElement), AddItemName = "Destination,AdditionalReportFile", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    //public class NestedElementCollection : ConfigurationElementCollection, IEnumerable<NestedElement>
    //{
    //    public NestedElementCollection() { }

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
    //        return (elementName == "Destination" || elementName=="AdditionalReportFile");
    //    }

    //    public NestedElement this[int index]
    //    {
    //        get { return (NestedElement)base.BaseGet(index); }
    //        set
    //        {
    //            if (base.BaseGet(index) != null)
    //                base.BaseRemoveAt(index);

    //            base.BaseAdd(index, value);
    //        }
    //    }

    //    public NestedElement this[string name]
    //    {
    //        get
    //        {
    //            NestedElement ne = base.BaseGet(name) as NestedElement;
    //            return (NestedElement)base.BaseGet(name);
    //        }
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

    //    public void Add(NestedElement ne)
    //    {
    //        base.BaseAdd(ne);
    //    }

    //    public void Remove(string name)
    //    {
    //        base.BaseRemove(name);
    //    }

    //    public void Remove(NestedElement ne)
    //    {
    //        base.BaseRemove(ne);
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
    //        return new NestedElement();
    //    }

    //    protected override object GetElementKey(ConfigurationElement element)
    //    {
    //        return (element as NestedElement).Name;
    //    }

    //    public new IEnumerator<NestedElement> GetEnumerator()
    //    {
    //        int count = base.Count;
    //        for (int i = 0; i < count; i++)
    //            yield return base.BaseGet(i) as NestedElement;
    //    }
    //}

    [ConfigurationCollection(typeof(Element), AddItemName = CONST_ELEMENT_NAME, CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class NestedElementCollection : BaseConfigurationElementCollection<Element>
    {
        private const string CONST_ELEMENT_NAME = "nested";

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