using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class LaunchBaseEntityCompoundDTO
    {
        public BaseEntityLaunchDTO LaunchBaseEntity { get; set; }
        public BaseEntityDTO StatusBaseEntity { get; set; }
        public BaseEntityDTO LaunchServiceProviderBaseEntity { get; set; }
        public BaseEntityDTO RocketBaseEntity { get; set; }
        public BaseEntityDTO ConfigurationBaseEntity { get; set; }
        public BaseEntityDTO MissionBaseEntity { get; set; }
        public BaseEntityDTO OrbitBaseEntity { get; set; }
        public BaseEntityDTO PadBaseEntity { get; set; }
        public BaseEntityDTO LocationBaseEntity { get; set; }
    }
}