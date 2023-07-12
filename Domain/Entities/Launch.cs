using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Launch
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public int LaunchLibraryId { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public int IdStatus { get; set; }
        public Status Status { get; set; }
        public DateTime Net { get; set; }
        public DateTime WindowEnd { get; set; }
        public DateTime WindowStart { get; set; }
        public bool Inhold { get; set; }
        public bool TbdTime { get; set; }
        public bool TbdDate { get; set; }
        public int Probability { get; set; }
        public string HoldReason { get; set; }
        public string FailReason { get; set; }
        public string Hashtag { get; set; }
        public int IdLaunchServiceProvider { get; set; }
        public LaunchServiceProvider LaunchServiceProvider { get; set; }
        public int IdRocket { get; set; }
        public Rocket Rocket { get; set; }

    }
}
