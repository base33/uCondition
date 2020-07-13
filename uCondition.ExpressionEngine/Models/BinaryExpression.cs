using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.ExpressionEngine.Interfaces;

namespace uCondition.ExpressionEngine.Models
{
    class BinaryExpression : IExpression
    {
        public IExpression Right { get; set; }
        public IExpression Left { get; set; }
        public BinaryExpressionOperator Operator { get; set; }

        public BinaryExpression()
        {

        }

        public BinaryExpression(IExpression left, IExpression right)
        {
            Left = left;
            Right = right;
        }
    }
}
