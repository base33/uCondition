using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Interfaces;
using uCondition.Models;
//using Umbraco.Core.Models;

namespace uCondition.Predicates.Members
{
    //public class MemberLikedImage : Predicate
    //{
    //    public MemberLikedImage()
    //    {
    //        Name = "Member Liked Image";
    //        Alias = "MemberLikedImage";
    //        Category = "Member Behaviour";
    //        Icon = "icon-user";
    //        Fields = new List<EditableProperty>
    //        {
    //            new EditableProperty
    //            {
    //                Name = "Images",
    //                Alias = "Images",
    //                Control = "Multiple Media Picker"
    //            }
    //        };
    //    }

    //    public override bool Validate(IFieldValues fieldValues)
    //    {
    //        var imageIds = fieldValues.GetValue<string>("Images").Split(new[] { ',' });
    //        var images = Umbraco.TypedMedia(imageIds).Where(c => c != null);
    //        var firstImage = images.First();
    //        return true;
    //    }
    //}
}
