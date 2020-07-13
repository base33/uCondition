using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using uCondition.Interfaces;
using uCondition.Models;
using Umbraco.Web;
using Umbraco.Web.Security;

namespace uCondition.Predicates.Members
{
    public class IsMemberInAnyGroup : Predicate
    {
        public IsMemberInAnyGroup()
        {
            Name = "Is member in group?";
            Alias = "uCondition.IsMemberInGroup";
            Icon = "icon-user";
            Category = "Member";
            Fields = new List<EditableProperty>
            {
                new EditableProperty("Group", "Group", "membergrouppicker"),
                new EditableProperty("Member of all or any?", "MatchType", "uCondition type: all or any member group", EditablePropertyDisplayMode.IsPreValue)
            };
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            var all = fieldValues.GetValue<int>("MatchType");
            var prevalue = Umbraco.GetPreValueAsString(all);
            var membership = new MembershipHelper(UmbracoContext.Current);

            bool inGroups = false;
            if (prevalue == "All")
                inGroups = fieldValues.GetValue("Group").Split(',').All(c => Roles.IsUserInRole(membership.CurrentUserName, c));
            else
                inGroups = fieldValues.GetValue("Group").Split(',').Any(c => Roles.IsUserInRole(membership.CurrentUserName, c));

            return membership.IsLoggedIn() && inGroups;
        }
    }
}
