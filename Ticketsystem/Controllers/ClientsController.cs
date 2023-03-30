using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Data;
using Ticketsystem.Models.Data;
using Ticketsystem.Models.Database;
using Ticketsystem.DbAccess;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ClientsDbAccess _clientsService;

        public ClientsController(IDbAccessFactory serviceFactory)
        {
            _clientsService = serviceFactory.GetDbAccess<ClientsDbAccess>();
        }

        // GET: Clients
        public async Task<IActionResult> Index(ClientFilterData clientData)
        {
            List<ClientIndexViewModel> clients = await _clientsService.GetAll<ClientIndexViewModel>(clientData);

            ViewBag.Take = clientData.Take;
            ViewBag.Skip = clientData.Skip;
            ViewBag.SortBy = clientData.SortBy;
            ViewBag.ClientsCount = _clientsService.GetCount(clientData);
            ViewBag.DoReverse = clientData.DoReverse;
            ViewBag.FilterByLastName = clientData.FilterByLastName;
            ViewBag.FilterByFirstName = clientData.FilterByFirstName;
            ViewBag.FilterByEmail = clientData.FilterByEmail;

            return View(clients);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string id)
        {
           var client = await _clientsService.GetById<Client, string>(id);
           if (client == null)
           {
               return NotFound();
           }

           ClientViewModel clientViewModel = client;

           return View(clientViewModel);
        }

        // GET: Clients/Update/5
        public async Task<IActionResult> Update(string id)
        {
           var client = await _clientsService.GetById<Client, string>(id);
           if (client == null)
           {
               return NotFound();
           }

           ClientViewModel clientViewModel = client;

           return View(clientViewModel);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, ClientViewModel clientViewModel)
        {
            if (id != clientViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Client client = clientViewModel.CopyForUpdate();

                try
                {
                    await _clientsService.Update(client);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(clientViewModel);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var client = await _clientsService.GetById<Client, string>(id);

            if (client == null)
            {
                return NotFound();
            }

            ClientViewModel clientViewModel = client;
            return View(clientViewModel);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _clientsService.GetById<Client, string>(id);

            await _clientsService.Delete(client);

            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
           return _clientsService.GetById<Client, string>(id) != null;
        }
    }
}
