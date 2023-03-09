using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketsystem.Models;

namespace Ticketsystem.ViewModels;
public class SoftwareViewModel
{
    public string Name { get; set; }
    public string Comments { get; set; }

    public static implicit operator Software(SoftwareViewModel viewModel)
    {
        return new Software
        {
            Name = viewModel.Name,
            Comments = viewModel.Comments,
        };
    }
}
