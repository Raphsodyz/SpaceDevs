using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.ViewModel
{
    public class RequestLaunchViewModel
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<LaunchViewModel> Results { get; set; }
    }
}