using MimicAPI.Helper;
using MimicAPI.V1.Model;

namespace MimicAPI.V1.Repository.Interface
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
