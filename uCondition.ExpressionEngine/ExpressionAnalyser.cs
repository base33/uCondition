using uCondition.ExpressionEngine.Interfaces;
using uCondition.ExpressionEngine.Models;

namespace uCondition.ExpressionEngine
{
    public class ExpressionAnalyser
    {
        public bool Analyse(IExpression expression)
        {
            if (expression is PredicateExpression predicateExpression)
            {
                return predicateExpression.Test();
            }
            else if (expression is BinaryExpression binaryExpression)
            {
                if (binaryExpression.Operator == BinaryExpressionOperator.And)
                    return Analyse(binaryExpression.Left) && (binaryExpression.Right == null || Analyse(binaryExpression.Right));
                else
                    return Analyse(binaryExpression.Left) || (binaryExpression.Right != null && Analyse(binaryExpression.Right));
            }

            return false;
        }
    }
}
