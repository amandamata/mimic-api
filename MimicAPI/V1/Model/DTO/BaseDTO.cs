using System.Collections.Generic;

namespace MimicAPI.V1.Model.DTO
{
    public abstract class BaseDTO
    {
        public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
    }
}
