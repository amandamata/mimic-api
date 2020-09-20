using MimicAPI.Helper;
using MimicAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repository.Interface
{
    public interface IPalavraRepository
    {
        ListaPaginacao<Palavra> GetAll(PalavraUrlQuery query, bool? status);
        Palavra Get(int id);
        void Create(Palavra palavra);
        void Update(Palavra palavra);
        void Delete(int id);
    }
}
