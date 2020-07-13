using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;
using uCondition.Models;

namespace uCondition.Predicates.Members
{
    public class FalsePredicate : Predicate
    {
        public FalsePredicate()
        {
            Name = "False Predicate";
            Alias = "FalsePredicate";
            Icon = "icon-question";
            Category = "Logic";
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            return false;
        }
    }
}
