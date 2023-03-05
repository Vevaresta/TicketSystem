using System.ComponentModel.DataAnnotations;

namespace Ticketsystem.Models;
public class Software
{
    public Software()
    {
        Id = Guid.NewGuid().ToString();
    }

    [Key]
    public string Id { get; set; }

    public string Name { get; set; }
    public DateTime InstallationDateTime { get; set; }
    public string RegUserName { get; set; }
    public string RegPassword { get; set; }
    public string Comments { get; set; }
}
