using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<ProtectedPage> GetAllById(string nodeIds)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql()
                    .Select("*")
                    .From<ProtectedPage>()
                    .WhereIn<ProtectedPage>(x => x.NodeId, nodeIds.Split(','));

                return scope.Database.Fetch<ProtectedPage>(sql);
            }
        }

        public ProtectedPage GetLastChild(string nodeIds)
        {
            var protectedPages = GetAllById(nodeIds);

            return protectedPages
                .OrderByDescending(p => p.NodeId)
                .FirstOrDefault();
        }

        public void Delete(int nodeId)
        {
            Delete(nodeId.ToString());
        }

        public void Delete(string nodeId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                _ = scope.Database
                    .DeleteMany<ProtectedPage>()
                    .Where(p => p.NodeId.Equals(nodeId))
                    .Execute();
            }
        }

        public void SaveOrUpdate(ProtectedPage protectedPage)
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

        public bool HasAny(string nodeIds)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql()
                    .Select("*")
                    .From<ProtectedPage>()
                    .WhereIn<ProtectedPage>(x => x.NodeId, nodeIds.Split(','));

                var result = scope.Database.Fetch<ProtectedPage>(sql);

                return result != null && result.Any();
            }
        }

        public bool HasAny(int nodeId)
        {
            return HasAny(nodeId.ToString());
        }
    }
}