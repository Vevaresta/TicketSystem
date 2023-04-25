using Ticketsystem.Enums;

namespace Ticketsystem.Models.Data
{
    public class PdfFormData
    {
        public string TicketId { get; set; }
        public string TicketType { get; set; }
        public string WorkOrder { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string DeviceType { get; set; }
        public string DeviceSerialNumber { get; set; }
        public string DeviceAccessories { get; set; }
        public bool BackupByClient { get; set; }
        public bool BackupByStaff { get; set; }
        public string OrderEndData { get; set; }
    }
}
