using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;

namespace uCondition.Core.Models.Converter
{
    public class PredicateFieldConverter : Newtonsoft.Json.Converters.CustomCreationConverter<IEditableProperty>
    {
        public override IEditableProperty Create(Type objectType)
        {
            return new EditablePropertyField();
        }
    }
}
