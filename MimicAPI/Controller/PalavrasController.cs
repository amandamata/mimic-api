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
            return new JsonResult(_banco.Palavras);
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
        public ActionResult Create(Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            return Ok();
        }

        //api/palavras/id (PUT: id, nome, ativo, pontuacao, criacao)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id, Palavra palavra)
        {
            _banco.Palavras.Update(palavra);
            return Ok();
        }

        //api/palavras/id
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _banco.Palavras.Remove(_banco.Palavras.Find(id));
            return Ok();
        }
    }
}
