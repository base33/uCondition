using uCondition.Core.Models;

namespace uCondition.ConditionalPublicAccess.Data.ViewModels
{
    public class ProtectedPageConditionViewModel
    {
        public uConditionModel Condition = new uConditionModel();

        public int FalseActionNodeId { get; set; }
    }
}
