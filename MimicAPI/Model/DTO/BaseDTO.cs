using System.Collections.Generic;

namespace MimicAPI.Model.DTO
{
    public abstract class BaseDTO
    {
        public List<LinkDTO> Links { get; set; }
    }
}
