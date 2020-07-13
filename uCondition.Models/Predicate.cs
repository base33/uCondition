using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uCondition.Interfaces;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Umbraco.Web.Security;

namespace uCondition.Models
{
    public abstract class Predicate : Base
    {
        public Predicate(string type = "Predicate") : base(type)
        {
            Name = "";
            Alias = "";
            Icon = "";
            Category = "";
            Fields = new List<EditableProperty>();
            
            
            UmbracoContext.EnsureContext(new HttpContextWrapper(HttpContext.Current), ApplicationContext.Current,
                new WebSecurity(new HttpContextWrapper(HttpContext.Current), ApplicationContext.Current),
                UmbracoConfig.For.UmbracoSettings(),
                UrlProviderResolver.Current.Providers,
                false);
            Umbraco = new UmbracoHelper(UmbracoContext.Current);
        }

        [JsonIgnore]
        public UmbracoHelper Umbraco { get; }

        public abstract bool Validate(IFieldValues fieldValues);
    }
}
