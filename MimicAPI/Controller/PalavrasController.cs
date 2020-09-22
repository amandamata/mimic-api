﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MimicAPI.Helper;
using MimicAPI.Model;
using MimicAPI.Model.DTO;
using MimicAPI.Repository.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MimicAPI.Controller
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly IPalavraRepository _repository;
        private readonly IMapper _mapper;
        public PalavrasController(IPalavraRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetAll")]
        public ActionResult GetAll([FromQuery] PalavraUrlQuery query, bool? status)
        {
            var item = _repository.GetAll(query, status);
            if (item.Resultados.Count == 0) return NotFound();
            if (item.Paginacao != null && query.NumeroPagina != null)
            {
                if (query.NumeroPagina > item.Paginacao.TotalPaginas) return NotFound();
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Paginacao));
            }

            var lista = _mapper.Map<ListaPaginacao<Palavra>, ListaPaginacao<PalavraDTO>>(item);
            foreach (var p in lista.Resultados)
            {
                p.Links = new List<LinkDTO>();
                p.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = p.Id }), "GET"));
            }

            lista.Links.Add(new LinkDTO("self", Url.Link("GetAll", query), "GET"));

            return Ok(lista);
        }

        [HttpGet("{id}", Name = "GetWord")]
        public ActionResult Get(int id)
        {
            var objeto = _repository.Get(id);
            if (objeto is null) return NotFound();

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(objeto);

            palavraDTO.Links = new List<LinkDTO>();

            palavraDTO.Links.Add(
                new LinkDTO("self", Url.Link("GetWord", new { id = palavraDTO.Id }), "GET")
            );
            palavraDTO.Links.Add(
                new LinkDTO("update", Url.Link("UpdateWord", new { id = palavraDTO.Id }),"PUT")
            );
            palavraDTO.Links.Add(
                new LinkDTO("delete", Url.Link("DeleteWord", new { id = palavraDTO.Id }), "DELETE")
            );
            return Ok(palavraDTO);
        }

        [Route("")]
        [HttpPost]
        public ActionResult Create([FromBody] Palavra palavra)
        {
            palavra.Criado = DateTime.Now;
            _repository.Create(palavra);
            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        [HttpPut("{id}", Name = "UpdateWord")]
        public ActionResult Update(int id, [FromBody] Palavra palavra)
        {
            var objeto = _repository.Get(id);
            if (objeto == null) return NotFound();
            palavra.Id = id;
            palavra.Atualizado = DateTime.Now;
            _repository.Update(palavra);
            return Ok(palavra);
        }

        [HttpDelete("{id}", Name = "DeleteWord")]
        public ActionResult Delete(int id)
        {
            var objeto = _repository.Get(id);
            if (objeto == null) return NotFound();
            _repository.Delete(id);
            return NoContent();
        }
    }
}
