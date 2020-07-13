using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCondition.Core.Models.Converter
{
    public class PredicateConverter : Newtonsoft.Json.Converters.CustomCreationConverter<PredicateBase>
    {
        public override PredicateBase Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public PredicateBase Create(Type objectType, JObject jObject)
        {
            var type = (string)jObject.Property("Type");

            switch(type)
            {
                case "Predicate":
                    return new Predicate();
                case "PredicateGroup":
                    return new PredicateGroup();
                case "Swimlane":
                    return new Swimlane();
            }

            throw new Exception("Do not recognise predicate type: " + type);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
