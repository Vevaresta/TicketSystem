using System.ComponentModel.DataAnnotations;

namespace Ticketsystem.Models;

public class Client
{
    public Client()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key]
    public string Id { get; set; }

    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string StreetAndHouseNumber { get; set; }
    public int PLZ { get; set; }
    public string City { get; set; }
    public string PhoneNumber1 { get; set; }
    public string PhoneNumber2 { get; set; }
    public int RehaNumber { get; set; }
    public string Course { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
}
