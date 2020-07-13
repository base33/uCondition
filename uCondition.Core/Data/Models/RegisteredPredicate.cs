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
    [TableName("RegisteredPredicates")]
    public class RegisteredPredicate
    {
        [Column(Name = "Id")]
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        [Column(Name = "Alias")]
        public string Alias { get; set; }
    }
}
