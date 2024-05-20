using Application.DTO.Entities;

namespace Application.DTO.Aggregates
{
    public class LaunchBaseEntityAggregate
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