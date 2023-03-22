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
using Ticketsystem.Services;
using Ticketsystem.ViewModels;

namespace Ticketsystem.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ClientsService _clientsService;

        public ClientsController(IServiceFactory serviceFactory)
        {
            _clientsService = serviceFactory.GetClientsService();
        }

        // GET: Clients
        public async Task<IActionResult> Index(ClientData clientData)
        {
            List<Client> clients = await _clientsService.GetAllClients(clientData);
            List<ClientViewModel> clientViewModels = new();

            foreach (var client in clients)
            {
                clientViewModels.Add(client);
            }

            ViewBag.Take = clientData.Take;
            ViewBag.Skip = clientData.Skip;
            ViewBag.SortBy = clientData.SortBy;
            ViewBag.ClientsCount = _clientsService.GetClientsCount(clientData);
            ViewBag.DoReverse = clientData.DoReverse;
            ViewBag.FilterByLastName = clientData.FilterByLastName;
            ViewBag.FilterByFirstName = clientData.FilterByFirstName;
            ViewBag.FilterByEmail = clientData.FilterByEmail;

            return View(clientViewModels);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string id)
        {
           var client = await _clientsService.GetClientById(id);
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
           var client = await _clientsService.GetClientById(id);
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
                    await _clientsService.UpdateClient(client);
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
            var client = await _clientsService.GetClientById(id);

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
            var client = await _clientsService.GetClientById(id);

            await _clientsService.DeleteClient(client);

            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
           return _clientsService.GetClientById(id) != null;
        }
    }
}
