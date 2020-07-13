using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;
using uCondition.Models;

namespace uCondition.Predicates.Members
{
    public class TruePredicate : Predicate
    {
        public TruePredicate() : base()
        {
            Name = "True Predicate";
            Alias = "TruePredicate";
            Icon = "icon-question";
            Category = "Logic";
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            return true;
        }
    }
}
