using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace uCondition.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EditablePropertyDisplayMode
    {
        /// <summary>
        /// Render the raw value
        /// </summary>
        Standard,
        /// <summary>
        /// Value is a boolean.  Render a '1' as true, or '0' as false
        /// </summary>
        IsBoolean,
        /// <summary>
        /// The value may be an ID for a prevalue or list of prevalues
        /// </summary>
        IsPreValue,
        /// <summary>
        /// Do not show the value.  Use this for complex types that cannot be rendered. E.g. JSON etc.
        /// </summary>
        Hidden
    }
}
