using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using uCondition.Core.Data.Models;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.SqlSyntax;
using Umbraco.Core.Scoping;

namespace uCondition.Core.Data
{
    public class RegisteredPredicateRepository
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

        public RegisteredPredicateRepository(IScopeAccessor scopeAccessor, IProfilingLogger logger)
        {
            this.scopeAccessor = scopeAccessor;
            this.logger = logger;
        }

        /// <summary>
        /// Get all global conditions
        /// </summary>
        /// <param name="withoutData"></param>
        /// <returns></returns>
        public IEnumerable<RegisteredPredicate> GetAll()
        {
            var sql = Sql().Select("*").From<RegisteredPredicate>();

            return Database.Fetch<RegisteredPredicate>(sql);
        }

        /// <summary>
        /// Get single global condition by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public RegisteredPredicate GetSingle(int id)
        {
            var sql = Sql().Select("*").From<RegisteredPredicate>().Where<RegisteredPredicate>(c => c.Id == id);

            return Database.FirstOrDefault<RegisteredPredicate>(sql);
        }

        /// <summary>
        /// Insert new global condition
        /// </summary>
        /// <param name="globalCondition"></param>
        public int Insert(RegisteredPredicate registeredCondition)
        {
            return Convert.ToInt32(Database.Insert(registeredCondition));
        }

        /// <summary>
        /// Update global condition
        /// </summary>
        /// <param name="globalCondition"></param>
        public void Update(RegisteredPredicate registeredCondition)
        {
            Database.Update(registeredCondition);
        }

        /// <summary>
        /// Delete global condition by Id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            Database.Delete<RegisteredPredicate>(id);
        }
    }
}
