﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Models;

public class Device
{
    public Device()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }

    [ForeignKey(nameof(Ticket))]
    public int TicketId { get; set; }

    public string Name { get; set; }
    public string DeviceType { get; set; }
    public string Manufacturer { get; set; }
    public string SerialNumber { get; set; }
    public string Accessories { get; set; }
    public string Comments { get; set; }

    public Ticket Ticket { get; set; }

    public virtual IList<Software> Software { get; set; }

    public static implicit operator DeviceViewModel(Device device)
    {
        DeviceViewModel deviceViewModel = new()
        {
            Name = device.Name,
            DeviceType = device.DeviceType,
            Manufacturer = device.Manufacturer,
            SerialNumber = device.SerialNumber,
            Accessories = device.Accessories,
            Comments = device.Comments,
            Software = new List<SoftwareViewModel>()
        };

        foreach (Software software in device.Software)
        {
            deviceViewModel.Software.Add(software);
        }

        return deviceViewModel;
    }
}
