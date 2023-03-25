using System.ComponentModel.DataAnnotations;
using Ticketsystem.Enums;
using Ticketsystem.Extensions;

namespace Ticketsystem.Models.Database;

public class TicketStatus
{
    public TicketStatus()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }

    public string Name { get; set; }

    public TicketStatuses AsEnum
    {
        get
        {
            return Enum.GetValues<TicketStatuses>().FirstOrDefault(ts => ts.ToString() == Name);
        }
    }
}
