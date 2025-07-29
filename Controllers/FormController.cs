using DocsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocsService.Controllers
{
    [Route("[controller]")]
    public class FormController : Controller
    {
        [HttpPost("SubmitData")]
        public IActionResult SubmitData([FromForm] FormData formData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("IncorrectData " + ModelState);
                }

                //var result = ProcessFormData(formData);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //    private (byte[] FileBytes) ProcessFormData(FormData data)
        //{
        //    // Здесь ваша логика формирования документа
        //    // Например, с использованием OpenXML или других библиотек

        //    byte[] fileContent = GenerateDocument(data);
        //    return (fileContent);
        //}
    }
}
