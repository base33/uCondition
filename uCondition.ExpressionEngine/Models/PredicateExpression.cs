using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Core.Interfaces;
using uCondition.Core.Models;
using uCondition.ExpressionEngine.Interfaces;
using uCondition.Interfaces;
using API = uCondition.Models;

namespace uCondition.ExpressionEngine.Models
{
    public class PredicateExpression : IPredicateExpression
    {
        protected Predicate Predicate { get; set; }
        protected IPredicateManager PredicateManager { get; set; }

        public PredicateExpression(Predicate predicate, IPredicateManager predicateManager)
        {
            Predicate = predicate;
            PredicateManager = predicateManager;
        }

        public bool Test()
        {
            var instance = PredicateManager.GetPredicate(Predicate.Config.Alias);
            var values = new API.FieldValues(Predicate.Values.ToDictionary(k => k.Alias, v => (object)v.Value));
            if (instance != null)
                return Predicate.Invert ? !instance.Validate(values) : instance.Validate(values);
            else
                return false;
        }
    }
}
