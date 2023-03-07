using System.ComponentModel.DataAnnotations;

namespace Ticketsystem.Models;

public class TicketType
{
    public TicketType()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }

    public string Name { get; set; }
}