using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;
using uCondition.Models;

namespace uCondition.Predicates.Members
{
    public class MemberIsInterestedIn : Predicate
    {
        public MemberIsInterestedIn()
        {
            Name = "Member Is Interested In";
            Alias = "MemberIsInterestedIn";
            Category = "Member Behaviour";
            Icon = "icon-user";
            Fields = new List<EditableProperty>
            {
                new EditableProperty("Interests", "Interests", "Interest Dropdown"),//, EditablePropertyDisplayMode.IsPreValue),
                new EditableProperty("All or Any", "AllOrAny", "uCondition: interested in all or any", EditablePropertyDisplayMode.IsPreValue)             
            };
        }

        public override bool Validate(IFieldValues fieldValues)
        {
            return true;
        }
    }
}
