namespace DocsService.Models
{
    public class FormData
    {
        public DateTime Date { get; set; }

        public List<string> Employees { get; set; } = new List<string>();

        public string InstructionType { get; set; }

        public string Reason { get; set; }

        public string LocalAct { get; set; }

        public int FormId { get; set; }

        public int NumDoc { get; set; }
    }
}
