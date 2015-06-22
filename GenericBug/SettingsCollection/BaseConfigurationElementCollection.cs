using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace GenericBug.SettingsCollection
{
    public interface IConfigurationElementCollectionElement
    {
        string ElementKey { get; }
    }

    public abstract class BaseConfigurationElementCollection<TConfigurationElementType> : ConfigurationElementCollection, IList<TConfigurationElementType> where TConfigurationElementType : ConfigurationElement, IConfigurationElementCollectionElement, new()
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new TConfigurationElementType();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TConfigurationElementType)element).ElementKey;
        }

        IEnumerator<TConfigurationElementType> IEnumerable<TConfigurationElementType>.GetEnumerator()
        {
            foreach (TConfigurationElementType type in this)
            {
                yield return type;
            }
        }

        public void Add(TConfigurationElementType configurationElement)
        {
            BaseAdd(configurationElement, true);
        }

        public void Clear()
        {
            BaseClear();
        }

        public bool Contains(TConfigurationElementType configurationElement)
        {
            return !(IndexOf(configurationElement) < 0);
        }

        public void CopyTo(TConfigurationElementType[] array, int arrayIndex)
        {
            base.CopyTo(array, arrayIndex);
        }

        bool ICollection<TConfigurationElementType>.IsReadOnly
        {
            get { return IsReadOnly(); }
        }

        public int IndexOf(TConfigurationElementType configurationElement)
        {
            return BaseIndexOf(configurationElement);
        }

        public void Insert(int index, TConfigurationElementType configurationElement)
        {
            BaseAdd(index, configurationElement);
        }

        public bool Remove(TConfigurationElementType configurationElement)
        {
            bool bFlag = true;

            try
            {
                BaseRemove(GetElementKey(configurationElement));
            }
            catch (Exception)
            {
                bFlag = false;
            }

            return bFlag;
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public TConfigurationElementType this[int index]
        {
            get
            {
                return (TConfigurationElementType)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public TConfigurationElementType this[string name]
        {
            get { return (TConfigurationElementType)BaseGet(name); }
            set
            {
                int index = BaseIndexOf(BaseGet(name));

                if (index != -1)
                {
                    BaseRemoveAt(index);
                    BaseAdd(index, value);
                }
                else
                    BaseAdd(value, false);
            }
        }
    }
}
