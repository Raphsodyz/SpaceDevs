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

        private DateTime? _atualizationDate;
        
        [Column("ATUALIZATION_DATE")]
        public virtual DateTime AtualizationDate { 
            get 
            {
                if(!_atualizationDate.HasValue)
                    _atualizationDate = DateTime.Now;
                return _atualizationDate.Value;
            }
            set 
            {
                if (value != DateTime.MinValue && value != new DateTime())
                    _atualizationDate = value;
                else
                    _atualizationDate = DateTime.Now;
            }
        }

        private DateTime? _importedT;

        [Column("IMPORTED_T")]
        public virtual DateTime importedT { 
            get 
            {
                if (!_importedT.HasValue)
                    _importedT = DateTime.Now;
                return _importedT.Value;
            }
            set 
            {
                if (value == DateTime.MinValue || value == new DateTime())
                    _importedT = DateTime.Now;
                else
                    _importedT = value;
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
