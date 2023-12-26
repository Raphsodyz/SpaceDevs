using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class SearchLaunchDTO
    {
        public string? Mission { get; set; }
        public string? Rocket { get; set; }
        public string? Location { get; set; }
        public string? Launch { get; set; }
    }
}