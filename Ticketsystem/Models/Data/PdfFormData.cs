using Ticketsystem.Enums;

namespace Ticketsystem.Models.Data
{
    public class PdfFormData
    {
        // Client Info

        public string TicketId { get; set; }
        
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string ParticipantNumber { get; set; }
        public string Course { get; set; }
        public string StreetAndHouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        // Device Info
        public string TicketType { get; set; }
        public bool VirusQuarantine { get; set; }

        public string DeviceType { get; set; }
        public string SerialNumber { get; set; }
        public string Accessories { get; set; }
        public string DeviceProducer { get; set; }
        public string DeviceDescription { get; set; }

        // Tasks

        public string TicketName { get; set; }
        public string WorkDescription { get; set; }

        // Security
        public bool BackupByClient{ get; set; }
        public bool BackupByStaff { get; set; }
        public string ClientSignature { get; set; }

        public string OrderEnd  { get; set; }
        public string SignatureOfAcceptance { get; set; }
    }
}
