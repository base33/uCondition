using System.Collections.Generic;
using uCondition.Interfaces;
using Umbraco.Core.Models.PublishedContent;

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
            return Umbraco.Web.PublishedPropertyExtension.Value<T>(
                new uConditionPublishedProperty(fieldName, Values[fieldName]));
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

        public object DataValue => value;

        /// <summary>
        /// Deprecated
        /// </summary>
        public string PropertyTypeAlias => name;

        public object Value => value;

        public object XPathValue => value;

        public IPublishedPropertyType PropertyType => throw new System.NotImplementedException();

        public string Alias => name;

        bool IPublishedProperty.HasValue(string culture, string segment) => value != null;

        public object GetSourceValue(string culture = null, string segment = null) => value;

        public object GetValue(string culture = null, string segment = null) => value;

        public object GetXPathValue(string culture = null, string segment = null) => value;
    }
}
