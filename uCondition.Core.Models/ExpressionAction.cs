using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCondition.Core.Models
{
    public class ExpressionAction
    {
        public ActionConfig Config { get; set; }
        public List<EditablePropertyValue> Values { get; set; }
    }
}
