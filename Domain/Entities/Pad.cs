using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Pad
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int? AgencyId { get; set; }
        public string Name { get; set; }
        public string InfoUrl { get; set; }
        public string WikiUrl { get; set; }
        public string MapUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location Location { get; set; }
        public string MapImage { get; set; }
        public int TotalLaunchCount { get; set; }

        public Pad()
        {
            Location = new Location();
        }
    }

    public class Location
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string MapImage { get; set; }
        public int TotalLaunchCount { get; set; }
        public int TotalLandingCount { get; set; }
    }
}
