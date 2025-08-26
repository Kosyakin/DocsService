namespace DocsService.Models
{
    public class FormData
    {
        public DateTime Date { get; set; }

        public List<int> Employees { get; set; } = new List<int>();

        public string InstructionType { get; set; }

        public string Reason { get; set; }

        public string LocalAct { get; set; }

        public string FormId { get; set; }

        //public string NumDoc { get; set; }
    }
}
