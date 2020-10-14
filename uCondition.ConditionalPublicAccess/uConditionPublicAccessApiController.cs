using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using uCondition.ConditionalPublicAccess.Data;
using uCondition.ConditionalPublicAccess.Data.ViewModels;
using uCondition.Core.Models.Converter;
using Umbraco.Web.WebApi;

namespace uCondition.ConditionalPublicAccess
{
    [IsBackOffice]
    public class UConditionPublicAccessApiController : UmbracoApiController
    {
        [UmbracoAuthorize]
        [HttpPost]
        public void Save([FromUri] int id, [FromBody] ProtectedPageViewModel model)
        {
            var protectedPageProvider = new ProtectedPageProvider();
            var protectedPages = protectedPageProvider.Load();
            var scriptSerializer = new JavaScriptSerializer();
            var protectedPageConditionList = new List<ProtectedPageCondition>();

            foreach (var condition in model.Conditions)
            {
                var protectedPageCondition = new ProtectedPageCondition()
                {
                    Condition = scriptSerializer.Serialize(condition.Condition),
                    FalseActionNodeId = condition.FalseActionNodeId
                };

                protectedPageConditionList.Add(protectedPageCondition);
            }

            if (protectedPages.Pages.Any(c => c.Id == id))
            {
                var protectedPage = protectedPages.Pages.FirstOrDefault(p => p.Id == id);

                protectedPage.Conditions = protectedPageConditionList;
                protectedPage.Condition = scriptSerializer.Serialize((object)model.Condition);
                protectedPage.FalseActionNodeId = model.FalseActionNodeId;
            }
            else
            {
                var pages = protectedPages.Pages;
                var protectedPage = new ProtectedPage();

                protectedPage.Id = id;
                protectedPage.Condition = scriptSerializer.Serialize(model.Condition);
                protectedPage.FalseActionNodeId = model.FalseActionNodeId;
                protectedPage.Conditions = protectedPageConditionList;
                pages.Add(protectedPage);
            }

            protectedPageProvider.Save(protectedPages);
        }

        [UmbracoAuthorize]
        [HttpGet]
        public ProtectedPageViewModel Get([FromUri] int id)
        {
            var protectedPages = new ProtectedPageProvider().Load();
            var protectedPage = protectedPages?.Pages.FirstOrDefault(p => p.Id == id);

            if (protectedPage == null)
            {
                return null;
            }

            var protectedPageViewModel = new ProtectedPageViewModel();

            foreach (var condition in protectedPage.Conditions)
            {
                var conditionViewModel = new ProtectedPageConditionViewModel()
                {
                    Condition = new uConditionModelConverter().Convert(condition.Condition),
                    FalseActionNodeId = condition.FalseActionNodeId
                };

                protectedPageViewModel.Conditions.Add(conditionViewModel);
            }

            protectedPageViewModel.Condition = new uConditionModelConverter().Convert(protectedPage.Condition);
            protectedPageViewModel.FalseActionNodeId = protectedPage.FalseActionNodeId;

            return protectedPageViewModel;
        }

        [UmbracoAuthorize]
        [HttpGet]
        public void Delete([FromUri] int id)
        {
            var protectedPageProvider = new ProtectedPageProvider();
            var protectedPages = protectedPageProvider.Load();
            var protectedPage = protectedPages.Pages.FirstOrDefault(c => c.Id == id);

            protectedPages.Pages.Remove(protectedPage);
            protectedPageProvider.Save(protectedPages);
        }
    }
}