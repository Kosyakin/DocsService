using DocsService.Data;
using DocsService.Models;

using DocumentFormat.OpenXml.Packaging;

using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace DocsService.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FormController : Controller
    {

        private readonly AppDbContext _context;

        public FormController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("SubmitData")]
        public async Task<IActionResult> SubmitData([FromForm] FormData formData)
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

                byte[] fileBytes = await ProcessFormData(formData);
                string fileName = "форма 04-СТО 07-12 Лист регистрации инструктажа по охране труда.docx";
                
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", fileName);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private async Task<byte[]> ProcessFormData(FormData data)
        {

            var formId = data.FormId;
            var Employees = data.Employees;

            var employeesDB = await _context.Employees.Where(e => Employees.Contains(e.ID)).Select(e =>
            new Employees
            {
                ID = e.ID, 
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Position = e.Position,
                //FullName = $"{e.LastName} {e.FirstName} {e.MiddleName}".Trim(),
                BirthDate = e.BirthDate
            }).ToListAsync();

            if (!employeesDB.Any())
            {
                throw new InvalidOperationException("Сотрудники по указанным ID не найдены в базе данных");
            }

            string templatesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Controllers", "templates");

            string templatePath = "";
            if (formId == "dataForm")
            { templatePath = Path.Combine(templatesFolder, "форма 04-СТО 07-12 Лист регистрации инструктажа по охране труда.docx");} 
            if (formId == "dataForm1")
            { templatePath = Path.Combine(templatesFolder, "форма 05-СТО 07-12 Лист учета противопожарных инструктажей.docx"); }
            if (formId == "dataForm2")
            { templatePath = Path.Combine(templatesFolder, "форма 07-СТО 07-12 Лист регистрации инструктажа по действиям в ЧС.docx"); }

            //string outputPath = Path.Combine(templatesFolder, "форма 04-СТО 07-12 Лист регистрации инструктажа по охране труда заполненный.docx");

            if (!System.IO.File.Exists(templatePath))
            { throw new FileNotFoundException($"Файл шаблона не найден: {templatePath}"); }

            //System.IO.File.Copy(templatePath, outputPath, overwrite: true);
            byte[] templateBytes = await System.IO.File.ReadAllBytesAsync(templatePath);

            using var memoryStream = new MemoryStream();
            memoryStream.Write(templateBytes, 0, templateBytes.Length);

            using (var doc = WordprocessingDocument.Open(memoryStream, true))
            {
                var body = doc.MainDocumentPart.Document.Body;
                var table = body.Descendants<Table>().FirstOrDefault();

                if (table != null)
                {
                    var templateRow = table.Elements<TableRow>().ElementAt(3);
                    if (formId == "dataForm1") { templateRow = table.Elements<TableRow>().ElementAt(4); }
                    
                    
                    templateRow.Remove();

                     //await EmployeesFromDB(data);

                    foreach (var employee in employeesDB)
                    {
                        var newRow = (TableRow)templateRow.Clone();
                        


                        foreach (TableCell cell in newRow.Elements<TableCell>())
                        {
                            if (formId == "dataForm" || formId == "dataForm2") 
                            {ReplaceTextInOTandGOCS(cell, data, employee);}
                            if (formId == "dataForm1")
                            { ReplaceTextInPB(cell, data, employee); }

                        }

                        table.AppendChild(newRow);
                    }


                }


                doc.MainDocumentPart.Document.Save();
            }

            return memoryStream.ToArray();
        }

        private void ReplaceTextInOTandGOCS(TableCell cell, FormData formData, Employees employee)
        {

            string cellText = string.Concat(cell.Descendants<Text>().Select(t => t.Text));
            string name_user = "Иванов Иван Иванович";
            string post_user = "начальник отдела";


            if (cellText != null)
            {
                cellText = cellText
                    .Replace("{{DATE}}", formData.Date.ToString("dd.MM.yyyy"))
                    .Replace("{{INSTRUCTIONTYPE}}", formData.InstructionType)
                    .Replace("{{REASON}}", formData.Reason)
                    .Replace("{{NAME}}", name_user)
                    .Replace("{{USER_POST}}", post_user)
                    .Replace("{{LOCAL_ACT}}", formData.LocalAct)
                    .Replace("{{NAME_EMP}}", $"{employee.LastName} {employee.FirstName} {employee.MiddleName}")
                    .Replace("{{DATE_OF_BIRTH}}", employee.BirthDate.Date.ToString("dd.MM.yyyy"))
                    .Replace("{{POST}}", employee.Position)
                    .Replace("{{YEAR_BIRTH}}", employee.BirthDate.Year.ToString());

            }

            foreach (var textElement in cell.Descendants<Text>())
            {
                textElement.Text = "";
            }

            var firstTextElement = cell.Descendants<Text>().FirstOrDefault();
            if (firstTextElement != null)
            {
                firstTextElement.Text = cellText;
            }

        }

        private void ReplaceTextInPB(TableCell cell, FormData formData, Employees employee)
        {

            string cellText = string.Concat(cell.Descendants<Text>().Select(t => t.Text));
            string name_user = "Иванов Иван Иванович";
            string post_user = "начальник отдела";

            if (cellText != null)
                {
                    cellText = cellText
                        .Replace("{{DATE}}", formData.Date.ToString("dd.MM.yyyy"))
                        .Replace("{{IT}}", formData.InstructionType)
                        .Replace("{{NAME_EMP}}", $"{employee.LastName} {employee.FirstName} {employee.MiddleName}")
                        .Replace("{{POST}}", employee.Position)
                        .Replace("{{NAME}}", name_user)
                        .Replace("{{USER_POST}}", post_user)
                        .Replace("{{NUM_DOC}}", formData.NumDoc)
                        ;

                }

            foreach (var textElement in cell.Descendants<Text>())
            {
                textElement.Text = "";
            }

            var firstTextElement = cell.Descendants<Text>().FirstOrDefault();
            if (firstTextElement != null)
            {
                firstTextElement.Text = cellText;
            }
        }

        //private void ReplaceTextInGOCS(TableCell cell, FormData formData, Employees employee)
        //{

        //    Text textElement = cell.Descendants<Text>().FirstOrDefault();
        //    if (textElement != null)
        //    {
        //        textElement.Text = textElement.Text
        //            .Replace("{{DATE}}", formData.Date.ToString("dd.MM.yyyy"))
        //            .Replace("{{INSTRUCTIONTYPE}}", formData.InstructionType)
        //            .Replace("{{REASON}}", formData.Reason)
        //            .Replace("{{LOCAL_ACT}}", formData.LocalAct)
        //            .Replace("{{NAME_EMP}}", $"{employee.LastName} {employee.FirstName} {employee.MiddleName}")
        //            .Replace("{{DATE_OF_BIRTH}}", employee.BirthDate.Date.ToString("dd.MM.yyyy"))
        //            .Replace("{{POST}}", employee.Position);
        //    }

        //}


    }
}
