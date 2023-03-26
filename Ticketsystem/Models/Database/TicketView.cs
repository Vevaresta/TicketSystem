namespace Ticketsystem.Models.Database
{
    public class TicketView
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public bool DataBackupByClient { get; set; }
        public bool DataBackupByStaff { get; set; }
        public bool DataBackupDone { get; set; }
        public bool DoBackup { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketTypeId { get; set; }
        public int WorkOrder { get; set; }
        public int ClientId1 { get; set; }
        public string City { get; set; }
        public string Course { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ParticipantNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string StreetAndHouseNumber { get; set; }
        public int TicketStatusId1 { get; set; }
        public int TicketTypeId1 { get; set; }
        public string TicketStatusName { get; set; }
        public string TicketTypeName { get; set; }
    }

}
