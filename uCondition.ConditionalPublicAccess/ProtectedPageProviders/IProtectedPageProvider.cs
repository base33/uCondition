using System.Collections.Generic;
using uCondition.ConditionalPublicAccess.Data;

namespace uCondition.ConditionalPublicAccess.ProtectedPageProviders
{
    public interface IProtectedPageProvider
    {
        ProtectedPage GetById(string nodeId);

        ProtectedPage GetById(int nodeId);

        IEnumerable<ProtectedPage> GetAllById(string nodeIds);

        ProtectedPage GetLastChild(string nodeIds);

        void Delete(int nodeId);

        void Delete(string nodeId);

        void SaveOrUpdate(ProtectedPage protectedPage);

        bool HasAny(int nodeId);

        bool HasAny(string nodeIds);
    }
}