using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("update_log_routine")]
    public class UpdateLog : BaseEntity
    {
        [Column("transction_date")]
        public DateTime TransactionDate { get; set; }
        [Column("offset_data")]
        public int OffSet { get; set; }
        [Column("success")]
        public bool Success { get; set; }
        [Column("message")]
        public string Message { get; set; }
        [Column("entity_count")]
        public int EntityCount { get; set; }
        [Column("origin")]
        public string Origin { get; set; }
    }
}
