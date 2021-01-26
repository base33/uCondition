using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Compilation;
using uCondition.Core.Data.Models;
using uCondition.Core.Interfaces;
using uCondition.Models;
using Umbraco.Core.Scoping;

namespace uCondition.Core
{
    public class PredicateManager : IPredicateManager
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IGlobalConditionsRepository _globalConditionsRepository;

        public PredicateManager(IScopeProvider scopeProvider, IGlobalConditionsRepository globalConditionsRepository)
        {
            _scopeProvider = scopeProvider;
            _globalConditionsRepository = globalConditionsRepository;
        }

        protected static List<Predicate> PredicateConfigs = new List<Predicate>();

        public List<Predicate> GetPredicates(bool withGlobalPredicates = true)
        {
            var predicates = new List<Predicate>();
            predicates.AddRange(PredicateConfigs);

            if (withGlobalPredicates)
            {
                using (var scope = _scopeProvider.CreateScope())
                {
                    predicates.AddRange(_globalConditionsRepository
                        .GetAll()
                        .Select(c => new Models.GlobalPredicate(Models.Mappers.DataToModel(c))));
                }
            }

            return predicates;
        }

        public Predicate GetPredicate(string alias)
        {
            var predicate = PredicateConfigs.FirstOrDefault(p => p.Alias == alias);

            if (predicate == null)
            {
                using (var scope = _scopeProvider.CreateScope())
                {
                    var globalCondition = _globalConditionsRepository.GetSingle(alias);
                    if (globalCondition != null)
                    {
                        return new Models.GlobalPredicate(Models.Mappers.DataToModel(globalCondition));
                    }
                }
            }

            return predicate != null ? (Predicate)Activator.CreateInstance(predicate.GetType()) : null;
        }

        //public List<IAction> GetActions()
        //{
        //    return ActionConfigs;
        //}

        //public IAction GetAction(string alias)
        //{
        //    var action = ActionConfigs.FirstOrDefault(a => a.Alias == alias);

        //    return action != null ? (uCondition.Models.Action)Activator.CreateInstance(action.GetType()) : null;
        //}

        internal static void StartUp()
        {
            var basePredicateType = typeof(Predicate);
            var globalPredicateType = typeof(Models.GlobalPredicate);
            //var baseActionType = typeof(uCondition.Models.Action);
            var dud = BuildManager.GetReferencedAssemblies();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = assemblies.SelectMany(s => s.GetTypes());

            var ourTypes = allTypes.Where(p => basePredicateType.IsAssignableFrom(p) && p != basePredicateType && p != globalPredicateType);
            //|| (baseActionType.IsAssignableFrom(p) && p != baseActionType))
            var typeGroups = ourTypes.GroupBy(p => basePredicateType.IsAssignableFrom(p)); //separate into actions and predicates

            foreach (var typeGroup in typeGroups)
            {
                if (typeGroup.Key == true)
                    PopulatePredicateConfigs(typeGroup);
                //else
                //    PopulateActionConfigs(typeGroup);
            }

        }

        protected static void PopulatePredicateConfigs(IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                PredicateConfigs.Add((Predicate)Activator.CreateInstance(type));
            }
        }
    }
}
