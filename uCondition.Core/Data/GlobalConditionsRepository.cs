using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Scoping;

namespace uCondition.Core.Data.Models
{
    public interface IGlobalConditionsRepository
    {
        void Delete(int id);
        IEnumerable<GlobalCondition> GetAll();
        GlobalCondition GetSingle(int id);
        GlobalCondition GetSingle(string guid);
        int Insert(GlobalCondition globalCondition);
        void Update(GlobalCondition globalCondition);
    }

    public class GlobalConditionsRepository : IGlobalConditionsRepository
    {
        protected readonly IScopeAccessor scopeAccessor;
        protected readonly IProfilingLogger logger;
        protected IScope AmbientScope
        {
            get
            {
                var scope = scopeAccessor.AmbientScope;
                if (scope == null)
                    throw new InvalidOperationException("Cannot run repository without an ambient scope");

                return scope;
            }
        }
        protected IUmbracoDatabase Database => AmbientScope.Database;
        protected ISqlContext SqlContext => AmbientScope.SqlContext;
        protected Sql<ISqlContext> Sql() => SqlContext.Sql();
        protected ISqlSyntaxProvider SqlSyntax => SqlContext.SqlSyntax;

        public GlobalConditionsRepository(IScopeAccessor scopeAccessor, IProfilingLogger logger)
        {
            this.scopeAccessor = scopeAccessor;
            this.logger = logger;
        }

        /// <summary>
        /// Get all global conditions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GlobalCondition> GetAll()
        {
            var sql = Sql().Select("*").From<GlobalCondition>();

            return Database.Fetch<GlobalCondition>(sql);
        }

        /// <summary>
        /// Get single global condition by guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public GlobalCondition GetSingle(string guid)
        {
            var sql = Sql().Select("*").From<GlobalCondition>().Where<GlobalCondition>(c => c.Guid == guid);

            return Database.FirstOrDefault<GlobalCondition>(sql);
        }

        /// <summary>
        /// Get single global condition by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public GlobalCondition GetSingle(int id)
        {
            var sql = Sql().Select("*").From<GlobalCondition>().Where<GlobalCondition>(c => c.Id == id);

            return Database.FirstOrDefault<GlobalCondition>(sql);
        }

        /// <summary>
        /// Insert new global condition
        /// </summary>
        /// <param name="globalCondition"></param>
        public int Insert(GlobalCondition globalCondition)
        {
            return Convert.ToInt32(Database.Insert(globalCondition));
        }

        /// <summary>
        /// Update global condition
        /// </summary>
        /// <param name="globalCondition"></param>
        public void Update(GlobalCondition globalCondition)
        {
            Database.Update(globalCondition);
        }

        /// <summary>
        /// Delete global condition by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            Database.Delete<GlobalCondition>(id);
        }
    }
}
