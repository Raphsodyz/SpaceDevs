using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("ROCKET")]
    public class Rocket : BaseEntity
    {
        [Column("ID_CONFIGURATION")]
        public Guid? IdConfiguration { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdConfiguration))]
        public Configuration Configuration { get; set; }
    }
}
