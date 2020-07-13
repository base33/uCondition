using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Models;

namespace uCondition.Core.Interfaces
{
    public interface IPredicateManager
    {
        List<Predicate> GetPredicates(bool withPredicates);
        Predicate GetPredicate(string alias);
    }
}
