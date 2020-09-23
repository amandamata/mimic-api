using AutoMapper;
using MimicAPI.V1.Model;
using MimicAPI.V1.Model.DTO;

namespace MimicAPI.Helper
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<Palavra, PalavraDTO>();
            CreateMap<ListaPaginacao<Palavra>,ListaPaginacao<PalavraDTO>>();
        }
    }
}
