using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Materializated.Views
{
    public class LocationView
    {
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? CountryCode { get; set; }
        public string? MapImage { get; set; }
        public int? TotalLaunchCount { get; set; }
        public int? TotalLandingCount { get; set; }
    }
}