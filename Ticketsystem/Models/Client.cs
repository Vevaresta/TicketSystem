using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Ticketsystem.Models;

public class Client
{
    public Client()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string StreetAndHouseNumber { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }

    public int? ParticipantNumber { get; set; }
    public string Course { get; set; }

    public IList<Ticket> Tickets { get; set; }
}
