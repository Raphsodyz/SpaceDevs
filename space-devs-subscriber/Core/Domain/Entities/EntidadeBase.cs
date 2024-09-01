using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Core.Domain.Entities
{
    public abstract class EntidadeBase
    {
        [Key]
        [Column("id")]
        public virtual Guid Id { get; set; }

        [Column("atualizado_em")]
        public virtual DateTime AtualizadoEm { get; set; }

        [Column("criado_em")]
        public virtual DateTime CriadoEm { get; set; }


        [NotMapped]
        [JsonIgnore]
        public bool? EntidadeNova => CriadoEm == DateTime.MinValue ? null : false;
        
        public virtual void AtualizaDatas()
        {
            if (EntidadeNova ?? true)
            {
                CriadoEm = DateTime.Now;
                AtualizadoEm = DateTime.Now;
            }
            else
                AtualizadoEm = DateTime.Now;
        }
    }
}