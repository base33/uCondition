using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;

namespace uCondition.Core.Models
{
    public class EditablePropertyField
    {
        public string Alias
        {
            get; set;
        }

        public string Control
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public EditablePropertyType ValueType { get; set; }
    }
}
