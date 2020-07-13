using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using uCondition.Interfaces;

namespace uCondition.Models
{
    internal abstract class Action : Base
    {
        public Action() : base("Action")
        {

        }

        public abstract void Do(IFieldValues fieldValues, HtmlHelper htmlHelper);
    }
}
