using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Core.Models.Converter;
using uCondition.Interfaces;

namespace uCondition.Core.Models
{
    public class ConfigBase
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Icon { get; set; }
        public string Category { get; set; }
        
        public IEnumerable<EditablePropertyField> Fields { get; set; }
    }
}
