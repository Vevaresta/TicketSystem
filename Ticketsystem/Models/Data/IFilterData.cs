namespace Ticketsystem.Models.Data
{
    public interface IFilterData
    {
        int Take { get; set; }
        int Skip { get; set; }
        string SortBy { get; set; }
        bool DoReverse { get; set; }
    }
}
