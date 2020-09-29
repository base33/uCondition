using NPoco;
using Umbraco.Core.Persistence.DatabaseAnnotations;

namespace uCondition.Core.Data.Models
{
    [PrimaryKey("Id", AutoIncrement = true)]
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
