using Microsoft.AspNetCore.Mvc;

namespace MimicAPI.V2.Controller
{
    /// <summary>
    /// Pega do banco de dados todas as palavras existentes
    /// </summary>
    /// <returns></returns>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class PalavrasController : ControllerBase
    {
        /// <summary>
        /// Pega do banco de dados todas as palavras existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet("", Name = "GetAll")]
        public string GetAll()
        {
            return "Versão 2.0";
        }
    }
}
