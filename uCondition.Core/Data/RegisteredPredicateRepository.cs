using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Core.Data.Models;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace uCondition.Core.Data
{
    public class RegisteredPredicateRepository
    {
        protected UmbracoDatabase Database;

        public RegisteredPredicateRepository()
        {
            Database = ApplicationContext.Current.DatabaseContext.Database;
        }

        /// <summary>
        /// Get all global conditions
        /// </summary>
        /// <param name="withoutData"></param>
        /// <returns></returns>
        public IEnumerable<RegisteredPredicate> GetAll(bool withoutData = false)
        {
            var sql = new Sql()
                .Select("*")
                .From<RegisteredPredicate>(ApplicationContext.Current.DatabaseContext.SqlSyntax);

            return Database.Fetch<RegisteredPredicate>(sql);
        }

        /// <summary>
        /// Get single global condition by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public RegisteredPredicate GetSingle(int id)
        {
            var sql = new Sql()
                .Select("*")
                .From<RegisteredPredicate>(ApplicationContext.Current.DatabaseContext.SqlSyntax)
                .Where<RegisteredPredicate>(c => c.Id == id);

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
