using AutoMapper;
using MimicAPI.Model;
using MimicAPI.Model.DTO;

namespace MimicAPI.Helper
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<Palavra, PalavraDTO>();
        }
    }
}
