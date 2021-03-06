﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uCondition.Core.Models.Converter;

namespace uCondition.Core.Models
{
    [JsonConverter(typeof(PredicateConverter))]
    public class PredicateBase
    {
        public string Type { get; set; }
        public bool RightOperationAnd { get; set; }
    }
}
