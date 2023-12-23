using Application.DTO;
using AutoMapper;
using Cross.Cutting.Helper;
using Services.ViewModel;

namespace Services.Mapper
{
    public class FutureSpaceViewModelMapper : Profile
    {
        public FutureSpaceViewModelMapper()
        {
            CreateMap<StatusDTO, StatusViewModel>().ReverseMap();
            CreateMap<RocketDTO, RocketViewModel>().ReverseMap();
            CreateMap<ConfigurationDTO, ConfigurationViewModel>().ReverseMap();
            CreateMap<LaunchServiceProviderDTO, LaunchServiceProviderViewModel>().ReverseMap();
            CreateMap<LocationDTO, LocationViewModel>().ReverseMap();
            CreateMap<MissionDTO, MissionViewModel>().ReverseMap();
            CreateMap<OrbitDTO, OrbitViewModel>().ReverseMap();
            CreateMap<PadDTO, PadViewModel>().ReverseMap();
            CreateMap<Pagination<LaunchDTO>, Pagination<LaunchViewModel>>().ReverseMap();
            CreateMap<LaunchDTO, LaunchViewModel>().ReverseMap();
        }
    }
}