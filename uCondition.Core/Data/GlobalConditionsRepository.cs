using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Core.Persistence;

namespace uCondition.Core.Data.Models
{
    public class GlobalConditionsRepository
    {
        protected UmbracoDatabase Database;

        public GlobalConditionsRepository()
        {
            Database = ApplicationContext.Current.DatabaseContext.Database;
        }

        /// <summary>
        /// Get all global conditions
        /// </summary>
        /// <param name="withoutData"></param>
        /// <returns></returns>
        public IEnumerable<GlobalCondition> GetAll(bool withoutData = false)
        {
            var sql = new Sql()
                .Select("*")
                .From<GlobalCondition>(ApplicationContext.Current.DatabaseContext.SqlSyntax);

            return Database.Fetch<GlobalCondition>(sql);
        }

        /// <summary>
        /// Get single global condition by guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public GlobalCondition GetSingle(string guid)
        {
            var sql = new Sql()
                .Select("*")
                .From<GlobalCondition>(ApplicationContext.Current.DatabaseContext.SqlSyntax)
                .Where<GlobalCondition>(c => c.Guid == guid);

            return Database.FirstOrDefault<GlobalCondition>(sql);
        }

        /// <summary>
        /// Get single global condition by id
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public GlobalCondition GetSingle(int id)
        {
            var sql = new Sql()
                .Select("*")
                .From<GlobalCondition>(ApplicationContext.Current.DatabaseContext.SqlSyntax)
                .Where<GlobalCondition>(c => c.Id == id);

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
