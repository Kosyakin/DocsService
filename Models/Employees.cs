using System;
using System.ComponentModel.DataAnnotations;

namespace DocsService.Models
{
    public class Employees
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }

        public string Position { get; set; }

        //Реализация напоминаний
        //public Guid Id_User { get; set; }
        public string Email_User {  get; set; }

        public DateTime LastDatePB { get; set; }
        public DateTime NextDatePB { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}
