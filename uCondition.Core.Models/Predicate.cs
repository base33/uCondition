using System.Collections.Generic;

namespace uCondition.Core.Models
{
    public class Predicate : PredicateBase
    {
        public bool Invert { get; set; }
        public List<EditablePropertyValue> Values { get; set; }
        public bool NeedsConfiguring { get; set; }
        public PredicateConfig Config { get; set; }
    }
}