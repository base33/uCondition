using System.Collections.Generic;

namespace uCondition.Models
{
    public class Base
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Icon { get; set; }
        public string Category { get; set; }
        public IEnumerable<EditableProperty> Fields { get; set; }

        public Base(string type)
        {
            Type = type;
        }
    }
}
