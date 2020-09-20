using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Helper
{
    public class ListaPaginacao<T> : List<T>
    {
        public Paginacao Paginacao { get; set; }
    }
}
