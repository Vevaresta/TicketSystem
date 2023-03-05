using System.ComponentModel.DataAnnotations;

namespace Ticketsystem.Models;

public class TicketType
{
    public TicketType()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key]
    public string Id { get; set; }

    public string Name { get; set; }
}