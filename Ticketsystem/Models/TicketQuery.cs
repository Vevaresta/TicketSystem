namespace Ticketsystem.Models
{
    public class TicketQuery
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public string SortByAttribute { get; set; }
        public bool DoReverse { get; set; }
        public string FilterByTicketId { get; set; }
        public string FilterByTicketName { get; set; }
        public string FilterByTicketStatus { get; set; }
        public string FilterByClientName { get; set; }
        public string FilterByStartDate { get; set; }
        public string FilterByEndDate { get; set; }
        public string FilterByTicketType { get; set; }
    }
}
