using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Domain.Entities
{
    [Table("rocket")]
    public class Rocket : BaseEntity
    {
        [Column("id_configuration")]
        public Guid? IdConfiguration { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdConfiguration))]
        public Configuration Configuration { get; set; }

        #region Constructors

        public Rocket()
        {
            
        }

        public Rocket(int? idFromApi, Guid? idConfiguration)
        {
            IdFromApi = idFromApi;
            IdConfiguration = idConfiguration;
        }

        #endregion
    }
}
