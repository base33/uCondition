using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCondition.Core.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EditablePropertyType
    {
        Standard,
        IsBoolean,
        IsPreValue,
        Hidden
    }
}
