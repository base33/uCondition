using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;
using uCondition.Models;

namespace uCondition.Predicates.Members
{
    public class MemberSharedPage : Predicate
    {
        public MemberSharedPage()
        {
            Name = "Member shared page on social media";
            Alias = "MemberSharedPage";
            Category = "Member Behaviour";
            Icon = "icon-user";
            Fields = new List<EditableProperty>
            {
                new EditableProperty("Page", "PageId", "Content Picker")         
            };
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            return true;
        }
    }
}
