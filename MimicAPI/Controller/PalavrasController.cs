using Microsoft.AspNetCore.Mvc;
using MimicAPI.Helper;
using MimicAPI.Model;
using MimicAPI.Repository.Interface;
using Newtonsoft.Json;
using System;

namespace MimicAPI.Controller
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly IPalavraRepository _repository;
        public PalavrasController(IPalavraRepository repository)
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public ActionResult GetAll([FromQuery]PalavraUrlQuery query, bool? status)
        {
            var item = _repository.GetAll(query, status);
            if (query.NumeroPagina > item.Paginacao.TotalPaginas) return NotFound();
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Paginacao));
            return Ok(item);
        }
        
        [Route("{id}")]
        [HttpGet]
        public ActionResult Get(int id)
        {
            var objeto = _repository.Get(id);
            if (objeto is null) return NotFound();
            return Ok(objeto);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Create([FromBody]Palavra palavra)
        {
            palavra.Criado = DateTime.Now;
            _repository.Create(palavra);
            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id,[FromBody]Palavra palavra)
        {
            var objeto = _repository.Get(id);
            if (objeto == null) return NotFound();
            palavra.Id = id;
            palavra.Atualizado = DateTime.Now;
            _repository.Update(palavra);
            return Ok(palavra);
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var objeto = _repository.Get(id);
            if (objeto == null) return NotFound();
            _repository.Delete(id);
            return NoContent();
        }
    }
}
