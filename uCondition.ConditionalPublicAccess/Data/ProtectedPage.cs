using NPoco;
using System.Collections.Generic;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace uCondition.ConditionalPublicAccess.Data
{
    [PrimaryKey(nameof(Id), AutoIncrement = true)]
    [TableName(nameof(ProtectedPage))]
    public class ProtectedPage : ProtectedPageCondition
    {
        [Column(Name = nameof(Id))]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column(Name = nameof(NodeId))]
        public string NodeId { get; set; }

        [Column(Name = nameof(Conditions))]
        [SerializedColumn]
        [SpecialDbType(SpecialDbTypes.NVARCHARMAX)]
        public List<ProtectedPageCondition> Conditions { get; set; }
    }
}