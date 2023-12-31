using AutoMapper;
using Domain.Entities;
using Cross.Cutting.Helper;
using Data.Materializated.Views;
using Business.DTO.Entities;

namespace Business.Mapper
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
                    entity => entity.ApiGuId,
                    dto => dto.MapFrom(src => src.Id))
                .ForMember(
                    entity => entity.Id,
                    dto => dto.MapFrom(src => src.Id))
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
        }
    }
}
