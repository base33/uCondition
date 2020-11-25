using System;
using System.Collections.Generic;
using uCondition.ConditionalPublicAccess.Data;
using Umbraco.Core.Persistence;
using Umbraco.Core.Scoping;

namespace uCondition.ConditionalPublicAccess.ProtectedPageProviders
{
    internal class DatabaseProtectedPageProvider : IProtectedPageProvider
    {
        private readonly IScopeProvider _scopeProvider;

        public DatabaseProtectedPageProvider(IScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public ProtectedPage GetById(int nodeId)
        {
            return GetById(nodeId.ToString());
        }

        public ProtectedPage GetById(string nodeId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql()
                    .Select("*")
                    .From<ProtectedPage>()
                    .Where<ProtectedPage>(p => p.NodeId.Equals(nodeId));

                return scope.Database.FirstOrDefault<ProtectedPage>(sql);
            }
        }

        public IEnumerable<ProtectedPage> GetAll()
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql()
                    .Select("*")
                    .From<ProtectedPage>();

                return scope.Database.Fetch<ProtectedPage>(sql);
            }
        }

        public void Delete(int nodeId)
        {
            Delete(nodeId.ToString());
        }

        public void Delete(string nodeId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                try
                {
                    _ = scope.Database
                            .DeleteMany<ProtectedPage>()
                            .Where(p => p.NodeId.Equals(nodeId))
                            .Execute();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public void SaveOrUpdate(ProtectedPage protectedPage)
        {
            try
            {
                using (var scope = _scopeProvider.CreateScope(autoComplete: true))
                {
                    if (protectedPage.Id > 0)
                    {
                        _ = scope.Database.Update(protectedPage);
                    }
                    else
                    {
                        scope.Database.Save(protectedPage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}