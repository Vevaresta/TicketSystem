using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketsystem.ViewModels;

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
    public string Comments { get; set; }

    public Device Device { get; set; }

    public static implicit operator SoftwareViewModel(Software software)
    {
        return new SoftwareViewModel()
        {
            Name = software.Name,
            Comments = software.Comments,
        };
    }
}
