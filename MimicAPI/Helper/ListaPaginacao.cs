using MimicAPI.Model.DTO;
using System.Collections.Generic;

namespace MimicAPI.Helper
{
    public class ListaPaginacao<T>
    {
        public List<T> Resultados { get; set; } = new List<T>();
        public Paginacao Paginacao { get; set; }
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
