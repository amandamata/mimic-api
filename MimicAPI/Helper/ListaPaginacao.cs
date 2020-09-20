using System.Collections.Generic;

namespace MimicAPI.Helper
{
    public class ListaPaginacao<T> : List<T>
    {
        public Paginacao Paginacao { get; set; }
    }
}
