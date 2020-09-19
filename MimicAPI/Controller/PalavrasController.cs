using Microsoft.AspNetCore.Mvc;
using MimicAPI.Database;
using MimicAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controller
{
    public class PalavrasController : ControllerBase
    {
        private readonly MimicContext _banco;
        public PalavrasController(MimicContext banco)
        {
            _banco = banco;
        }
        public ActionResult GetAll()
        {
            return new JsonResult(_banco.Palavras);
        }
        public ActionResult Get(int id)
        {
            return Ok(_banco.Palavras.Find(id));
        }
        public ActionResult Create(Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            return Ok();
        }
        public ActionResult Update(int id, Palavra palavra)
        {
            _banco.Palavras.Update(palavra);
            return Ok();
        }
        public ActionResult Delete(int id)
        {
            _banco.Palavras.Remove(_banco.Palavras.Find(id));
            return Ok();
        }
    }
}
