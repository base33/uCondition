using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using uCondition.Interfaces;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Composing;
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
        }

        public abstract bool Validate(IFieldValues fieldValues);
    }
}
