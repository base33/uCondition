using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCondition.Core.Models
{
    public class Swimlane : PredicateGroup
    {
        public string Description { get; set; }
        public List<ExpressionAction> Actions { get; set; }
    }
}
