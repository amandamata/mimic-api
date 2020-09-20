using MimicAPI.Helper;
using MimicAPI.Model;

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
