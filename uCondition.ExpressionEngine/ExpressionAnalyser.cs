using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.ExpressionEngine.Interfaces;
using uCondition.ExpressionEngine.Models;

namespace uCondition.ExpressionEngine
{
    public class ExpressionAnalyser
    {
        public bool Analyse(IExpression expression)
        {
            if(expression is PredicateExpression)
            {
                var predicateExpression = (PredicateExpression)expression;
                return predicateExpression.Test();
            }
            else if(expression is BinaryExpression)
            {
                var binaryExpression = (BinaryExpression)expression;

                if (binaryExpression.Operator == BinaryExpressionOperator.And)
                    return Analyse(binaryExpression.Left) && (binaryExpression.Right != null ? Analyse(binaryExpression.Right) : true);
                else
                    return Analyse(binaryExpression.Left) || (binaryExpression.Right != null ? Analyse(binaryExpression.Right) : false);
            }

            return false;
        }
    }
}
