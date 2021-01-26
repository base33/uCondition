using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace uCondition.ConditionalPublicAccess.Data
{
    public class ProtectedPageCondition
    {
        [Column(Name = nameof(FalseActionNodeId))]
        public int FalseActionNodeId { get; set; }

        [Column(Name = nameof(Condition))]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Condition { get; set; }
    }
}