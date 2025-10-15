using DocsService.Interfaces;
using System.Data;

namespace DocsService.Repositories
{
    public class NotificationRepository: INotification
    {
        public DateTime? GetDateNotification(string typeInstr) 
        {
            DateTime? date = null;
            switch(typeInstr)
            {
                case "OT":
                    date = new DateTime(2025, 9, 1);
                    break;
                case "PB":
                    date = new DateTime(2025, 9, 1);
                    break;
            }
            
            return date; 
        }
    }
}
