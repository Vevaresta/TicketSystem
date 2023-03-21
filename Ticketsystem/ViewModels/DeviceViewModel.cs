using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketsystem.Models.Database;

namespace Ticketsystem.ViewModels;

public class DeviceViewModel
{
    public string Id { get; set; }

    [DisplayName("Bezeichnung")]
    public string Name { get; set; }

    [DisplayName("Geräteart")]
    public string DeviceType { get; set; }

    [DisplayName("Hersteller")]
    public string Manufacturer { get; set; }

    [DisplayName("Seriennummer")]
    public string SerialNumber { get; set; }

    [DisplayName("Zubehör")]
    public string Accessories { get; set; }

    [DisplayName("Notizen")]
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

    public Device CopyForUpdate()
    {
        var device = new Device
        {
            Id = this.Id,
            Name = this.Name,
            DeviceType = this.DeviceType,
            Manufacturer = this.Manufacturer,
            SerialNumber = this.SerialNumber,
            Accessories = this.Accessories,
            Comments = this.Comments,
            Software = new List<Software>()
        };

        if (this.Software != null)
        {
            foreach (var softwareViewModel in this.Software)
            {
                Software software = softwareViewModel.CopyForUpdate();
                device.Software.Add(software);
            }
        }

        return device;
    }
}
