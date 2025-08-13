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

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}
