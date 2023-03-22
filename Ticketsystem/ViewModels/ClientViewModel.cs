using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Ticketsystem.Models.Database;

namespace Ticketsystem.ViewModels;

public class ClientViewModel
{
    public string Id { get; set; }

    [Required(ErrorMessage = "Pflichtfeld")]
    [DisplayName("Nachname")]
    public string LastName { get; set; }

    [DisplayName("Vorname")]
    public string FirstName { get; set; }

    [DisplayName("Email-Adresse")]
    [EmailAddress(ErrorMessage = "Keine gültige Email-Adresse")]
    public string Email { get; set; }

    [DisplayName("Straße und Hausnummer")]
    public string StreetAndHouseNumber { get; set; }

    [RegularExpression(@"^\d{5}$|^\d{5}-\d{4}$", ErrorMessage = "00000 - 99999")]
    [DisplayName("PLZ")]
    public string PostalCode { get; set; }

    [DisplayName("Wohnort")]
    public string City { get; set; }

    [DisplayName("Telefonnummer")]
    public string PhoneNumber { get; set; }

    [DisplayName("Teilnehmernummer")]
    public int? ParticipantNumber { get; set; }

    [DisplayName("Kurs")]
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
            ParticipantNumber = viewModel.ParticipantNumber,
            Course = viewModel.Course,
        };
    }

    public Client CopyForUpdate()
    {
        var client = new Client
        {
            Id = this.Id,
            LastName = this.LastName,
            FirstName = this.FirstName,
            Email = this.Email,
            PhoneNumber = this.PhoneNumber,
            StreetAndHouseNumber = this.StreetAndHouseNumber,
            PostalCode = this.PostalCode,
            City = this.City,
            ParticipantNumber = this.ParticipantNumber,
            Course = this.Course,
        };

        return client;
    }
}
