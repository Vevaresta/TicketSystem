namespace Ticketsystem.Models.Data
{
    public class ClientFilterData : IFilterData
    {
        public int Take { get; set; } = 10;
        public int Skip { get; set; } = 0;
        public string SortBy { get; set; } = "LastName";
        public bool DoReverse { get; set; } = false;
        public string FilterByLastName { get; set; }
        public string FilterByFirstName { get; set; }
        public string FilterByEmail { get; set; }
    }
}