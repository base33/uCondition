using uCondition.Core.Components;
using uCondition.Core.Data;
using uCondition.Core.Data.Models;
using uCondition.Core.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace uCondition.Core.Composers
{
    public class UConditionComposer : ComponentComposer<UConditionComponent>
    {
        public override void Compose(Composition composition)
        {
            base.Compose(composition);

            composition.RegisterUnique<IGlobalConditionsRepository, GlobalConditionsRepository>();
            composition.RegisterUnique<IRegisteredPredicateRepository, RegisteredPredicateRepository>();
            composition.RegisterUnique<IPredicateManager, PredicateManager>();
        }
    }
}
