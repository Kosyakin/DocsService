using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocsService.Controllers
{

    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult GetHello()
        {
            return Ok(new { message = "Привет от бэкенда!" });
        }
    }

}
