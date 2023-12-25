using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("status")]
    public class Status : BaseEntity
    {
        [Column("name")]
        public string? Name { get; set; }

        [Column("abbrev")]
        public string? Abbrev { get; set; }

        [Column("description")]
        public string? Description  { get; set; }
    }
}
