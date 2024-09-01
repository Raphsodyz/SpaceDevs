using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    [Table("orbit")]
    public class Orbit : BaseEntity
    {
        [Column("name")]
        public string? Name { get; set; }
        [Column("abbrev")]
        public string? Abbrev { get; set; }
    }
}
