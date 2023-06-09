﻿using Ticketsystem.Enums;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;
using Ticketsystem.DbAccess;
using Ticketsystem.ViewModels;
using Ticketsystem.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System.Net;

namespace Ticketsystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ClientsDbAccess _clientsService;
        private readonly TicketsDbAccess _ticketsService;
        private readonly TicketStatusesDbAccess _ticketStatusesService;
        private readonly TicketTypesDbAccess _ticketTypesService;
        private readonly TicketChangesDbAccess _ticketChangesService;
        private readonly UserManager<User> _userManager;
        private readonly PdfUtility _pdfUtilty;
        private readonly IEmailSender _emailSender;

        public TicketsController(
            IDbAccessFactory serviceFactory,
            UserManager<User> userManager,
            PdfUtility pdfUtility,
            IEmailSender emailSender)
        {
            _ticketsService = serviceFactory.GetDbAccess<TicketsDbAccess>();
            _clientsService = serviceFactory.GetDbAccess<ClientsDbAccess>();
            _ticketStatusesService = serviceFactory.GetDbAccess<TicketStatusesDbAccess>();
            _ticketTypesService = serviceFactory.GetDbAccess<TicketTypesDbAccess>();
            _ticketChangesService = serviceFactory.GetDbAccess<TicketChangesDbAccess>();
            _userManager = userManager;
            _pdfUtilty = pdfUtility;
            _emailSender = emailSender;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(TicketFilterData ticketData)
        {
            var tickets = await _ticketsService.GetAll<TicketIndexViewModel>(ticketData);

            var statusPercentages = await _ticketStatusesService.GetTicketStatusPercentages();

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
            ViewBag.TicketStatusesOpen = statusPercentages[0];
            ViewBag.TicketStatusesInProgress = statusPercentages[1];
            ViewBag.TicketStatusesClose = statusPercentages[2];

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
            if (ticketViewModel.DoBackup)
            {
                if (ticketViewModel.BackupChoices == BackupChoices.BackupByStaff.ToString())
                {
                    ticketViewModel.DataBackupByStaff = true;
                }
                else if (ticketViewModel.BackupChoices == BackupChoices.BackupByClient.ToString())
                {
                    ticketViewModel.DataBackupByClient = true;
                    ticketViewModel.DataBackupDone = true;
                }
            }

            ticketViewModel.Devices = JsonConvert.DeserializeObject<List<DeviceViewModel>>(deviceList);

            if (ModelState.IsValid)
            {
                Ticket ticket = ticketViewModel;
                if (ticketViewModel.Client.Id != 0)
                {
                    ticket.Client.Id = ticketViewModel.Client.Id;
                    ticket.ClientId = ticketViewModel.Client.Id;
                }
                ticket.TicketType = await _ticketTypesService.GetTicketTypeByName(ticketType);

                var ticketStatusOpen = await _ticketStatusesService.GetTicketStatusByName(TicketStatuses.Open.ToString());

                ticket.TicketStatus = ticketStatusOpen;
                ticket.OrderDate = DateTime.Now.ToUniversalTime();

                var newTicketInDb = await _ticketsService.Add(ticket);

                TicketChange ticketChange = new()
                {
                    TicketId = ticket.Id,
                    UserId = loggedInUserId,
                    ChangeDate = DateTime.Now.ToUniversalTime(),
                    OldTicketStatus = ticketStatusOpen,
                    Comment = "Ticket erstellt"
                };

                await _ticketChangesService.AddTicketChange(ticketChange);

                if (ticketViewModel.DoSendEmail)
                {
                    await SendEmail(newTicketInDb.Id);
                }

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
            ticketViewModel.Id = ticket.Id;
            ticketViewModel.TicketChanges = new List<TicketChangeViewModel>();

            if (ticket.PdfSigned != null)
            {
                ticketViewModel.IsPdfSigned = true;
            }

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

            if (ticketViewModel.DoBackup)
            {
                if (ticketViewModel.BackupChoices == BackupChoices.BackupByStaff.ToString())
                {
                    ticketViewModel.DataBackupByStaff = true;
                }
                else if (ticketViewModel.BackupChoices == BackupChoices.BackupByClient.ToString())
                {
                    ticketViewModel.DataBackupByClient = true;
                    ticketViewModel.DataBackupDone = true;
                }
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

        public async Task<IActionResult> ShowPdf(int id)
        {
            var ticket = await _ticketsService.GetById<Ticket, int>(id);

            string formDataDeviceType = "";
            string formDataDeviceSerialNumber = "";
            string formDataDeviceAccessories = "";

            if (ticket.Devices != null)
            {
                if (ticket.Devices.Count == 1)
                {
                    formDataDeviceType = ticket.Devices[0].DeviceType;
                    formDataDeviceSerialNumber = ticket.Devices[0].SerialNumber;
                    formDataDeviceAccessories = ticket.Devices[0].Accessories;
                }
            }

            PdfFormData formData = new()
            {
                TicketId = ticket.Id.ToString(),
                WorkOrder = ticket.WorkOrder,
                TicketType = ticket.TicketType.Name,
                ClientName = ticket.Client.FirstName ?? "" + " " + ticket.Client.LastName ?? "",
                ClientEmail = ticket.Client.Email,
                ClientPhone = ticket.Client.PhoneNumber,
                BackupByClient = ticket.DataBackupByClient,
                BackupByStaff = ticket.DataBackupByStaff,
                DeviceType = formDataDeviceType,
                DeviceSerialNumber = formDataDeviceSerialNumber,
                DeviceAccessories = formDataDeviceAccessories,
            };

            var pdf = await _pdfUtilty.FillPdfNewTicket(formData);

            if (pdf != null)
            {
                return File(pdf, "application/pdf");
            }
            else
            {
                return RedirectToPage("/Tickets/ErrorPdfNotFound");
            }
        }

        public async Task<IActionResult> UploadPdf(int id)
        {
            var ticket = await _ticketsService.GetById<Ticket, int>(id);

            byte[] fileBytes = new byte[Request.ContentLength.Value];
            Request.Body.Read(fileBytes, 0, fileBytes.Length);

            ticket.PdfSigned = fileBytes;
            await _ticketsService.Update(ticket);

            return Json(new { Message = "SUCCESS" });
        }

        public async Task<IActionResult> ShowPdfSigned(int id)
        {
            var ticket = await _ticketsService.GetById<Ticket, int>(id);

            if (ticket.PdfSigned != null)
            {
                return File(ticket.PdfSigned, "application/pdf");
            }
            else
            {
                return Json(new { message = "Error" });
            }

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

        [HttpPost]
        public async Task<IActionResult> SendEmail(int id)
        {
            var ticketInDb = await _ticketsService.GetById<Ticket, int>(id);

            var clientName = ticketInDb.Client.FirstName + " " + ticketInDb.Client.LastName;
            var clientEmail = ticketInDb.Client.Email;

            try
            {
                Message message = new(new List<MailboxAddress>() { new MailboxAddress(clientName, clientEmail) }, EmailTypes.ConfirmationEmail);
                await _emailSender.SendEmail(message);

                return Json(new { Message = "SUCCESS" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailClosed(int id)
        {
            var ticketInDb = await _ticketsService.GetById<Ticket, int>(id);

            var clientName = ticketInDb.Client.FirstName + " " + ticketInDb.Client.LastName;
            var clientEmail = ticketInDb.Client.Email;

            try
            {
                Message message = new(new List<MailboxAddress>() { new MailboxAddress(clientName, clientEmail) }, EmailTypes.OrderFinished);
                await _emailSender.SendEmail(message);

                return Json(new { Message = "SUCCESS" });
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { ex.Message });
            }
        }

        public async Task<IActionResult> CreateFromClient(string id)
        {
            var client = await _clientsService.GetById<Client, string>(id);
            ClientViewModel clientViewModel = client;

            if (clientViewModel == null)
            {
                return NotFound();
            }

            TicketViewModel ticketViewModel = new();
            ticketViewModel.Client = client;

            return View("Create", ticketViewModel);
        }
    }
}
