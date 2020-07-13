using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCondition.ExpressionEngine.Interfaces
{
    interface IPredicateExpression : IExpression
    {
        bool Test();
    }
}
