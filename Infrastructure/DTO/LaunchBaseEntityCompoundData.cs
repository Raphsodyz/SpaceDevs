using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DTO
{
    public class LaunchBaseEntityCompoundData
    {
        public BaseEntityLaunchData LaunchBaseEntity { get; set; }
        public BaseEntityData StatusBaseEntity { get; set; }
        public BaseEntityData LaunchServiceProviderBaseEntity { get; set; }
        public BaseEntityData RocketBaseEntity { get; set; }
        public BaseEntityData ConfigurationBaseEntity { get; set; }
        public BaseEntityData MissionBaseEntity { get; set; }
        public BaseEntityData OrbitBaseEntity { get; set; }
        public BaseEntityData PadBaseEntity { get; set; }
        public BaseEntityData LocationBaseEntity { get; set; }
    }
}