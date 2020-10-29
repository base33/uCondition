using System.Collections.Generic;
using uCondition.Interfaces;

namespace uCondition.Models
{
    public abstract class Predicate : Base
    {
        public Predicate(string type = "Predicate") : base(type)
        {
            Name = "";
            Alias = "";
            Icon = "";
            Category = "";
            Fields = new List<EditableProperty>();
        }

        public abstract bool Validate(IFieldValues fieldValues);
    }
}
