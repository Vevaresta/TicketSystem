using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;

namespace Ticketsystem.ViewModels
{
    public class TicketIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public string ClientLastName { get; set; }
        public string TicketType { get; set; }
        public string TicketStatus { get; set; }
    }
}
