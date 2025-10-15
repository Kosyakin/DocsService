namespace DocsService.Contracts
{
    public class UserInfoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
        public string DocumentNumber { get; set; }
        public string Email { get; set; }

        public DateTime? ReminderDateOTseptember { get; set; }
        public DateTime? ReminderDateOTmarch { get; set; }
        public DateTime? ReminderDatePBseptember { get; set; }
    }
}
