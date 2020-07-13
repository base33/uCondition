using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCondition.Core.Models.Converter
{
    public class uConditionModelConverter
    {
        public uConditionModel Convert(string json)
        {
            return JsonConvert.DeserializeObject<uConditionModel>(json, new JsonConverter[] { new PredicateConverter() });
        }
    }
}
