using Ticketsystem.Enums;

namespace Ticketsystem.Utilities
{
    public class EnumUtility
    {
        public static TicketStatuses GetTicketStatusByName(string name)
        {
            return Enum.GetValues(typeof(TicketStatuses)).Cast<TicketStatuses>().FirstOrDefault(e => e.ToString() == name);
        }
    }
}
