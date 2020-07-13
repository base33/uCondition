using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uCondition.Core.Models
{
    public class GlobalConditionModel
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public uConditionModel Condition { get; set; }
        public string Created { get; set; }
        public string LastUpdated { get; set; }
    }
}
