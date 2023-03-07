using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketsystem.Models;
public class Software
{
    public Software()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }

    [ForeignKey(nameof(DeviceId))]
    public string DeviceId { get; set; }

    public string Name { get; set; }
    public DateTime InstallationDateTime { get; set; }
    public string RegUserName { get; set; }
    public string RegPassword { get; set; }
    public string Comments { get; set; }

    public Device Device { get; set; }
}
