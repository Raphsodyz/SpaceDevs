using Domain.Entities;

namespace Domain.Aggregates
{
    public class LaunchBaseEntityAggregate
    {
        public BaseEntity LaunchBaseEntity { get; set; }
        public BaseEntity StatusBaseEntity { get; set; }
        public BaseEntity LaunchServiceProviderBaseEntity { get; set; }
        public BaseEntity RocketBaseEntity { get; set; }
        public BaseEntity ConfigurationBaseEntity { get; set; }
        public BaseEntity MissionBaseEntity { get; set; }
        public BaseEntity OrbitBaseEntity { get; set; }
        public BaseEntity PadBaseEntity { get; set; }
        public BaseEntity LocationBaseEntity { get; set; }
        public Guid? ApiGuid { get; set; }
    }
}