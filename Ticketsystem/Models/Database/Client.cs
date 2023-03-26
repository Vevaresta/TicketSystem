using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Models.Database;

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

    [JsonIgnore]
    public virtual IList<Ticket> Tickets { get; set; }

    public static implicit operator ClientViewModel(Client client)
    {
        return new ClientViewModel()
        {
            Id = client.Id,
            LastName = client.LastName,
            FirstName = client.FirstName,
            Email = client.Email,
            StreetAndHouseNumber = client.StreetAndHouseNumber,
            PostalCode = client.PostalCode,
            City = client.City,
            PhoneNumber = client.PhoneNumber,
            ParticipantNumber = client.ParticipantNumber,
            Course = client.Course
        };
    }
}
