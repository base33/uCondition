using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;
using uCondition.ConditionalPublicAccess.Data;
using uCondition.ConditionalPublicAccess.Data.ViewModels;
using uCondition.ConditionalPublicAccess.ProtectedPageProviders;
using uCondition.Core.Models.Converter;
using Umbraco.Web.WebApi;

namespace uCondition.ConditionalPublicAccess
{
    [IsBackOffice]
    public class UConditionPublicAccessApiController : UmbracoApiController
    {
        private readonly IProtectedPageProvider _protectedPageProvider;
        private readonly JavaScriptSerializer _scriptSerializer;
        private readonly uConditionModelConverter _uConditionModelConverter;

        public UConditionPublicAccessApiController(IProtectedPageProvider protectedPageProvider)
        {
            _protectedPageProvider = protectedPageProvider;
            _scriptSerializer = new JavaScriptSerializer();
            _uConditionModelConverter = new uConditionModelConverter();
        }

        [UmbracoAuthorize]
        [HttpPost]
        public void Save([FromUri] int id, [FromBody] ProtectedPageViewModel model)
        {
            var protectedPageConditionList = new List<ProtectedPageCondition>();

            foreach (var condition in model.Conditions)
            {
                var protectedPageCondition = new ProtectedPageCondition()
                {
                    Condition = _scriptSerializer.Serialize(condition.Condition),
                    FalseActionNodeId = condition.FalseActionNodeId
                };

                protectedPageConditionList.Add(protectedPageCondition);
            }

            var protectedPage = _protectedPageProvider.GetById(id);

            if (protectedPage == null)
            {
                protectedPage = new ProtectedPage
                {
                    NodeId = id.ToString()
                };
            }

            protectedPage.Conditions = protectedPageConditionList;
            protectedPage.Condition = _scriptSerializer.Serialize(model.Condition);
            protectedPage.FalseActionNodeId = model.FalseActionNodeId;

            //            if (protectedPages.Pages.Any(c => c.Id == id))
            //            {
            //                var protectedPage = protectedPages.Pages.FirstOrDefault(p => p.Id == id);
            //
            //                protectedPage.Conditions = protectedPageConditionList;
            //                protectedPage.Condition = scriptSerializer.Serialize(model.Condition);
            //                protectedPage.FalseActionNodeId = model.FalseActionNodeId;
            //            }
            //            else
            //            {
            //                var pages = protectedPages.Pages;
            //                var protectedPage = new ProtectedPage
            //                {
            //                    NodeId = id,
            //                    Condition = scriptSerializer.Serialize(model.Condition),
            //                    FalseActionNodeId = model.FalseActionNodeId,
            //                    Conditions = protectedPageConditionList
            //                };
            //                pages.Add(protectedPage);
            //            }
            //
            //            protectedPageProvider.Save(protectedPages);

            _protectedPageProvider.SaveOrUpdate(protectedPage);
        }

        [UmbracoAuthorize]
        [HttpGet]
        public ProtectedPageViewModel Get([FromUri] int id)
        {
            var protectedPage = _protectedPageProvider.GetById(id);

            if (protectedPage == null)
            {
                return null;
            }

            var protectedPageViewModel = new ProtectedPageViewModel();

            foreach (var condition in protectedPage.Conditions)
            {
                var conditionViewModel = new ProtectedPageConditionViewModel()
                {
                    Condition = _uConditionModelConverter.Convert(condition.Condition),
                    FalseActionNodeId = condition.FalseActionNodeId
                };

                protectedPageViewModel.Conditions.Add(conditionViewModel);
            }

            protectedPageViewModel.Condition = _uConditionModelConverter.Convert(protectedPage.Condition);
            protectedPageViewModel.FalseActionNodeId = protectedPage.FalseActionNodeId;

            return protectedPageViewModel;
        }

        [UmbracoAuthorize]
        [HttpDelete]
        public void Delete([FromUri] int id)
        {
            _protectedPageProvider.Delete(id);
        }
    }
}