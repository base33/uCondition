using uCondition.Core.Data.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;

namespace uCondition.Core.Data.Migrations
{
    public class CreateRegisteredConditionsTable : MigrationBase
    {
        public CreateRegisteredConditionsTable(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<CreateRegisteredConditionsTable>("Running migration: {MigrationStep}", "CreateRegisteredConditionsTable");

            if (TableExists("RegisteredPredicates") == false)
            {
                Create.Table<RegisteredPredicate>();
            }
        }
    }
}
