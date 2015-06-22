using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GenericBug.Core.Util.Serialization
{
    [Serializable]
    public class SerializableException
    {
        public SerializableException()
        {
        }

        public SerializableException(Exception exception) : this()
        {
            if (null != exception)
            {
                if (exception.TargetSite != null)
                    _TargetSite = exception.TargetSite.ToString() + " @ " + exception.TargetSite.DeclaringType.ToString();
                
                _Type = exception.GetType().ToString() ?? string.Empty;
                _HelpLink = exception.HelpLink ?? string.Empty;
                _Message = exception.Message ?? string.Empty;
                _Source = exception.Source ?? string.Empty;
                _StackTrace = exception.StackTrace ?? string.Empty;
                _InnerException = new SerializableException(exception.InnerException);

                setInnerExceptions(exception);
                setData(exception.Data);
                setExtendedInformation(exception);
            }
        }

        private string _TargetSite = string.Empty;
        public string TargetSite { get { return _TargetSite; } set { _TargetSite = value; } }

        private string _Type = string.Empty;
        public string Type { get { return _Type; } set { _Type = value; } }

        private string _HelpLink = string.Empty;
        public string HelpLink { get { return _HelpLink; } set { _HelpLink = value; } }

        private string _Message = string.Empty;
        public string Message { get { return _Message; } set { _Message = value; } }

        private string _Source = string.Empty;
        public string Source { get { return _Source; } set { _Source = value; } }

        private string _StackTrace = string.Empty;
        public string StackTrace { get { return _StackTrace; } set { _StackTrace = value; } }

        private SerializableException _InnerException;
        public SerializableException InnerException { get { return _InnerException; } set { _InnerException = value; } }

        private List<SerializableException> _InnerExceptions;
        public List<SerializableException> InnerExceptions { get { return _InnerExceptions ?? new List<SerializableException>(0); } set { _InnerExceptions = value; } }

        private KeyValuePair<object, object>[] _Data;
        public KeyValuePair<object, object>[] Data { get { return _Data ?? new KeyValuePair<object, object>[0]; } set { _Data = value; } }

        private KeyValuePair<string, object>[] _ExtendedInformation;
        public KeyValuePair<string, object>[] ExtendedInformation { get { return _ExtendedInformation ?? new KeyValuePair<string, object>[0]; } set { _ExtendedInformation = value; } }
        
        private void setInnerExceptions(Exception e)
        {
            if (e is AggregateException)
            {
                this.InnerExceptions = new List<SerializableException>();

                foreach (var innerException in ((AggregateException)e).InnerExceptions)
                {
                    this.InnerExceptions.Add(new SerializableException(innerException));
                }

                this.InnerExceptions.RemoveAt(0);
            }
        }

        private void setData(ICollection collection)
        {
            _Data = new KeyValuePair<object, object>[0];

            if (null != collection)
                collection.CopyTo(_Data, 0);
        }

        private void setExtendedInformation(Exception exception)
        {
            PropertyInfo[] ep = (from PropertyInfo property in exception.GetType().GetProperties()
                                 where
                                 property.Name != "Data" && property.Name != "InnerExceptions" && property.Name != "InnerException" &&
                                 property.Name != "Message" && property.Name != "Source" && property.Name != "StackTrace" &&
                                 property.Name != "TargetSite" && property.Name != "HelpLink" && property.CanRead
                                 select property).ToArray();

            if (ep != null)
            {
                PropertyInfo[] prop = (from PropertyInfo p in ep where p.GetValue(exception, null) != null select p).ToArray();

                _ExtendedInformation = new KeyValuePair<string, object>[prop.Length];

                for (int i = 0; i < prop.Length; i++)
                {
                    _ExtendedInformation[i] = new KeyValuePair<string, object>(prop[i].Name, prop[i].GetValue(exception,null));
                }
            }
        }

        public override string ToString()
        {
            var serializer = new XmlSerializer(typeof(SerializableException));
            using (var stream = new MemoryStream())
            {
                stream.SetLength(0);
                serializer.Serialize(stream, this);
                stream.Position = 0;
                var doc = XDocument.Load(stream);
                return doc.Root.ToString();
            }
        }
    }
}
