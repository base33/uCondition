using Umbraco.Core;
using Umbraco.Core.Composing;

namespace uCondition.ConditionalPublicAccess.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class UConditionPublicAccessComposer : ComponentComposer<UConditionPublicAccessComponent>, IUserComposer
    {
    }
}