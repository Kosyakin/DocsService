using DocumentFormat.OpenXml.Spreadsheet;

namespace DocsService.Models
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        //Дополнительная информация о пользователе
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        // Служебная информация
        public string Position { get; set; }        // Должность
        public string DocumentNumber { get; set; }  // Номер документа

        // Даты
        //public DateTime BirthDate { get; set; }
    }
}
