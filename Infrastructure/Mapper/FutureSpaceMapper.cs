using AutoMapper;
using Domain.Entities;
using Cross.Cutting.Helper;
using Domain.Materializated.Views;
using Infrastructure.DTO;

namespace Infrastructure.Mapper
{
    public class FutureSpaceMapper : Profile
    {
        public FutureSpaceMapper()
        {
            CreateMap<StatusDTO, Status>().ReverseMap();
            CreateMap<RocketDTO, Rocket>().ReverseMap();
            CreateMap<ConfigurationDTO, Configuration>().ReverseMap();
            CreateMap<LaunchServiceProviderDTO, LaunchServiceProvider>().ReverseMap();
            CreateMap<LocationDTO, Location>().ReverseMap();
            CreateMap<MissionDTO, Mission>().ReverseMap();
            CreateMap<OrbitDTO, Orbit>().ReverseMap();
            CreateMap<PadDTO, Pad>().ReverseMap();
            CreateMap<Pagination<LaunchView>, Pagination<Launch>>().ReverseMap();
            CreateMap<LaunchDTO, Launch>()
                .ForMember(
                    entity => entity.ApiGuid,
                    dto => dto.MapFrom(src => src.Id))
                .ForMember(
                    entity => entity.Id,
                    dto => dto.Ignore())
                .ForMember(
                    entity => entity.LaunchLibraryId,
                    dto => dto.MapFrom(src => src.Launch_Library_Id))
                .ForPath(
                    entity => entity.Status,
                    dto => dto.MapFrom(src => src.Status))
                .ForPath(
                    entity => entity.Status.Id,
                    dto => dto.Ignore())
                .ForMember(
                    entity => entity.WindowEnd,
                    dto => dto.MapFrom(src => src.Window_End))
                .ForMember(
                    entity => entity.WindowStart,
                    dto => dto.MapFrom(src => src.Window_Start))
                .ForMember(
                    entity => entity.LaunchServiceProvider,
                    dto => dto.MapFrom(src => src.Launch_Service_Provider))
                .ForPath(
                    entity => entity.Rocket,
                    dto => dto.MapFrom(src => src.Rocket))
                .ForPath(
                    entity => entity.Rocket.Configuration,
                    dto => dto.MapFrom(src => src.Rocket.Configuration))
                .ForPath(
                    entity => entity.Rocket.Configuration.LaunchLibraryId,
                    dto => dto.MapFrom(src => src.Rocket.Configuration.Launch_Library_Id))
                .ForPath(
                    entity => entity.Rocket.Configuration.FullName,
                    dto => dto.MapFrom(src => src.Rocket.Configuration.full_name))
                .ForPath(
                    entity => entity.Mission,
                    dto => dto.MapFrom(src => src.Mission))
                .ForPath(
                    entity => entity.Mission.LaunchLibraryId,
                    dto => dto.MapFrom(src => src.Mission.Launch_Library_Id))
                .ForPath(
                    entity => entity.Mission.Orbit,
                    dto => dto.MapFrom(src => src.Mission.Orbit))
                .ForPath(
                    entity => entity.Pad,
                    dto => dto.MapFrom(src => src.Pad))
                .ForPath(
                    entity => entity.Pad.AgencyId,
                    dto => dto.MapFrom(src => src.Pad.Agency_Id))
                .ForPath(
                    entity => entity.Pad.WikiUrl,
                    dto => dto.MapFrom(src => src.Pad.Wiki_Url))
                .ForPath(
                    entity => entity.Pad.MapUrl,
                    dto => dto.MapFrom(src => src.Pad.Map_Url))
                .ForPath(
                    entity => entity.Pad.InfoUrl,
                    dto => dto.MapFrom(src => src.Pad.Info_Url))
                .ForPath(
                    entity => entity.Pad.Location,
                    dto => dto.MapFrom(src => src.Pad.Location))
                .ForPath(
                    entity => entity.Pad.Location.CountryCode,
                    dto => dto.MapFrom(src => src.Pad.Location.Country_Code))
                .ForPath(
                    entity => entity.Pad.Location.MapImage,
                    dto => dto.MapFrom(src => src.Pad.Location.Map_Image))
                .ForPath(
                    entity => entity.Pad.Location.TotalLaunchCount,
                    dto => dto.MapFrom(src => src.Pad.Location.Total_Launch_Count))
                .ForPath(
                    entity => entity.Pad.Location.TotalLandingCount,
                    dto => dto.MapFrom(src => src.Pad.Location.Total_Landing_Count))
                .ForPath(
                    entity => entity.Pad.MapImage,
                    dto => dto.MapFrom(src => src.Pad.Map_Image))
                .ForPath(
                    entity => entity.Pad.TotalLaunchCount,
                    dto => dto.MapFrom(src => src.Pad.Total_Launch_Count))
                .ForMember(
                    entity => entity.WebcastLive,
                    dto => dto.MapFrom(src => src.Webcast_Live)
                )
            .ReverseMap();

            CreateMap<LaunchBaseEntityCompoundDTO, Launch>()
                .ForPath(entity => entity.Id, dto => dto.MapFrom(src => src.LaunchBaseEntity.Id))
                .ForPath(entity => entity.ApiGuid, dto => 
                {
                    dto.Condition(src => src.Source?.LaunchBaseEntity?.ApiGuid != Guid.Empty && src.Source?.LaunchBaseEntity?.ApiGuid != null);
                    dto.MapFrom(src => src.LaunchBaseEntity.ApiGuid);
                })
                .ForPath(entity => entity.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.LaunchBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.ImportedT, dto => dto.MapFrom(src => src.LaunchBaseEntity.ImportedT))
                .ForPath(entity => entity.EntityStatus, dto => dto.MapFrom(src => src.LaunchBaseEntity.Status))
                .ForPath(entity => entity.Status.Id, dto => dto.MapFrom(src => src.StatusBaseEntity.Id))
                .ForPath(entity => entity.Status.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.StatusBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.Status.ImportedT, dto => dto.MapFrom(src => src.StatusBaseEntity.ImportedT))
                .ForPath(entity => entity.Status.EntityStatus, dto => dto.MapFrom(src => src.StatusBaseEntity.Status))
                .ForPath(entity => entity.LaunchServiceProvider.Id, dto => dto.MapFrom(src => src.LaunchServiceProviderBaseEntity.Id))
                .ForPath(entity => entity.LaunchServiceProvider.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.LaunchServiceProviderBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.LaunchServiceProvider.ImportedT, dto => dto.MapFrom(src => src.LaunchServiceProviderBaseEntity.ImportedT))
                .ForPath(entity => entity.LaunchServiceProvider.EntityStatus, dto => dto.MapFrom(src => src.LaunchServiceProviderBaseEntity.Status))
                .ForPath(entity => entity.Rocket.Id, dto => dto.MapFrom(src => src.RocketBaseEntity.Id))
                .ForPath(entity => entity.Rocket.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.RocketBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.Rocket.ImportedT, dto => dto.MapFrom(src => src.RocketBaseEntity.ImportedT))
                .ForPath(entity => entity.Rocket.EntityStatus, dto => dto.MapFrom(src => src.RocketBaseEntity.Status))
                .ForPath(entity => entity.Rocket.Configuration.Id, dto => dto.MapFrom(src => src.ConfigurationBaseEntity.Id))
                .ForPath(entity => entity.Rocket.Configuration.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.ConfigurationBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.Rocket.Configuration.ImportedT, dto => dto.MapFrom(src => src.ConfigurationBaseEntity.ImportedT))
                .ForPath(entity => entity.Rocket.Configuration.EntityStatus, dto => dto.MapFrom(src => src.ConfigurationBaseEntity.Status))
                .ForPath(entity => entity.Mission.Id, dto => dto.MapFrom(src => src.MissionBaseEntity.Id))
                .ForPath(entity => entity.Mission.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.MissionBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.Mission.ImportedT, dto => dto.MapFrom(src => src.MissionBaseEntity.ImportedT))
                .ForPath(entity => entity.Mission.EntityStatus, dto => dto.MapFrom(src => src.MissionBaseEntity.Status))
                .ForPath(entity => entity.Mission.Orbit.Id, dto => dto.MapFrom(src => src.OrbitBaseEntity.Id))
                .ForPath(entity => entity.Mission.Orbit.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.OrbitBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.Mission.Orbit.ImportedT, dto => dto.MapFrom(src => src.OrbitBaseEntity.ImportedT))
                .ForPath(entity => entity.Mission.Orbit.EntityStatus, dto => dto.MapFrom(src => src.OrbitBaseEntity.Status))
                .ForPath(entity => entity.Pad.Id, dto => dto.MapFrom(src => src.PadBaseEntity.Id))
                .ForPath(entity => entity.Pad.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.PadBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.Pad.ImportedT, dto => dto.MapFrom(src => src.PadBaseEntity.ImportedT))
                .ForPath(entity => entity.Pad.EntityStatus, dto => dto.MapFrom(src => src.PadBaseEntity.Status))
                .ForPath(entity => entity.Pad.Location.Id, dto => dto.MapFrom(src => src.LocationBaseEntity.Id))
                .ForPath(entity => entity.Pad.Location.AtualizationDate, dto => 
                {
                    dto.Condition(src => src.Source?.LocationBaseEntity?.AtualizationDate != null);
                    dto.MapFrom(src => DateTime.Now);
                })
                .ForPath(entity => entity.Pad.Location.ImportedT, dto => dto.MapFrom(src => src.LocationBaseEntity.ImportedT))
                .ForPath(entity => entity.Pad.Location.EntityStatus, dto => dto.MapFrom(src => src.LocationBaseEntity.Status))
                .ReverseMap();
        }
    }
}
