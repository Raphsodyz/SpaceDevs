using Application.DTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class FutureSpaceMapper : Profile
    {
        public FutureSpaceMapper()
        {
            CreateMap<LaunchDTO, Launch>()
                .ForMember(
                    entity => entity.ApiGuId,
                    dto => dto.MapFrom(src => src.Id)).ReverseMap();
        }
    }
}
