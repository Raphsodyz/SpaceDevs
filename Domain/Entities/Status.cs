using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("STATUS")]
    public class Status : BaseEntity
    {
        [Column("NAME")]
        public string? Name { get; set; }
    }
}
