using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;
using Umbraco.Core.Models;

namespace uCondition.Models
{
    public class FieldValues : IFieldValues
    {
        protected Dictionary<string, object> Values { get; set; }

        public FieldValues(Dictionary<string, object> fieldValues)
        {
            Values = fieldValues;
        }

        public string GetValue(string fieldName)
        {
            return Values[fieldName].ToString();
        }

        public T GetValue<T>(string fieldName)
        {
            return Umbraco.Web.PublishedPropertyExtension.GetValue<T>(new uConditionPublishedProperty(fieldName, Values[fieldName]), default(T));
            //(T)Convert.ChangeType(Values[fieldName], typeof(T));
        }

        public bool TryGetValue<T>(string fieldName, out T value)
        {
            if (Values.ContainsKey(fieldName))
            {
                value = GetValue<T>(fieldName);
                return true;
            }

            value = default(T);
            return false;
        }
    }

    public class uConditionPublishedProperty : IPublishedProperty
    {
        protected string name { get; }
        protected object value { get; set; }

        public uConditionPublishedProperty(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        public object DataValue
        {
            get
            {
                return value;
            }
        }

        public bool HasValue
        {
            get
            {
                return value != null;
            }
        }

        public string PropertyTypeAlias
        {
            get
            {
                return name;
            }
        }

        public object Value
        {
            get
            {
                return value;
            }
        }

        public object XPathValue
        {
            get
            {
                return value;
            }
        }
    }
}
