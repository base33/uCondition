using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace uCondition.Core.Data.Models
{
    [PrimaryKey("Id", autoIncrement = true)]
    [TableName("GlobalConditions")]
    public class GlobalCondition
    {
        [Column(Name = "Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column(Name = "Guid")]
        public string Guid { get; set; }

        [Column(Name = "Name")]
        public string Name { get; set; } 

        [Column(Name = "Data")]
        [SpecialDbType(SpecialDbTypes.NTEXT)]
        public string Data { get; set; }

        [Column(Name = "Created")]
        public DateTime Created { get; set; }

        [Column(Name = "LastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}
