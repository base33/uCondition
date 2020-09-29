using uCondition.Core.Data.Migrations;
using Umbraco.Core.Logging;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;
using Umbraco.Core.Migrations.Upgrade;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using System;

namespace uCondition.Core.Components
{
    public class UConditionComponent : IComponent
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IMigrationBuilder _migrationBuilder;
        private readonly IKeyValueService _keyValueService;
        private readonly ILogger _logger;

        public UConditionComponent(IScopeProvider scopeProvider, IMigrationBuilder migrationBuilder, IKeyValueService keyValueService, ILogger logger)
        {
            _scopeProvider = scopeProvider;
            _migrationBuilder = migrationBuilder;
            _keyValueService = keyValueService;
            _logger = logger;
        }

        public void Initialize()
        {
            PredicateManager.StartUp();

            HandleMigrations();
        }

        private void HandleMigrations()
        {
            var migrationPlan = new MigrationPlan("uCondition");

            migrationPlan.From(string.Empty)
                .To<CreateGlobalConditionsTable>("init-v2.0.0-step1")
                .To<CreateRegisteredConditionsTable>("init-v2.0.0-step2");

            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(_scopeProvider, _migrationBuilder, _keyValueService, _logger);
        }

        public void Terminate()
        {
        }
    }
}
