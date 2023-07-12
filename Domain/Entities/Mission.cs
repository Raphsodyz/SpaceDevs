using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Mission
    {
        public int Id { get; set; }
        public int LaunchLibraryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public LaunchDesignator LaunchDesignator { get; set; }
        public string Type { get; set; }
        public Orbit Orbit { get; set; }

        public Mission()
        {
            Orbit = new Orbit();
            LaunchDesignator = new LaunchDesignator();
        }
    }

    public class LaunchDesignator
    {

    }

    public class Orbit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbrev { get; set; }
    }
}
