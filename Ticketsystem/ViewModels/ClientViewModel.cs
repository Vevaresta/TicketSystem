using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Ticketsystem.Models;

namespace Ticketsystem.ViewModels;

public class ClientViewModel
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string StreetAndHouseNumber { get; set; }
    public int? PostalCode { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }

    public int? RehaNumber { get; set; }
    public string Course { get; set; }

    public static implicit operator Client(ClientViewModel viewModel)
    {
        return new Client
        {
            LastName = viewModel.LastName,
            FirstName = viewModel.FirstName,
            Email = viewModel.Email,
            PhoneNumber = viewModel.PhoneNumber,
            StreetAndHouseNumber = viewModel.StreetAndHouseNumber,
            PostalCode = viewModel.PostalCode,
            City = viewModel.City,
            RehaNumber = viewModel.RehaNumber,
            Course = viewModel.Course,
        };
    }
}
