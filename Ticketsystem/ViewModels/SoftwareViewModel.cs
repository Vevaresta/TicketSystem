using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketsystem.Models.Database;

namespace Ticketsystem.ViewModels;
public class SoftwareViewModel
{
    public string Id { get; set; }

    [Required(ErrorMessage = "Pflichtfeld")]
    [DisplayName("Bezeichnung")]
    public string Name { get; set; }

    [DisplayName("Notizen")]
    public string Comments { get; set; }

    public static implicit operator Software(SoftwareViewModel viewModel)
    {
        return new Software
        {
            Name = viewModel.Name,
            Comments = viewModel.Comments,
        };
    }

    public Software CopyForUpdate()
    {
        var software = new Software
        {
            Id = Id,
            Name = Name,
            Comments = Comments,
        };

        return software;
    }
}
