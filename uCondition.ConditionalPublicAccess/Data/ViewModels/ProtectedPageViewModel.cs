using System.Collections.Generic;

namespace uCondition.ConditionalPublicAccess.Data.ViewModels
{
    public class ProtectedPageViewModel : ProtectedPageConditionViewModel
    {
        public List<ProtectedPageConditionViewModel> Conditions { get; set; } = new List<ProtectedPageConditionViewModel>();
    }
}
