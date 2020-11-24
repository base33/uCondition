using System.Linq;
using System.Web.Mvc;
using uCondition.ConditionalPublicAccess.Data;
using uCondition.Core.Interfaces;
using uCondition.Core.Models.Converter;
using uCondition.ExpressionEngine;

namespace uCondition.ConditionalPublicAccess.Helpers
{
    public class ConditionalAccess
    {
        //        public static bool HasAccess(string path)
        //        {
        //            var protectedPage = new ProtectedPageProvider()
        //                .LoadForPath(
        //                    path.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //                        .Select(c => int.Parse(c))
        //                        .ToList());
        //
        //            return protectedPage == null || HasAccess(protectedPage);
        //        }
        //
        //        public static bool HasAccess(IPublishedContent content)
        //        {
        //            var protectedPage = new ProtectedPageProvider()
        //                .LoadForPath(
        //                    content.Path
        //                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //                        .Select(c => int.Parse(c))
        //                        .ToList());
        //
        //            return protectedPage == null || HasAccess(protectedPage);
        //        }

        public static bool HasAccess(ProtectedPage protectedPage)
        {
            return protectedPage == null || TestCondition(protectedPage.Condition);
        }

        public static bool HasAccess(ProtectedPageCondition protectedPage)
        {
            return protectedPage == null || TestCondition(protectedPage.Condition);
        }

        public static bool TestCondition(string json)
        {
            var uConditionModel = new uConditionModelConverter().Convert(json);
            var flag = false;

            if (uConditionModel.PredicateGroups.Count >= 1 && uConditionModel.PredicateGroups.First().Conditions.Any())
            {
                var expressionAnalyser = new ExpressionAnalyser();
                var expressionCompiler = new ExpressionCompiler();

                var predicateManager = DependencyResolver.Current.GetService<IPredicateManager>();

                foreach (var swimlane in uConditionModel.PredicateGroups)
                {
                    var compiledExpression = expressionCompiler.Compile(swimlane, predicateManager);
                    flag = expressionAnalyser.Analyse(compiledExpression) || flag;
                }
            }

            return flag;
        }
    }
}