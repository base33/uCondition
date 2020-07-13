using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Logging;

namespace uCondition.Core.Data.Models.Migrations
{
    [Migration("1.0.4", 1, "uCondition")]
    public class CreateGlobalConditionsTable : MigrationBase
    {
        private readonly UmbracoDatabase _database = ApplicationContext.Current.DatabaseContext.Database;
        private readonly DatabaseSchemaHelper _schemaHelper;

        public CreateGlobalConditionsTable(ISqlSyntaxProvider sqlSyntax, ILogger logger)
          : base(sqlSyntax, logger)
        {
            _schemaHelper = new DatabaseSchemaHelper(_database, logger, sqlSyntax);
        }

        public override void Up()
        {
            _schemaHelper.CreateTable<GlobalCondition>(true);
        }

        public override void Down()
        {
            _schemaHelper.DropTable<GlobalCondition>();
        }
    }
}
