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

        private DateTime? atualizationDate;
        
        [Column("ATUALIZATION_DATE")]
        public virtual DateTime AtualizationDate { 
            get 
            {
                if(atualizationDate == DateTime.MinValue)
                    atualizationDate = DateTime.Now;

                return atualizationDate.Value;
            }
            set 
            {
                atualizationDate = DateTime.Now;
            }
        }

        private DateTime? importedT;

        [Column("IMPORTED_T")]
        public virtual DateTime ImportedT { 
            get 
            {
                if (importedT == DateTime.MinValue)
                    importedT = DateTime.Now;

                return importedT.Value;
            }
            set 
            {
                if (value != null && value != new DateTime() && value != DateTime.MinValue)
                    importedT = value;
                else
                    importedT = DateTime.Now;
            }
        }

        [NotMapped]
        [JsonIgnore]
        private EStatus? _statusEnum { get; set; }

        [Column("STATUS")]
        public string EntityStatus { 
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
    }
}
