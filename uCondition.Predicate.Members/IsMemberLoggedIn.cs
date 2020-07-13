using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;
using uCondition.Models;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace uCondition.Predicates.Members
{
    public class IsMemberLoggedIn : Predicate
    {
        public IsMemberLoggedIn()
        {
            Name = "Is member logged in?";
            Alias = "uCondition.IsMemberLoggedIn";
            Icon = "icon-user";
            Category = "Member";
            Fields = new List<EditableProperty>();
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            var membership = new MembershipHelper(UmbracoContext.Current);
            return membership.IsLoggedIn();
        }
    }
}
