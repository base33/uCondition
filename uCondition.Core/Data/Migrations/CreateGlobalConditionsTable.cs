using uCondition.Core.Data.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;

namespace uCondition.Core.Data.Migrations
{
    public class CreateGlobalConditionsTable : MigrationBase
    {
        public CreateGlobalConditionsTable(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<CreateGlobalConditionsTable>("Running migration: {MigrationStep}", "CreateGlobalConditionsTable");

            if (TableExists("GlobalConditions") == false)
            {
                Create.Table<GlobalCondition>();
            }
        }
    }
}
