using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;
using Ticketsystem.DbAccess;
using Ticketsystem.ViewModels;
using Ticketsystem.Extensions;

namespace Ticketsystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketsClientsDbAccess _ticketsService;
        private readonly TicketStatusesDbAccess _ticketStatusesService;
        private readonly TicketTypesDbAccess _ticketTypesService;
        private readonly TicketChangesDbAccess _ticketChangesService;
        private readonly UserManager<User> _userManager;

        public TicketsController(IDbAccessFactory serviceFactory, UserManager<User> userManager)
        {
            _ticketsService = serviceFactory.GetTicketsClientsDbAccess<TicketsDbAccess>();
            _ticketStatusesService = serviceFactory.GetTicketStatusesDbAccess();
            _ticketTypesService = serviceFactory.GetTicketTypesDbAccess();
            _ticketChangesService = serviceFactory.GetTicketChangesDbAccess();
            _userManager = userManager;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(TicketFilterData ticketData)
        {
            var tickets = await _ticketsService.GetAll<TicketIndexViewModel>(ticketData);

            ViewBag.Take = ticketData.Take;
            ViewBag.Skip = ticketData.Skip;
            ViewBag.SortBy = ticketData.SortBy;
            ViewBag.TicketsCount = _ticketsService.GetCount(ticketData);
            ViewBag.DoReverse = ticketData.DoReverse;
            ViewBag.FilterByTicketId = ticketData.FilterByTicketId;
            ViewBag.FilterByTicketName = ticketData.FilterByTicketName;
            ViewBag.FilterByTicketStatus = ticketData.FilterByTicketStatus;
            ViewBag.FilterByClientName = ticketData.FilterByClientName;
            ViewBag.FilterByStartDate = ticketData.FilterByStartDate;
            ViewBag.FilterByEndDate = ticketData.FilterByEndDate;
            ViewBag.FilterByTicketType = ticketData.FilterByTicketType;

            return View(tickets);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id");
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TicketViewModel ticketViewModel, string ticketType, string deviceList, string loggedInUserId)
        {
            ticketViewModel.Devices = JsonConvert.DeserializeObject<List<DeviceViewModel>>(deviceList);

            if (ModelState.IsValid)
            {
                Ticket ticket = ticketViewModel;
                ticket.TicketType = await _ticketTypesService.GetTicketTypeByName(ticketType);

                if (ticketViewModel.DoBackup)
                {
                    if (ticketViewModel.BackupChoices == BackupChoices.BackupByStaff.ToString())
                    {
                        ticket.DataBackupByStaff = true;
                    }
                    else if (ticketViewModel.BackupChoices == BackupChoices.BackupByClient.ToString())
                    {
                        ticket.DataBackupByClient = true;
                        ticket.DataBackupDone = true;
                    }
                }

                var ticketStatusOpen = await _ticketStatusesService.GetTicketStatusByName(TicketStatuses.Open.ToString());

                ticket.TicketStatus = ticketStatusOpen;
                ticket.OrderDate = DateTime.Now.ToUniversalTime();

                await _ticketsService.Add(ticket);

                TicketChange ticketChange = new()
                {
                    TicketId = ticket.Id,
                    UserId = loggedInUserId,
                    ChangeDate = DateTime.Now.ToUniversalTime(),
                    OldTicketStatus = ticketStatusOpen,
                    Comment = "Ticket erstellt"
                };

                await _ticketChangesService.AddTicketChange(ticketChange);

                return RedirectToAction(nameof(Index));
            }
            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticketViewModel.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticketViewModel.TicketTypeId);
            return View(ticketViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketsService.GetById<Ticket, int>(id);

            if (ticket == null)
            {
                return NotFound();
            }

            TicketViewModel ticketViewModel = ticket;
            ticketViewModel.TicketChanges = new List<TicketChangeViewModel>();

            foreach (var ticketChange in ticket.TicketChanges)
            {
                ticketViewModel.TicketChanges.Add(ticketChange);
            }

            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticketViewModel);
        }

        public async Task<IActionResult> Update(int id)
        {
            var ticket = await _ticketsService.GetById<Ticket, int>(id);

            if (ticket == null)
            {
                return NotFound();
            }

            TicketViewModel ticketViewModel = ticket;
            ticketViewModel.TicketChanges = new List<TicketChangeViewModel>();

            foreach (var ticketChange in ticket.TicketChanges)
            {
                ticketViewModel.TicketChanges.Add(ticketChange);
            }

            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticketViewModel);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TicketViewModel ticketViewModel, string ticketType, string deviceList, string loggedInUserId, string ticketStatusChange)
        {
            if (id != ticketViewModel.Id)
            {
                return NotFound();
            }

            ticketViewModel.Devices = JsonConvert.DeserializeObject<List<DeviceViewModel>>(deviceList);

            if (ModelState.IsValid)
            {
                Ticket ticket = ticketViewModel.CopyForUpdate();

                TicketChange ticketChange = new()
                {
                    TicketId = ticket.Id,
                    UserId = loggedInUserId,
                    ChangeDate = DateTime.Now.ToUniversalTime(),
                    Comment = ticketViewModel.TicketChange.Comment,
                    OldTicketStatus = await _ticketStatusesService.GetTicketStatusByName(ticketViewModel.TicketStatus.ToString())
                };

                if (ticketStatusChange != ticketViewModel.TicketStatus.ToString())
                {
                    ticketChange.NewTicketStatus = await _ticketStatusesService.GetTicketStatusByName(ticketStatusChange);
                }

                await _ticketChangesService.AddTicketChange(ticketChange);

                ticket.TicketType = await _ticketTypesService.GetTicketTypeByName(ticketType);
                ticket.TicketStatus = await _ticketStatusesService.GetTicketStatusByName(ticketStatusChange);

                if (ticketViewModel.DoBackup)
                {
                    if (ticketViewModel.BackupChoices == BackupChoices.BackupByStaff.ToString())
                    {
                        ticket.DataBackupByStaff = true;
                    }
                    else if (ticketViewModel.BackupChoices == BackupChoices.BackupByClient.ToString())
                    {
                        ticket.DataBackupByClient = true;
                        ticket.DataBackupDone = true;
                    }
                }
                try
                {
                    await _ticketsService.Update(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticketViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }


            var t = await _ticketsService.GetById<Ticket, int>(id);

            if (t == null)
            {
                return NotFound();
            }

            //ticketViewModel = t;
            ticketViewModel.TicketStatus = Enum.GetValues<TicketStatuses>().FirstOrDefault(ts => ts.ToString() == ticketStatusChange);
            ticketViewModel.TicketChanges = new List<TicketChangeViewModel>();

            foreach (var ticketChange in t.TicketChanges)
            {
                ticketViewModel.TicketChanges.Add(ticketChange);
            }

            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticketViewModel);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketsService.GetById<Ticket, int>(id);

            if (ticket == null)
            {
                return NotFound();
            }

            TicketViewModel ticketViewModel = ticket;
            return View(ticketViewModel);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _ticketsService.GetById<Ticket, int>(id);

            await _ticketsService.Delete(ticket);

            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _ticketsService.GetById<Ticket, int>(id) != null;
        }

        public IActionResult TicketHistory()
        {
            return View();
        }

        public IActionResult Login()
        {
            return LocalRedirect("/Identity/Account/Login");
        }

        public IActionResult PermissionError()
        {
            return RedirectToAction("PermissionError", "Home");
        }
    }
}
