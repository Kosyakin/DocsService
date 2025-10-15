namespace DocsService.Models
{
    public class User
    {
        public User(Guid id, string passwordHash, string email,
            string firstName, string lastName, string middleName, string position, string documentNember)
        {
            Id = id;
            //UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;    
            Position = position;
            DocumentNumber = documentNember;
        }

        public Guid Id { get; set; }

        //public string UserName { get; set; }
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

        public static User Create(Guid id, string userName, string passwordHash, string email,
            string firstName, string lastName, string middleName,
            string position, string documentNumber)
        {
            return new User(id, passwordHash, email, firstName, lastName, middleName,
                position, documentNumber);
        }
    }
}
