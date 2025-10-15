using DocumentFormat.OpenXml.Spreadsheet;

namespace DocsService.Models
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        
        public string PasswordHash { get; set; }
        public string Email { get; set; }

        //Дополнительная информация о пользователе
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; } = string.Empty;

        // Служебная информация
        public string Position { get; set; }        // Должность
        public string DocumentNumber { get; set; }  // Номер документа

        // Даты
        public DateTime? ReminderDateOTseptember { get; set; }
        public DateTime? ReminderDateOTmarch { get; set; }
        public DateTime? ReminderDatePBseptember { get; set; }

        //Напоминания
        public bool OTseptember { get; set; } = false;
        public bool OTmarch { get; set; } = false;
        public bool PBseptember { get; set; } = false;
    }
}
