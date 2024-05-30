using System.ComponentModel.DataAnnotations.Schema;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;

namespace Domain.Entities
{
    [Table("update_log_routine")]
    public class UpdateLog : BaseEntity
    {
        [Column("transaction_date")]
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

        #region Constructors

        public UpdateLog()
        {
        }

        public UpdateLog(int offset, string message, int entityCount, bool success)
        {
            TransactionDate = DateTime.Now;
            OffSet = offset;
            Success = success;
            Message = message;
            EntityCount = entityCount;
            Origin = EOrigin.API_UPDATE.GetDisplayName();
            EntityStatus = EStatus.PUBLISHED.GetDisplayName();
        }

        #endregion
    }
}
