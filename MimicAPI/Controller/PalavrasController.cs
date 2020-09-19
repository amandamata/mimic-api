using Microsoft.AspNetCore.Mvc;
using MimicAPI.Database;
using MimicAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controller
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {
        private readonly MimicContext _banco;
        public PalavrasController(MimicContext banco)
        {
            _banco = banco;
        }

        //APP /api/palavras
        [Route("")]
        [HttpGet]
        public ActionResult GetAll()
        {
            return new JsonResult(_banco.Palavras.Where(p => p.Ativo != false));
        }

        //WEB /api/palavras/id
        [Route("{id}")]
        [HttpGet]
        public ActionResult Get(int id)
        {
            return Ok(_banco.Palavras.Find(id));
        }

        //api/palavras(POST: id, nome, ativo, pontuacao, criacao)
        [Route("")]
        [HttpPost]
        public ActionResult Create([FromBody]Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
            return Ok();
        }

        //api/palavras/id (PUT: id, nome, ativo, pontuacao, criacao)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id,[FromBody]Palavra palavra)
        {
            palavra.Id = id;
            palavra.Atualizado = DateTime.Now;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
            return Ok();
        }

        //api/palavras/id
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var palavra = _banco.Palavras.Find(id);
            palavra.Ativo = false;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
            return Ok();
        }
    }
}
