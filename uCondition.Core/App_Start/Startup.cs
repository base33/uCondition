using Semver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Logging;
//using Umbraco.Core.Persistence.Migrations;
using Umbraco.Web;

namespace uCondition.Core.App_Start
{
    //public class Startup : ApplicationEventHandler
    //{
    //    protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    //    {
    //        base.ApplicationStarting(umbracoApplication, applicationContext);

    //        PredicateManager.StartUp();

    //        HandleMigrations();
    //    }

    //    /// <summary>
    //    /// Triggers migrations for the Global Conditions
    //    /// </summary>
    //    protected void HandleMigrations()
    //    {
    //        const string productName = "uCondition";
    //        var currentVersion = new SemVersion(0, 0, 0);

    //        var migrations = ApplicationContext.Current.Services.MigrationEntryService.GetAll(productName);
    //        var latestMigration = migrations.OrderByDescending(c => c.Version).FirstOrDefault();

    //        if (latestMigration != null)
    //            currentVersion = latestMigration.Version;

    //        //the current version for this release
    //        var targetVersion = new SemVersion(1, 0, 4);
    //        if (targetVersion == currentVersion)
    //            return;

    //        var migrationRunner = new MigrationRunner(
    //            ApplicationContext.Current.Services.MigrationEntryService,
    //            ApplicationContext.Current.ProfilingLogger.Logger,
    //            currentVersion,
    //            targetVersion,
    //            productName);

    //        try
    //        {
    //            migrationRunner.Execute(UmbracoContext.Current.Application.DatabaseContext.Database);
    //        }
    //        catch (System.Web.HttpException)
    //        {
    //            // because umbraco runs some other migrations after the migration runner 
    //            // is executed we get httpexception
    //            // catch this error, but don't do anything
    //            // fixed in 7.4.2+ see : http://issues.umbraco.org/issue/U4-8077
    //        }
    //        catch (Exception e)
    //        {
    //            LogHelper.Error<Startup>("Error running uCondition GlobalConditions migrations", e);
    //        }
    //    }
    //}
}
