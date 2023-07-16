using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("LOCATION")]
    public class Location : BaseEntity
    {
        [Column("URL")]
        public string? Url { get; set; }
        [Column("NAME")]
        public string? Name { get; set; }
        [Column("COUNTRY_CODE")]
        public string? CountryCode { get; set; }
        [Column("MAP_IMAGE")]
        public string? MapImage { get; set; }
        [Column("TOTAL_LAUNCH_COUNT")]
        public int? TotalLaunchCount { get; set; }
        [Column("TOTAL_LANDING_COUNT")]
        public int? TotalLandingCount { get; set; }
    }
}
