using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaDTO, VillaCreateDTO>();
            CreateMap<VillaDTO, VillaUpdateDTO>();

            CreateMap<VillaNumberDTO, VillaNumberCreateDTO>().ReverseMap(); //eita escribir 2 veces como arriba
            CreateMap<VillaNumberDTO, VillaNumberUpdateDTO>().ReverseMap();
        }
    }
}
