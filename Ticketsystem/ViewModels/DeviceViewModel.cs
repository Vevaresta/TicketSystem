using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketsystem.Models;

namespace Ticketsystem.ViewModels;

public class DeviceViewModel
{
    [Required]
    public string Name { get; set; }
    public string DeviceType { get; set; }
    public string Manufacturer { get; set; }
    public string SerialNumber { get; set; }
    public string Accessories { get; set; }
    public string Comments { get; set; }
    public virtual IList<SoftwareViewModel> Software { get; set; }

    public static implicit operator Device(DeviceViewModel viewModel)
    {
        var device = new Device
        {
            Name = viewModel.Name,
            DeviceType = viewModel.DeviceType,
            Manufacturer = viewModel.Manufacturer,
            SerialNumber = viewModel.SerialNumber,
            Accessories = viewModel.Accessories,
            Comments = viewModel.Comments,
            Software = new List<Software>()
        };

        if (viewModel.Software != null)
        {
            foreach (var softwareViewModel in viewModel.Software)
            {
                device.Software.Add(softwareViewModel);
            }
        }

        return device;
    }
}
