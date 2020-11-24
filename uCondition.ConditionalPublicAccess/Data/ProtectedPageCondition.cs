using NPoco;

namespace uCondition.ConditionalPublicAccess.Data
{
    public class ProtectedPageCondition
    {
        [Column(Name = nameof(FalseActionNodeId))]
        public int FalseActionNodeId { get; set; }

        [Column(Name = nameof(Condition))]
        public string Condition { get; set; }
    }
}