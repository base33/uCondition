using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration;

namespace uCondition.Core.Compositions
{
    //public class RegisterUConditionApiRoutesComponent : IComponent
    //{
    //    private readonly IGlobalSettings _globalSettings;

    //    public RegisterUConditionApiRoutesComponent(IGlobalSettings globalSettings)
    //    {
    //        _globalSettings = globalSettings;
    //    }

    //    public void Initialize()
    //    {
    //        RouteTable.Routes.MapRoute("cats", _globalSettings.GetUmbracoMvcArea() + "/backoffice/cats/{action}/{id}", new
    //        {
    //            controller = "cats",
    //            action = "meow",
    //            id = UrlParameter.Optional
    //        })
    //    }

    //    public void Terminate()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
