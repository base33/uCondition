using System;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;

namespace uCondition.ConditionalPublicAccess.Data.Migrations
{
    public class CreateProtectedPageTable : MigrationBase
    {
        public CreateProtectedPageTable(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<CreateProtectedPageTable>("Running migration: {MigrationStep}", nameof(CreateProtectedPageTable));

            if (TableExists(nameof(ProtectedPage)) == false)
            {
                try
                {
                    Create.Table<ProtectedPage>().Do();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}