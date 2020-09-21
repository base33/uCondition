﻿using System.Linq;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Rdbms;
using Umbraco.Core.Persistence.SqlSyntax;

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
