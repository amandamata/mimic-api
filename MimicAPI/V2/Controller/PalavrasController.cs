using Microsoft.AspNetCore.Mvc;

namespace MimicAPI.V2.Controller
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class PalavrasController : ControllerBase
    {
        [HttpGet("", Name = "GetAll")]
        public string GetAll()
        {
            return "Versão 2.0";
        }
    }
}
