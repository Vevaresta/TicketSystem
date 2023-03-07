﻿using System.ComponentModel.DataAnnotations;

namespace Ticketsystem.Models;

public class Client
{
    public Client()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }

    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string StreetAndHouseNumber { get; set; }
    public int PostalCode { get; set; }
    public string City { get; set; }
    public string PhoneNumber { get; set; }
    public int RehaNumber { get; set; }
    public string Course { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
}
