using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Core.Interfaces;
using uCondition.Core.Models;
using uCondition.ExpressionEngine.Interfaces;
using uCondition.ExpressionEngine.Models;
using uCondition.Interfaces;

namespace uCondition.ExpressionEngine
{
    public class ExpressionCompiler
    {
        public IExpression Compile(PredicateGroup predicateGroup, IPredicateManager predicateManager)
        {
            var root = new BinaryExpression();
            var node = root;

            for(var i = 0; i < predicateGroup.Conditions.Count(); i++)
            {
                PredicateBase current = predicateGroup.Conditions.ElementAt(i);

                
                if(node.Left == null)
                {
                    node.Left = current is Predicate ? new PredicateExpression(current as Predicate, predicateManager) : Compile(current as PredicateGroup, predicateManager);
                    node.Operator = current.RightOperationAnd ? BinaryExpressionOperator.And : BinaryExpressionOperator.Or;
                }
                else if(node.Right == null)
                {
                    if(current.RightOperationAnd)
                    {
                        node.Right = new BinaryExpression();
                        node = (BinaryExpression)node.Right;
                        node.Operator = BinaryExpressionOperator.And;
                        node.Left = current is Predicate ? new PredicateExpression(current as Predicate, predicateManager) : Compile(current as PredicateGroup, predicateManager);
                    }
                    else
                    {
                        node.Right = current is Predicate ? new PredicateExpression(current as Predicate, predicateManager) : Compile(current as PredicateGroup, predicateManager);
                    }
                }

                if(current.RightOperationAnd == false) //or
                {
                    //create a parent and assign new parent to node
                    var newParent = new BinaryExpression();
                    newParent.Operator = BinaryExpressionOperator.Or;
                    newParent.Left = root;
                    root = newParent;
                    node = newParent;
                }
            }

            return root;
        }
    }
}

//if(current is Predicate)
//                {
//                    var predicate = (Predicate)current;
//var predicateExpression = new PredicateExpression(predicate);

//                    if(predicate.RightOperationAnd && node.Left == null)
//                    {
//                        node.Left = predicateExpression;
//                        node.Operator = BinaryExpressionOperator.And;
//                    }
//                    else if(predicate.RightOperationAnd && node.Right == null)
//                    {
//                        if()
//                    }
//                }
//                else
//                {

//                }