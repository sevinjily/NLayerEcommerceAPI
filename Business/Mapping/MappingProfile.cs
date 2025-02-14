using AutoMapper;
using Entities.Concrete;
using Entities.DTOs.BrandDTOs;

namespace Business.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<AddBrandDTO,Brand>().ReverseMap();
            CreateMap<Brand,GetBrandDTO>().ReverseMap();
            CreateMap<UpdateBrandDTO, Brand>().ReverseMap();
            //ReverseMap ozu avto teyin edir birinci DTO mu yazmaq lazimdir yoxsa Brand mi
        }
    }
}
