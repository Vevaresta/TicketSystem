using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;
using Ticketsystem.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ticketsystem.Data
{
    public class ContextSeed
    {
        private readonly TicketsystemContext _ticketSystemContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        private readonly IServiceFactory _serviceFactory;
        private readonly RolesService _rolesService;

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
            _rolesService = serviceFactory.GetRolesService();
        }

        public async Task Seed(bool doSeedTestData = false)
        {
            await SeedUserRoles();
            await SeedDefaultAdmin();
            await SeedPermissions();
            await SeedRolePermissions();
            await SeedTicketStatuses();
            await SeedTicketTypes();
            await SeedFallbackClient();

            if (doSeedTestData)
            {
                await SeedTestUsers();
                await SeedGuest();
                if (await _ticketSystemContext.Tickets.FirstOrDefaultAsync(x => x.WorkOrder.Contains("WorkOrder_")) == null)
                {
                    await SeedTestTickets();
                }
            }
        }

        private async Task SeedFallbackClient()
        {
            var fallbackClientInDb = await _ticketSystemContext.Clients.FirstOrDefaultAsync(c => c.Id == "Fallback");

            if (fallbackClientInDb == null)
            {
                var fallbackClient = new Client
                {
                    Id = "Fallback",
                    LastName = "<Client gelöscht>",
                    FirstName = "<Client gelöscht>",
                    Email = "<Client gelöscht>",
                    StreetAndHouseNumber = "<Client gelöscht>",
                    City = "<Client gelöscht>",
                    PostalCode = "00000",
                    Course = "<Client gelöscht>",
                    ParticipantNumber = 0,
                    PhoneNumber = "<Client gelöscht>",
                };

                await _ticketSystemContext.AddAsync(fallbackClient);
                await _ticketSystemContext.SaveChangesAsync();
            }
        }

        private async Task SeedTestUsers()
        {
            User mitarbeiter = new()
            {
                LastName = "Mitarbeiter",
                FirstName = "Mitarbeiter",
                UserName = "mitarbeiter",
                Email = "mitarbeiter@localhost"
            };

            var mitarbeiterInDb = await _userManager.FindByNameAsync(mitarbeiter.UserName);
            if (mitarbeiterInDb == null)
            {
                await _userManager.CreateAsync(mitarbeiter, "Service1234!");
                await _userManager.AddToRoleAsync(mitarbeiter, DefaultRoles.Mitarbeiter.ToString());
            }

            User abteilungsleiter = new()
            {
                LastName = "Abteilungsleiter",
                FirstName = "Abteilungsleiter",
                UserName = "abteilungsleiter",
                Email = "abteilungsleiter@localhost"
            };

            var abteilungsleiterInDb = await _userManager.FindByNameAsync(abteilungsleiter.UserName);
            if (abteilungsleiterInDb == null)
            {
                await _userManager.CreateAsync(abteilungsleiter, "Service1234!");
                await _userManager.AddToRoleAsync(abteilungsleiter, DefaultRoles.Abteilungsleiter.ToString());
            }

            User fallback = new()
            {
                Id = "Fallback",
                LastName = "Fallback",
                FirstName = "Fallback",
                UserName = "fallback",
                Email = "fallback@localhost"
            };

            var fallbackInDb = await _userManager.FindByNameAsync(fallback.UserName);
            if (fallbackInDb == null)
            {
                await _userManager.CreateAsync(fallback, "Service1234!");
                await _userManager.AddToRoleAsync(fallback, DefaultRoles.Fallback.ToString());
            }

            await _ticketSystemContext.SaveChangesAsync();
        }

        private async Task SeedTestTickets()
        {
            Random rnd = new();
            Ticket[] tickets = new Ticket[250];

            for (int i = 1; i < tickets.Length + 1; i++)
            {
                var randomStatusIndex = rnd.Next(0, Enum.GetNames<TicketStatuses>().Length);
                var randomTypeIndex = rnd.Next(0, Enum.GetNames<TicketTypes>().Length);
                var randomTimeDayOffset = rnd.Next(0, 364);
                var randomTimeHourOffset = rnd.Next(0, 24);
                var randomMinuteOffset = rnd.Next(0, 60);
                var randomSecondOffset = rnd.Next(0, 60);

                var orderDate = DateTime.Today.AddYears(-1).AddDays(randomTimeDayOffset).AddHours(randomTimeHourOffset).AddMinutes(randomMinuteOffset).AddSeconds(randomSecondOffset).ToUniversalTime();

                tickets[i - 1] = new Ticket
                {
                    Name = "Name_" + i.ToString(),
                    OrderDate = orderDate,
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
                        Email = "Email" + i.ToString() + "@localhost",
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
                                },
                                new Software
                                {
                                    Name = "Name_C" + i.ToString(),
                                    Comments = "Comments_C" + i.ToString()
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
                        },
                        new Device
                        {
                            Name = "Name_C" + i.ToString(),
                            Comments = "Comments_C" + i.ToString(),
                            DeviceType = "DeviceType_C" + i.ToString(),
                            Manufacturer = "Manufacturer_C" + i.ToString(),
                            SerialNumber = "SerialNumber_C" + i.ToString(),
                            Accessories = "Accessories_C" + i.ToString(),
                            Software = new List<Software>
                            {
                                new Software
                                {
                                    Name = "Name_A" + i.ToString(),
                                    Comments = "Comments_A" + i.ToString()
                                }
                            }
                        }
                    },
                };
            };

            await _ticketSystemContext.Tickets.AddRangeAsync(tickets);

            await _ticketSystemContext.SaveChangesAsync();

            var ticketsInDb = _ticketSystemContext.Tickets.Include(t => t.TicketChanges).ToList();
            foreach (Ticket ticket in ticketsInDb)
            {
                TicketChange initialChange = new()
                {
                    TicketId = ticket.Id,
                    UserId = (await _userManager.FindByNameAsync("admin")).Id,
                    ChangeDate = ticket.OrderDate,
                    OldTicketStatus = await _serviceFactory.GetTicketStatusesService().GetTicketStatusByName("Open"),
                    Comment = "Ticket erstellt"
                };

                _ticketSystemContext.Add(initialChange);
            }

            await _ticketSystemContext.SaveChangesAsync();
        }

        private async Task SeedUserRoles()
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

        private async Task SeedDefaultAdmin()
        {
            User admin = new()
            {
                UserName = "admin",
                FirstName = "Super",
                LastName = "User",
                Email = "admin@localhost",
            };

            var adminInDb = await _userManager.FindByNameAsync(admin.UserName);
            if (adminInDb == null)
            {
                await _userManager.CreateAsync(admin, "Service1234!");
                await _userManager.AddToRoleAsync(admin, DefaultRoles.Administrator.ToString());
            }

            await _ticketSystemContext.SaveChangesAsync();
        }

        private async Task SeedGuest()
        {
            var rolesInDb = from role in _roleManager.Roles
                            select role.Name;

            if (!rolesInDb.Contains("Gast"))
            {
                await _roleManager.CreateAsync(new Role { Name = "Gast", Permissions = new List<Permission>() } );
            }

            User guest = new()
            {
                UserName = "gast",
                FirstName = "Gast",
                LastName = "Benutzer",
                Email = "guest@localhost",
            };

            var guestInDb = await _userManager.FindByNameAsync(guest.UserName);
            if (guestInDb == null)
            {
                await _userManager.CreateAsync(guest, "Gast1234!");
                await _userManager.AddToRoleAsync(guest, "Gast");
            }

            var roleInDb = await _serviceFactory.GetRolesService().GetRoleByName("Gast");
            foreach (var permission in Enum.GetValues<RolePermissions>())
            {
                if (permission.ToString().Contains("Ticket") || permission.ToString().Contains("Client"))
                {
                    await _serviceFactory.GetRolePermissionsService().AddPermissionToRole(roleInDb, permission);
                }
            }

            await _ticketSystemContext.SaveChangesAsync();
        }

        private async Task SeedPermissions()
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

        private async Task SeedRolePermissions()
        {
            var administrator = await _serviceFactory.GetRolesService().GetRoleByName(DefaultRoles.Administrator.ToString());
            var fallback = await _serviceFactory.GetRolesService().GetRoleByName(DefaultRoles.Fallback.ToString());

            foreach (var permission in Enum.GetValues<RolePermissions>())
            {
                await _serviceFactory.GetRolePermissionsService().AddPermissionToRole(administrator, permission);
            }

            await _serviceFactory.GetRolePermissionsService().RemoveAllPermissionsFromRole(fallback);
        }

        private async Task SeedTicketTypes()
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

        private async Task SeedTicketStatuses()
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
