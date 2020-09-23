using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helper;
using MimicAPI.V1.Model;
using MimicAPI.V1.Repository.Interface;
using System;
using System.Linq;

namespace MimicAPI.V1.Repository
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly MimicContext _banco;

        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }

        public ListaPaginacao<Palavra> GetAll(PalavraUrlQuery query, bool? status)
        {
            var lista = new ListaPaginacao<Palavra>();
            var item = _banco.Palavras.AsNoTracking().AsQueryable();
            
            if (item != null)
            {
                item = (status != null) ? item.Where(s => s.Ativo == status) : item.Where(s => s.Ativo == true);

                if (query.Data.HasValue)
                {
                    item = item.Where(a => a.Criado > query.Data.Value || a.Atualizado > query.Data.Value);
                }
                if (query.NumeroPagina.HasValue && query.QuantidadeRegistro.HasValue)
                {
                    var quantidadeTotalRegistros = item.Count();
                    var paginacao = new Paginacao();

                    item = item.Skip((query.NumeroPagina.Value - 1) * query.QuantidadeRegistro.Value).Take(query.QuantidadeRegistro.Value);

                    paginacao.NumeroPagina = query.NumeroPagina.Value;
                    paginacao.RegistroPorPagina = query.QuantidadeRegistro.Value;
                    paginacao.TotalRegistros = quantidadeTotalRegistros;
                    paginacao.TotalPaginas = (int)Math.Ceiling((double)(paginacao.TotalRegistros / paginacao.RegistroPorPagina));
                    lista.Paginacao = paginacao;
                }
                lista.Resultados.AddRange(item.ToList());
                return lista;
            }
            return new ListaPaginacao<Palavra>();
        }

        public Palavra Get(int id)
        {
            return _banco.Palavras.AsNoTracking().FirstOrDefault(i => i.Id == id && i.Ativo == true);
        }

        public void Create(Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
        }

        public void Update(Palavra palavra)
        {
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
        }

        public void Delete(int id)
        {
            var palavra = Get(id);
            palavra.Ativo = false;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
        }
    }
}
