using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("ORBIT")]
    public class Orbit : BaseEntity
    {
        [Column("NAME")]
        public string? Name { get; set; }
        [Column("ABBREV")]
        public string? Abbrev { get; set; }
    }
}
