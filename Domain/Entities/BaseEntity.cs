using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [Column("id")]
        public virtual Guid Id { get; set; }

        [Column("id_from_api")]
        [JsonIgnore]
        public int? IdFromApi { get; set; }

        [Column("atualization_date")]
        public virtual DateTime AtualizationDate { get; set; }

        [Column("imported_t")]
        public virtual DateTime ImportedT { get; set; }

        [NotMapped]
        [JsonIgnore]
        private EStatus? _statusEnum { get; set; }

        [Column("status")]
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
        public bool? IsNew => ImportedT == DateTime.MinValue ? null : false;
        
        public virtual void BeforeSave()
        {
            bool newEntity = IsNew ?? true;
            if (newEntity)
            {
                ImportedT = DateTime.Now;
                AtualizationDate = DateTime.Now;
                EntityStatus = EStatus.PUBLISHED.GetDisplayName();
            }
            else
                AtualizationDate = DateTime.Now;
        }
    }
}
