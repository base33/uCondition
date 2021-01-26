using uCondition.ConditionalPublicAccess.ProtectedPageProviders;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace uCondition.ConditionalPublicAccess.Composers
{
    internal class ProvidersComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register<IProtectedPageProvider, DatabaseProtectedPageProvider>();
        }
    }
}