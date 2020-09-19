using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [Route("")]
        [HttpGet]
        public ActionResult GetAll()
        {
            return new JsonResult(_banco.Palavras.Where(p => p.Ativo != false));
        }

        [Route("{status}")]
        [HttpGet]
        public ActionResult GetAll(bool status)
        {
            return new JsonResult(_banco.Palavras.Where(p => p.Ativo == status));
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Get(int id)
        {
            var obj = _banco.Palavras.Find(id);
            if (obj == null) return NotFound();

            return Ok();
        }

        [Route("")]
        [HttpPost]
        public ActionResult Create([FromBody]Palavra palavra)
        {
            palavra.Criado = DateTime.Now;
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();

            return Created($"/api/palavras/{palavra.Id}", palavra);
        }

        [Route("{id}")]
        [HttpPut]
        public ActionResult Update(int id,[FromBody]Palavra palavra)
        {
            var obj = _banco.Palavras.AsNoTracking().FirstOrDefault(i => i.Id == id);
            if (obj == null) return NotFound();

            palavra.Id = id;
            palavra.Atualizado = DateTime.Now;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
            
            return NoContent();
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var palavra = _banco.Palavras.Find(id);
            if (palavra == null) return NotFound();

            palavra.Ativo = false;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();

            return NoContent();
        }

        [Route("{id}/{nome}")]
        [HttpDelete]
        public ActionResult DeleteHard(int id, string nome)
        {
            var palavra = _banco.Palavras.Find(id);

            if (palavra == null) return NotFound();
            
            if (palavra.Nome == nome)
            {
                _banco.Palavras.Remove(palavra);
                _banco.SaveChanges();
                return NoContent();
            }
            else return NotFound();
        }
    }
}
