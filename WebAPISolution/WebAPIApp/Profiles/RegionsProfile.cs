using AutoMapper;

namespace WebAPIApp.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            //create map will map the source(Domain Model) to the destination (DTO Model) 
           
            CreateMap<Models.Domain.Region, Models.DTO.Region>();
/*
            //Note if the properties names inside the source and destination then we need to define a map for it 
            CreateMap<Models.Domain.Region, Models.DTO.Region>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.RegId));*/
        }
    }
}
