using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("PAD")]
    public class Pad : BaseEntity
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("URL")]
        public string Url { get; set; }
        [Column("AGENCY_ID")]
        public int? AgencyId { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("INFO_URL")]
        public string InfoUrl { get; set; }
        [Column("WIKI_URL")]
        public string WikiUrl { get; set; }
        [Column("MAP_URL")]
        public string MapUrl { get; set; }
        [Column("LATITUDE")]
        public double Latitude { get; set; }
        [Column("LONGITUDE")]
        public double Longitude { get; set; }
        [Column("ID_LOCATION")]
        public int IdLocation { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(IdLocation))]
        public Location Location { get; set; }
        [Column("MAP_IMAGE")]
        public string MapImage { get; set; }
        [Column("TOTAL_LAUNCH_COUNT")]
        public int TotalLaunchCount { get; set; }

        public Pad()
        {
            Location = new Location();
        }
    }
}
