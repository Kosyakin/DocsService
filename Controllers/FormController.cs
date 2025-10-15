using DocsService.Data;
using DocsService.Interfaces;
using DocsService.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;

using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace DocsService.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class FormController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IUsersRepository _usersRepository;

        public FormController(AppDbContext context, IUsersRepository usersRepository)
        {
            _context = context;
            _usersRepository = usersRepository;
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
                //string fileName = "форма 04-СТО 07-12 Лист регистрации инструктажа по охране труда.docx";
                string fileName = "Заполненный шаблон";

                try
                {
                    // Находим пользователя по email
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == formData.Email);

                    if (user == null)
                    {
                        throw new Exception();
                    }

                    if (formData.FormId == "dataForm")
                    {
                        var today = DateTime.Now;
                        if (1 <= today.Month && today.Month <= 6)
                            user.OTmarch = true;
                        else
                            user.OTseptember = true;
                    }
                    else if (formData.FormId == "dataForm1")
                    {
                        user.PBseptember = true;
                    }


                        
                   

                   

                    // Сохраняем изменения
                    await _context.SaveChangesAsync();

                    
                }
                catch (Exception ex)
                {
                    throw new Exception();// Логирование ошибки

                    
                }


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
                BirthDate = e.BirthDate,
                Email_User = e.Email_User
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

            using (var doc = DocX.Load(templatePath))
            {
                var table = doc.Tables.FirstOrDefault();
                if (table == null)
                    throw new InvalidOperationException("Таблица не найдена в шаблоне.");


                // Определяем шаблонную строку (например, последняя строка или с меткой)
                Row templateRow = null;

                if (formId == "dataForm")
                {
                    templateRow = table.Rows[3]; // 4-я строка (индекс 3)
                    table.RemoveRow(3);
                }
                else if (formId == "dataForm2")
                {
                    templateRow = table.Rows[3];
                    table.RemoveRow(3);
                }
                else if (formId == "dataForm1")
                {
                    templateRow = table.Rows[4]; // 5-я строка
                    table.RemoveRow(4);
                }

                if (templateRow == null)
                    throw new InvalidOperationException("Шаблонная строка не найдена.");

                // Удаляем шаблонную строку (её будем клонировать)
                

                //await EmployeesFromDB(data);

                foreach (var employee in employeesDB)
                    {
                    var newRow = table.InsertRow(templateRow, table.Rows.Count);
                    var user = await (from emp in _context.Employees
                                      join u in _context.Users on emp.Email_User equals u.Email
                                      where u.Email == employee.Email_User
                                      select new
                                      {
                                          u.FirstName,
                                          u.LastName,
                                          u.MiddleName,
                                          u.Position,
                                          u.DocumentNumber
                                      })
                              .FirstOrDefaultAsync();

                    if (user == null)
                    {
                        throw new InvalidOperationException($"Пользователь с email '{employee.Email_User}' не найден.");
                    }

                    string post_user = user.Position;


                    // Заменяем текст в каждой ячейке новой строки
                    foreach (var cell in newRow.Cells)
                    {
                        // Полный текст ячейки — DocX позволяет легко получить и заменить
                        cell.ReplaceText("{{NAME_EMP}}", $"{employee.LastName} {employee.FirstName} {employee.MiddleName}");
                        cell.ReplaceText("{{DATE_OF_BIRTH}}", employee.BirthDate.ToString("dd.MM.yyyy"));
                        cell.ReplaceText("{{POST}}", employee.Position);

                        cell.ReplaceText("{{NAME}}", $"{user.LastName} {user.FirstName} {user.MiddleName}");
                        cell.ReplaceText("{{USER_POST}}", user.Position.ToLower());
                        cell.ReplaceText("{{NUM_DOC}}", user.DocumentNumber);

                        // Глобальные поля (можно повторить, если нужно)
                        cell.ReplaceText("{{DATE}}", data.Date.ToString("dd.MM.yyyy"));
                        cell.ReplaceText("{{INSTRUCTIONTYPE}}", data.InstructionType);
                        cell.ReplaceText("{{REASON}}", data.Reason);
                        cell.ReplaceText("{{LOCAL_ACT}}", data.LocalAct);
                        cell.ReplaceText("{{IT}}", data.InstructionType);
                        cell.ReplaceText("{{YEAR_BIRTH}}", employee.BirthDate.Year.ToString());
                        
                    }

                    //table.AppendChild(newRow);
                }

                // Сохраняем в память
                //using (var memoryStream = new MemoryStream())
                //{
                    doc.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                //}



                //doc.MainDocumentPart.Document.Save();
            }



            //return memoryStream.ToArray();
        }

        private async Task ReplaceTextInOT(TableCell cell, FormData formData, Employees employee)
        {
            // Получаем данные пользователя
            var user = await (from emp in _context.Employees
                              join u in _context.Users on emp.Email_User equals u.Email
                              where u.Email == employee.Email_User
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.MiddleName,
                                  u.Position
                              })
                              .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Пользователь с email '{employee.Email_User}' не найден.");
            }

            string post_user = user.Position;

            /// Собираем полный текст
    string fullText = string.Concat(cell.Descendants<Text>().Select(t => t.Text));

            // Выполняем замены
            string updatedText = fullText
                .Replace("{{DATE}}", formData.Date.ToString("dd.MM.yyyy"))
                .Replace("{{NAME_EMP}}", $"{employee.LastName} {employee.FirstName} {employee.MiddleName}")
                .Replace("{{DATE_OF_BIRTH}}", employee.BirthDate.ToString("dd.MM.yyyy"))
                .Replace("{{POST}}", employee.Position)
                .Replace("{{INSTRUCTIONTYPE}}", formData.InstructionType)
                .Replace("{{REASON}}", formData.Reason)
                .Replace("{{LOCAL_ACT}}", formData.LocalAct)
                .Replace("{{NAME}}", $"{user.LastName} {user.FirstName} {user.MiddleName}")
                .Replace("{{USER_POST}}", user.Position);

            // Удаляем все старые Text
            var textElements = cell.Descendants<Text>().ToList();
            foreach (var text in textElements)
            {
                text.Text = "";
            }

            // Пишем результат в первый Text
            if (textElements.Any())
            {
                textElements[0].Text = updatedText;
            }
        }

        private async void ReplaceTextInPB(TableCell cell, FormData formData, Employees employee)
        {

            string cellText = string.Concat(cell.Descendants<Text>().Select(t => t.Text));
            var user = await(from emp in _context.Employees
                             join u in _context.Users on emp.Email_User equals u.Email
                             select new
                             {
                                 u.FirstName,
                                 u.LastName,
                                 u.MiddleName,
                                 u.Position, 
                                 u.DocumentNumber
                             })
                              .FirstOrDefaultAsync();
            if (user == null) { throw new InvalidOperationException($"Пользователь с email '{employee.Email_User}' не найден в таблице Users."); }
            ;
            string post_user = user.Position;

            if (cellText != null)
                {
                    cellText = cellText
                        .Replace("{{DATE}}", formData.Date.ToString("dd.MM.yyyy"))
                        .Replace("{{IT}}", formData.InstructionType)
                        .Replace("{{NAME_EMP}}", $"{employee.LastName} {employee.FirstName} {employee.MiddleName}")
                        .Replace("{{POST}}", employee.Position)
                        .Replace("{{NAME}}", $"{user.LastName} {user.FirstName} {user.MiddleName}")
                        .Replace("{{USER_POST}}", post_user)
                        .Replace("{{NUM_DOC}}", user.DocumentNumber)
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

        private async Task ReplaceTextInGOCHS(TableCell cell, FormData formData, Employees employee)
        {

            string cellText = string.Concat(cell.Descendants<Text>().Select(t => t.Text));
            var user = await (from emp in _context.Employees
                              join u in _context.Users on emp.Email_User equals u.Email
                              where u.Email == employee.Email_User
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.MiddleName,
                                  u.Position
                              })
                              .FirstOrDefaultAsync();
            if (user == null) { throw new InvalidOperationException($"Пользователь с email '{employee.Email_User}' не найден в таблице Users."); }
            ;
            string post_user = user.Position;





            if (cellText != null)
            {
                cellText = cellText
                    .Replace("{{DATE}}", formData.Date.ToString("dd.MM.yyyy"))
                    .Replace("{{NAME_EMP}}", $"{employee.LastName} {employee.FirstName} {employee.MiddleName}")
                    .Replace("{{YEAR_BIRTH}}", employee.BirthDate.Year.ToString())
                    .Replace("{{POST}}", employee.Position)
                    .Replace("{{NAME}}", $"{user.LastName} {user.FirstName} {user.MiddleName}")
                    .Replace("{{USER_POST}}", post_user.ToLower());
                    
                    
                    
                    
                    

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


    }
}
