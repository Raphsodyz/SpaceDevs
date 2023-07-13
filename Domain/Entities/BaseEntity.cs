using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
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
    }
}
