using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Materializated.Views
{
    public class ConfigurationView
    {
        public int? LaunchLibraryId { get; set; }
        public string? Url { get; set; }
        public string? Name { get; set; }
        public string? Family { get; set; }
        public string? FullName { get; set; }
        public string? Variant { get; set; }
    }
}