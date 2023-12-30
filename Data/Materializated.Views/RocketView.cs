using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Materializated.Views
{
    public class RocketView
    {
        public Guid? IdConfiguration { get; set; }
        public ConfigurationView Configuration { get; set; }
    }
}