using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MimicAPI.Helper;
using MimicAPI.V1.Model;
using MimicAPI.V1.Model.DTO;
using MimicAPI.V1.Repository.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MimicAPI.V1.Controller
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    public class PalavrasController : ControllerBase
    {
        private readonly IPalavraRepository _repository;
        private readonly IMapper _mapper;
        public PalavrasController(IPalavraRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Pega do banco de dados todas as palavras existentes
        /// </summary>
        /// <param name="query">Filtros de pesquisa</param>
        /// <param name="status">Listagem de palavras</param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpGet("", Name = "GetAll")]
        public ActionResult GetAll([FromQuery] PalavraUrlQuery query, bool? status)
        {
            var item = _repository.GetAll(query, status);
            if (item.Resultados.Count == 0) return NotFound();

            ListaPaginacao<PalavraDTO> lista = CriarLinksListPalavraDTO(query, item);
            return Ok(lista);
        }
        
        /// <summary>
        /// Pega do banco de dados uma palavra escolhida
        /// </summary>
        /// <param name="id">Identificador da palavra</param>
        /// <returns>Objeto palavra</returns>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpGet("{id}", Name = "GetWord")]
        public ActionResult Get(int id)
        {
            var objeto = _repository.Get(id);
            if (objeto is null) return NotFound();

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(objeto);
            palavraDTO.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = palavraDTO.Id }), "GET"));
            palavraDTO.Links.Add(new LinkDTO("update", Url.Link("UpdateWord", new { id = palavraDTO.Id }),"PUT"));
            palavraDTO.Links.Add(new LinkDTO("delete", Url.Link("DeleteWord", new { id = palavraDTO.Id }), "DELETE"));
            return Ok(palavraDTO);
        }

        /// <summary>
        /// Cadastro da palavra no banco de dados
        /// </summary>
        /// <param name="palavra">Objeto palavra</param>
        /// <returns>Objeto com ID</returns>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpPost]
        public ActionResult Create([FromBody] Palavra palavra)
        {
            if (palavra == null) return BadRequest();
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            palavra.Ativo = true;
            palavra.Criado = DateTime.Now;
            _repository.Create(palavra);

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
            palavraDTO.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = palavraDTO.Id }), "GET"));
            
            return Created($"/api/palavras/{palavra.Id}", palavraDTO);
        }

        /// <summary>
        /// Atualiza a palavra no banco de dados
        /// </summary>
        /// <param name="id">Identificador da palavra</param>
        /// <param name="palavra">Objeto palavra com dados para alteração</param>
        /// <returns>Objeto palavra</returns>
        [MapToApiVersion("1.0")]
        [MapToApiVersion("1.1")]
        [HttpPut("{id}", Name = "UpdateWord")]
        public ActionResult Update(int id, [FromBody] Palavra palavra)
        {
            var objeto = _repository.Get(id);
            if (objeto == null) return NotFound();
            
            if (palavra == null) return BadRequest();
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            palavra.Id = id;
            palavra.Atualizado = DateTime.Now;
            palavra.Ativo = objeto.Ativo;
            palavra.Criado = objeto.Criado;

            _repository.Update(palavra);

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(palavra);
            palavraDTO.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = palavraDTO.Id }), "GET"));
            
            return Ok(palavraDTO);
        }

        /// <summary>
        /// Desativa uma palavra do sistema
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns></returns>
        [MapToApiVersion("1.1")]
        [HttpDelete("{id}", Name = "DeleteWord")]
        public ActionResult Delete(int id)
        {
            var objeto = _repository.Get(id);
            if (objeto == null) return NotFound();
            _repository.Delete(id);
            return NoContent();
        }
        private ListaPaginacao<PalavraDTO> CriarLinksListPalavraDTO(PalavraUrlQuery query, ListaPaginacao<Palavra> item)
        {
            var lista = _mapper.Map<ListaPaginacao<Palavra>, ListaPaginacao<PalavraDTO>>(item);

            foreach (var p in lista.Resultados)
            {
                p.Links = new List<LinkDTO>();
                p.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = p.Id }), "GET"));
            }

            lista.Links.Add(new LinkDTO("self", Url.Link("GetAll", query), "GET"));

            if (item.Paginacao != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Paginacao));
                if (query.NumeroPagina <= item.Paginacao.TotalPaginas)
                {
                    var queryString = new PalavraUrlQuery() { NumeroPagina = query.NumeroPagina + 1, QuantidadeRegistro = query.QuantidadeRegistro, Data = query.Data };
                    lista.Links.Add(new LinkDTO("next", Url.Link("GetAll", queryString), "GET"));
                }
                if (query.NumeroPagina - 1 > 0)
                {
                    var queryString = new PalavraUrlQuery() { NumeroPagina = query.NumeroPagina + 1, QuantidadeRegistro = query.QuantidadeRegistro, Data = query.Data };
                    lista.Links.Add(new LinkDTO("prev", Url.Link("GetAll", queryString), "GET"));
                }
            }
            return lista;
        }
    }
}
