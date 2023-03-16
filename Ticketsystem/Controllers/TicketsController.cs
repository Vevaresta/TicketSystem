﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models;
using Ticketsystem.Services;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly IServiceFactory _serviceFactory;

        public TicketsController(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(
            int take = 10,
            int skip = 0,
            string sortBy = "OrderDate",
            bool doReverse = false,
            string filterByTicketId = "",
            string filterByTicketName = "",
            string filterByTicketStatus = "",
            string filterByClientName = "",
            string filterByStartDate = "",
            string filterByEndDate = "",
            string filterByTicketType = ""
            )
        {
            TicketQuery queryModel = new TicketQuery
            {
                Take = take,
                Skip = skip,
                SortByAttribute = sortBy,
                DoReverse = doReverse,
                FilterByTicketId = filterByTicketId,
                FilterByTicketName = filterByTicketName,
                FilterByTicketStatus = filterByTicketStatus,
                FilterByTicketType = filterByTicketType,
                FilterByClientName = filterByClientName,
                FilterByStartDate = filterByStartDate,
                FilterByEndDate = filterByEndDate
            };

            var tickets = await _serviceFactory.GetTicketsService().GetAllTickets(queryModel);

            List<TicketViewModel> ticketViewModels = new();

            foreach (var ticket in tickets)
            {
                ticketViewModels.Add(ticket);
            }

            ViewBag.Take = take;
            ViewBag.Skip = skip;
            ViewBag.SortBy = sortBy;
            ViewBag.TicketsCount = _serviceFactory.GetTicketsService().GetTicketsCount(queryModel);
            ViewBag.DoReverse = doReverse;
            ViewBag.FilterByTicketId = filterByTicketId;
            ViewBag.FilterByTicketName = filterByTicketName;
            ViewBag.FilterByTicketStatus = filterByTicketStatus;
            ViewBag.FilterByClientName = filterByClientName;
            ViewBag.FilterByStartDate = filterByStartDate;
            ViewBag.FilterByEndDate = filterByEndDate;
            ViewBag.FilterByTicketType = filterByTicketType;

            return View(ticketViewModels);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _serviceFactory.GetTicketsService().GetTicketById(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
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
                ticket.TicketType = await _serviceFactory.GetTicketTypesService().GetTicketTypeByName(ticketType);

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

                var ticketStatusOpen = await _serviceFactory.GetTicketStatusesService().GetTicketStatusByName(TicketStatuses.Open.ToString());

                ticket.TicketStatus = ticketStatusOpen;
                ticket.OrderDate = DateTime.Now;

                await _serviceFactory.GetTicketsService().AddTicket(ticket);

                return RedirectToAction(nameof(Index));
            }
            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticketViewModel.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticketViewModel.TicketTypeId);
            return View(ticketViewModel);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _serviceFactory.GetTicketsService().GetTicketById(id);

            if (ticket == null)
            {
                return NotFound();
            }

            //ViewData["TicketStatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.TicketStatusId);
            //ViewData["TicketTypeId"] = new SelectList(_context.TicketTypes, "Id", "Id", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketTypeId,TicketStatusId,WorkOrder,DataBackupByClient,DataBackupByStaff,DataBackupDone,Comments")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceFactory.GetTicketsService().UpdateTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _serviceFactory.GetTicketsService().GetTicketById(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _serviceFactory.GetTicketsService().GetTicketById(id);

            await _serviceFactory.GetTicketsService().DeleteTicket(ticket);

            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _serviceFactory.GetTicketsService().GetTicketById(id) != null;
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
