using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helper;
using MimicAPI.Model;
using MimicAPI.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Repository
{
    public class PalavraRepository : IPalavraRepository
    {
        private readonly MimicContext _banco;

        public PalavraRepository(MimicContext banco)
        {
            _banco = banco;
        }

        public List<Palavra> GetAll(PalavraUrlQuery query)
        {
            var item = _banco.Palavras.AsNoTracking().AsQueryable();
            if (item != null)
            {
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
                }
                return item.ToList();
            }
            return new List<Palavra>();
        }

        public List<Palavra> GetAll(bool status)
        {
            return (List<Palavra>)_banco.Palavras.Where(p => p.Ativo == status);
        }

        public Palavra Get(int id)
        {
            return _banco.Palavras.AsNoTracking().FirstOrDefault(i => i.Id == id);
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
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
        }

        public void DeleteHard(int id, string nome)
        {
            var palavra = Get(id);
            if (palavra.Nome.Equals(palavra))
            {
                _banco.Palavras.Remove(palavra);
                _banco.SaveChanges();
            }
        }
    }
}
