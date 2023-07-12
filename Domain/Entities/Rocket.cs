using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Rocket
    {
        public int Id { get; set; }
        public Configuration Configuration { get; set; }
        public Rocket() 
        {
            Configuration = new Configuration();
        }
    }

    public class Configuration
    {
        public int Id { get; set; }
        public int LaunchLibraryId { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string FullName { get; set; }
        public string Variant { get; set; }
    }
}
