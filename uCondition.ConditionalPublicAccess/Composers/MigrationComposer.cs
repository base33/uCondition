using Umbraco.Core;
using Umbraco.Core.Composing;

namespace uCondition.ConditionalPublicAccess.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    internal class MigrationComposer : ComponentComposer<MigrationComponent>, IUserComposer
    {
    }
}