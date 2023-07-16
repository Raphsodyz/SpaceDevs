using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UpdateLog : BaseEntity
    {
        public DateTime TransactionDate { get; set; }
        public int OffSet { get; set; }
        public bool Success { get; set; }
    }
}
