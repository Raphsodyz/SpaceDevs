using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Materializated.Views
{
    public class MissionView
    {
        public int? LaunchLibraryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public Guid? IdOrbit { get; set; }
        public OrbitView Orbit { get; set; }
        public string? LaunchDesignator { get; set; }
    }
}