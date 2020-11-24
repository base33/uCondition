using Umbraco.Core;
using Umbraco.Core.Composing;

namespace uCondition.ConditionalPublicAccess.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    internal class MiddlewareComposer : ComponentComposer<MiddlewareComponent>, IUserComposer
    {
    }
}