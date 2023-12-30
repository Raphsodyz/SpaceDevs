using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Materializated.Views
{
    public class PadView
    {
        public string? Url { get; set; }
        public int? AgencyId { get; set; }
        public string? Name { get; set; }
        public string? InfoUrl { get; set; }
        public string? WikiUrl { get; set; }
        public string? MapUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public Guid? IdLocation { get; set; }
        public LocationView Location { get; set; }
        public string? MapImage { get; set; }
        public int? TotalLaunchCount { get; set; }
    }
}