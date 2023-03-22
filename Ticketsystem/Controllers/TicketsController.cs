using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;
using Ticketsystem.Services;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly TicketsService _ticketsService;
        private readonly TicketStatusesService _ticketStatusesService;
        private readonly TicketTypesService _ticketTypesService;

        public TicketsController(IServiceFactory serviceFactory)
        {
            _ticketsService = serviceFactory.GetTicketsService();
            _ticketStatusesService = serviceFactory.GetTicketStatusesService();
            _ticketTypesService = serviceFactory.GetTicketTypesService();
        }

        // GET: Tickets
        public async Task<IActionResult> Index(TicketData ticketData)
        {
            var tickets = await _ticketsService.GetAllTickets(ticketData);

            List<TicketViewModel> ticketViewModels = new();

            foreach (var ticket in tickets)
            {
                ticketViewModels.Add(ticket);
            }

            ViewBag.Take = ticketData.Take;
            ViewBag.Skip = ticketData.Skip;
            ViewBag.SortBy = ticketData.SortBy;
            ViewBag.TicketsCount = _ticketsService.GetTicketsCount(ticketData);
            ViewBag.DoReverse = ticketData.DoReverse;
            ViewBag.FilterByTicketId = ticketData.FilterByTicketId;
            ViewBag.FilterByTicketName = ticketData.FilterByTicketName;
            ViewBag.FilterByTicketStatus = ticketData.FilterByTicketStatus;
            ViewBag.FilterByClientName = ticketData.FilterByClientName;
            ViewBag.FilterByStartDate = ticketData.FilterByStartDate;
            ViewBag.FilterByEndDate = ticketData.FilterByEndDate;
            ViewBag.FilterByTicketType = ticketData.FilterByTicketType;

            return View(ticketViewModels);
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
        public async Task<IActionResult> Create(TicketViewModel ticketViewModel, string ticketType, string deviceList)
        {
            if (ModelState.IsValid)
            {
                ticketViewModel.Devices = JsonConvert.DeserializeObject<List<DeviceViewModel>>(deviceList);
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

                await _ticketsService.AddTicket(ticket);

                return RedirectToAction(nameof(Index));
            }
            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticketViewModel.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticketViewModel.TicketTypeId);
            return View(ticketViewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketsService.GetTicketById(id);

            if (ticket == null)
            {
                return NotFound();
            }

            TicketViewModel ticketViewModel = ticket;

            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticketViewModel);
        }

        public async Task<IActionResult> Update(int id)
        {
            var ticket = await _ticketsService.GetTicketById(id);

            if (ticket == null)
            {
                return NotFound();
            }

            TicketViewModel ticketViewModel = ticket;

            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticketViewModel);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TicketViewModel ticketViewModel, string ticketType, string deviceList)
        {
            if (id != ticketViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ticketViewModel.Devices = JsonConvert.DeserializeObject<List<DeviceViewModel>>(deviceList);
                Ticket ticket = ticketViewModel.CopyForUpdate();
                ticket.TicketType = await _ticketTypesService.GetTicketTypeByName(ticketType);
                ticket.TicketStatus = await _ticketStatusesService.GetTicketStatusByName(ticketViewModel.TicketStatus.ToString());

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
                    await _ticketsService.UpdateTicket(ticket);
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

            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticketViewModel);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketsService.GetTicketById(id);

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
            var ticket = await _ticketsService.GetTicketById(id);

            await _ticketsService.DeleteTicket(ticket);

            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _ticketsService.GetTicketById(id) != null;
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
