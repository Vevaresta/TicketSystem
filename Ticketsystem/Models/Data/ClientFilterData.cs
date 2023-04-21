namespace Ticketsystem.Models.Data
{
    public class ClientFilterData : IFilterData
    {
        public int Take { get; set; } = 10;
        public int Skip { get; set; } = 0;
        public string SortBy { get; set; } = "Id";
        public bool DoReverse { get; set; } = false;
        public int? FilterById { get; set; } = null;
        public string FilterByLastName { get; set; }
        public string FilterByEmail { get; set; }
    }
}