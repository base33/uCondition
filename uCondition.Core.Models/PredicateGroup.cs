using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Core.Models.Converter;

namespace uCondition.Core.Models
{
    public class PredicateGroup : PredicateBase
    {
        public IEnumerable<PredicateBase> Conditions { get; set; }
    }
}
