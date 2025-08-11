using DocsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

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
                    var errors = ModelState
        .Where(x => x.Value.Errors.Count > 0)
        .ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );

                    return BadRequest(new { Errors = errors });
                }

                ProcessFormData(formData);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private void ProcessFormData(FormData data)
        {

            var formId = data.FormId;
            var Employees = data.Employees;
            //var date = data.Date;
            //var instructionType = data.InstructionType;
            //var reason = data.Reason;
            //var localAct = data.LocalAct;

            string templatesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Controllers", "templates");
            string templatePath = Path.Combine(templatesFolder, "форма 04-СТО 07-12 Лист регистрации инструктажа по охране труда.docx");
            string outputPath = Path.Combine(templatesFolder, "форма 04-СТО 07-12 Лист регистрации инструктажа по охране труда заполненный.docx");

            if (!System.IO.File.Exists(templatePath))
            { throw new FileNotFoundException($"Файл шаблона не найден: {templatePath}"); }

            System.IO.File.Copy(templatePath, outputPath, overwrite: true);

            using (var doc = WordprocessingDocument.Open(outputPath, true))
            {
                var body = doc.MainDocumentPart.Document.Body;
                var table = body.Descendants<Table>().FirstOrDefault();

                if (table != null)
                {
                    var templateRow = table.Elements<TableRow>().ElementAt(3);
                    templateRow.Remove();

                    foreach(var employee in Employees)
                    {
                        var newRow = (TableRow)templateRow.Clone();

                        foreach (TableCell cell in newRow.Elements<TableCell>())
                        {
                            ReplaceTextInCell(cell, data, employee);
                            
                        }
                        
                        table.AppendChild(newRow);
                    }
                    

                }

                doc.MainDocumentPart.Document.Save();
            }

        }

        private void ReplaceTextInCell(TableCell cell, FormData formData, string employee)
        {
            Text textElement = cell.Descendants<Text>().FirstOrDefault();
            if (textElement != null)
            {
                textElement.Text = textElement.Text
                    .Replace("{{DATE}}", formData.Date.ToString("dd.MM.yyyy"))
                    .Replace("{{INSTRUCTIONTYPE}}", formData.InstructionType)
                    .Replace("{{REASON}}", formData.Reason)
                    .Replace("{{LOCAL_ACT}}", formData.LocalAct)
                    .Replace("{{NAME_EMP}}", employee);
            }
            
        }

        
    }
}
