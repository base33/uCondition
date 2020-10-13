using System;
using System.Collections.Generic;
using System.Linq;
using uCondition.ConditionalPublicAccess.Data;
using uCondition.Core;
using uCondition.Core.Interfaces;
using uCondition.Core.Models;
using uCondition.Core.Models.Converter;
using uCondition.ExpressionEngine;
using Umbraco.Core.Models;

namespace uCondition.ConditionalPublicAccess.Helpers
{
    public class ConditionalAccess
    {
        public static bool HasAccess(string path)
        {
            var protectedPage = new ProtectedPageProvider().LoadForPath(path.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)));
            return protectedPage == null || HasAccess(protectedPage);
        }

        public static bool HasAccess(IPublishedContent content)
        {
            var protectedPage = new ProtectedPageProvider().LoadForPath(content.Path.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(c => int.Parse(c)));
            return protectedPage == null || HasAccess(protectedPage);
        }

        public static bool HasAccess(ProtectedPage protectedPage)
        {
            return protectedPage == null || ConditionalAccess.TestCondition(protectedPage.Condition);
        }

        public static bool HasAccess(ProtectedPageCondition protectedPage)
        {
            return protectedPage == null || ConditionalAccess.TestCondition(protectedPage.Condition);
        }

        public static bool TestCondition(string json)
        {
            uConditionModel uConditionModel = new uConditionModelConverter().Convert(json);
            bool flag = false;
            if (uConditionModel.PredicateGroups.Count >= 1 && uConditionModel.PredicateGroups.First().Conditions.Any())
            {
                var expressionAnalyser = new ExpressionAnalyser();
                var expressionCompiler = new ExpressionCompiler();
                var predicateManager = new PredicateManager();

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
