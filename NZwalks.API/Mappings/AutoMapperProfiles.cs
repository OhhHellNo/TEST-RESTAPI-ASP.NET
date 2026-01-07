using AutoMapper;
using NZwalks.API.Models.Domains;
using NZwalks.API.Models.DTOs;

namespace NZwalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Region, UpdateRegionDto>().ReverseMap();
            CreateMap<AddRegionDto ,Region>().ReverseMap();

        }
    }
}
