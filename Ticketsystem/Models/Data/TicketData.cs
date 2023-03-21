namespace Ticketsystem.Models.Data
{
    public class TicketData
    {
        public int Take { get; set; } = 10;
        public int Skip { get; set; } = 0;
        public string SortBy { get; set; } = "OrderDate";
        public bool DoReverse { get; set; } = true;
        public string FilterByTicketId { get; set; }
        public string FilterByTicketName { get; set; }
        public string FilterByTicketStatus { get; set; }
        public string FilterByClientName { get; set; }
        public string FilterByStartDate { get; set; }
        public string FilterByEndDate { get; set; }
        public string FilterByTicketType { get; set; }
    }
}
