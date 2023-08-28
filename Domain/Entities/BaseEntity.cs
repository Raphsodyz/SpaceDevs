using Domain.Enum;
using Domain.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("ID")]
        public virtual int Id { get; set; }

        [Column("ID_FROM_API")]
        [JsonIgnore]
        public int? IdFromApi { get; set; }

        [Column("ATUALIZATION_DATE")]
        public virtual DateTime AtualizationDate { get; set; } = DateTime.Now;

        [Column("IMPORTED_T")]
        public virtual DateTime ImportedT { get; set; } = DateTime.Now;

        [NotMapped]
        [JsonIgnore]
        private EStatus? _statusEnum { get; set; }

        [Column("STATUS")]
        public string EntityStatus
        {
            get
            {
                return _statusEnum.GetDisplayName();
            }
            set
            {
                try
                {
                    if (value == null)
                        _statusEnum = EStatus.DRAFT;
                    else
                        _statusEnum = (EStatus)System.Enum.Parse(typeof(EStatus), value);
                }
                catch
                {
                    _statusEnum = null;
                }
            }
        }

        [NotMapped]
        [JsonIgnore]
        public bool IsNew => Id == 0;

        public virtual void BeforeSave()
        {
            if (IsNew)
            {
                ImportedT = DateTime.Now;
                AtualizationDate = DateTime.Now;
            }
            else
                AtualizationDate = DateTime.Now;
        }
    }
}
