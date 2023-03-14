﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Ticketsystem.Enums;
using Ticketsystem.Models;
using Ticketsystem.Services;

namespace Ticketsystem.Data
{
    public class ContextSeed
    {
        private readonly TicketsystemContext _ticketSystemContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        private readonly IServiceFactory _serviceFactory;

        public ContextSeed(
            TicketsystemContext ticketsystemContext,
            RoleManager<Role> roleManager,
            UserManager<User> userManager,
            IServiceFactory serviceFactory
            )
        {
            _ticketSystemContext = ticketsystemContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _serviceFactory = serviceFactory;
        }

        public async Task Seed(bool doSeedTickets = false)
        {
            await SeedUserRoles();
            await SeedDefaultAdmin();
            await SeedPermissions();
            await SeedRolePermissions();
            await SeedTicketStatuses();
            await SeedTicketTypes();

            if (doSeedTickets)
            {
                if (await _ticketSystemContext.Tickets.FirstOrDefaultAsync(x => x.WorkOrder.Contains("WorkOrder_")) == null)
                {
                    await SeedTestTickets();
                }
            }
        }

        public async Task SeedTestTickets()
        {
            Random rnd = new Random();
            Ticket[] tickets = new Ticket[251];

            for (int i = 1; i < tickets.Length + 1; i++)
            {
                var randomStatusIndex = rnd.Next(0, Enum.GetNames<TicketStatuses>().Length);
                var randomTypeIndex = rnd.Next(0, Enum.GetNames<TicketTypes>().Length);
                var randomTimeDayOffset = rnd.Next(0, 265);
                var randomTimeHourOffset = rnd.Next(0, 24);
                var randomMinuteOffset = rnd.Next(0, 60);
                var randomSecondOffset = rnd.Next(0, 60);

                tickets[i - 1] = new Ticket
                {
                    Name = "Name_" + i.ToString(),
                    OrderDate = DateTime.Now.AddDays(randomTimeDayOffset).AddHours(randomTimeHourOffset).AddMinutes(randomMinuteOffset).AddSeconds(randomSecondOffset),
                    WorkOrder = "WorkOrder_" + i.ToString(),
                    DataBackupByClient = true,
                    TicketStatus = await _serviceFactory.GetTicketStatusesService().GetTicketStatusByName(Enum.GetValues<TicketStatuses>()[randomStatusIndex].ToString()),
                    TicketType = await _serviceFactory.GetTicketTypesService().GetTicketTypeByName(Enum.GetValues<TicketTypes>()[randomTypeIndex].ToString()),
                    Client = new Client
                    {
                        LastName = "LastName_" + i.ToString(),
                        FirstName = "FirstName_" + i.ToString(),
                        StreetAndHouseNumber = "StreetAndHouseNumber_" + i.ToString(),
                        PostalCode = "12345",
                        City = "City_" + i.ToString(),
                        Email = "Email" + i.ToString(),
                        Course = "Course_" + i.ToString(),
                        ParticipantNumber = i,
                        PhoneNumber = "12345678",
                    },
                    Devices = new List<Device>
                    {
                        new Device
                        {
                            Name = "Name_A" + i.ToString(),
                            Comments = "Comments_A" + i.ToString(),
                            DeviceType = "DeviceType_A" + i.ToString(),
                            Manufacturer = "Manufacturer_A" + i.ToString(),
                            SerialNumber = "SerialNumber_A" + i.ToString(),
                            Accessories = "Accessories_A" + i.ToString(),
                            Software = new List<Software>
                            {
                                new Software
                                {
                                    Name = "Name_A" + i.ToString(),
                                    Comments = "Comments_A" + i.ToString()
                                },
                                new Software
                                {
                                    Name = "Name_B" + i.ToString(),
                                    Comments = "Comments_B" + i.ToString()
                                }
                            }
                        },
                        new Device
                        {
                            Name = "Name_B" + i.ToString(),
                            Comments = "Comments_B" + i.ToString(),
                            DeviceType = "DeviceType_B" + i.ToString(),
                            Manufacturer = "Manufacturer_B" + i.ToString(),
                            SerialNumber = "SerialNumber_B" + i.ToString(),
                            Accessories = "Accessories_B" + i.ToString(),
                            Software = new List<Software>
                            {
                                new Software
                                {
                                    Name = "Name_A" + i.ToString(),
                                    Comments = "Comments_A" + i.ToString()
                                },
                                new Software
                                {
                                    Name = "Name_B" + i.ToString(),
                                    Comments = "Comments_B" + i.ToString()
                                }
                            }
                        }
                    }
                };
            };

            await _ticketSystemContext.Tickets.AddRangeAsync(tickets);
            await _ticketSystemContext.SaveChangesAsync();
        }

        public async Task SeedUserRoles()
        {
            var query = from role in _roleManager.Roles
                        select role.Name;

            foreach (var role in Enum.GetNames<DefaultRoles>())
            {
                if (!query.Contains(role.ToString()))
                {
                    await _roleManager.CreateAsync(new Role(role.ToString()));
                }
            }
        }

        public async Task SeedDefaultAdmin()
        {
            User admin = new()
            {
                UserName = "admin",
                FirstName = "Super",
                LastName = "User",
                Email = "admin@localhost",
            };

            if (_userManager.Users.All(user => user.Id != admin.Id))
            {
                var adminInDb = await _userManager.FindByNameAsync(admin.UserName);
                if (adminInDb == null)
                {
                    await _userManager.CreateAsync(admin, "Service1234!");
                    await _userManager.AddToRoleAsync(admin, DefaultRoles.Administrator.ToString());
                }
            }
        }

        public async Task SeedPermissions()
        {
            var permissionsEnumList = Enum.GetValues<RolePermissions>();
            List<string> permissions = new();

            foreach (RolePermissions pEnum in permissionsEnumList)
            {
                permissions.Add(pEnum.ToString());
            }

            List<Permission> tempList = new(_ticketSystemContext.Permissions);

            foreach (var p in tempList)
            {
                if (!permissions.Contains(p.Name))
                {
                    _ticketSystemContext.Permissions.Remove(p);
                }
            }

            foreach (string permission in permissions)
            {
                if (!_ticketSystemContext.Permissions.Where(p => p.Name == permission).Any())
                {
                    Permission perm = new()
                    {
                        Name = permission.ToString()
                    };
                    _ticketSystemContext.Permissions.Add(perm);
                }
            }

            await _ticketSystemContext.SaveChangesAsync();
        }

        public async Task SeedRolePermissions()
        {
            var administrator = await _serviceFactory.GetRolesService().GetRoleByName(DefaultRoles.Administrator.ToString());
            var fallback = await _serviceFactory.GetRolesService().GetRoleByName(DefaultRoles.Fallback.ToString());

            foreach (var permission in Enum.GetValues<RolePermissions>())
            {
                await _serviceFactory.GetRolePermissionsService().AddPermissionToRole(administrator, permission);
            }

            await _serviceFactory.GetRolePermissionsService().RemoveAllPermissionsFromRole(fallback);
        }

        public async Task SeedTicketTypes()
        {
            foreach (var type in Enum.GetValues<TicketTypes>())
            {
                if (!await _ticketSystemContext.TicketTypes.AnyAsync(t => t.Name == type.ToString()))
                {
                    await _ticketSystemContext.TicketTypes.AddAsync(new TicketType() { Name = type.ToString() });
                }
            }
            await _ticketSystemContext.SaveChangesAsync();
        }

        public async Task SeedTicketStatuses()
        {
            foreach (var status in Enum.GetValues(typeof(TicketStatuses)))
            {
                if (!await _ticketSystemContext.TicketStatuses.AnyAsync(t => t.Name == status.ToString()))
                {
                    await _ticketSystemContext.TicketStatuses.AddAsync(new TicketStatus { Name = status.ToString() });
                }
            }
            await _ticketSystemContext.SaveChangesAsync();
        }
    }
}
