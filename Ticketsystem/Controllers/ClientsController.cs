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

            return View(clientViewModels);
        }

        // GET: Clients/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null || _context.Clients == null)
        //    {
        //        return NotFound();
        //    }

        //    var client = await _context.Clients
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (client == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(client);
        //}

        // GET: Clients/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,Email,StreetAndHouseNumber,PostalCode,City,PhoneNumber,ParticipantNumber,Course")] Client client)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(client);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(client);
        //}

        // GET: Clients/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null || _context.Clients == null)
        //    {
        //        return NotFound();
        //    }

        //    var client = await _context.Clients.FindAsync(id);
        //    if (client == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(client);
        //}

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,LastName,FirstName,Email,StreetAndHouseNumber,PostalCode,City,PhoneNumber,ParticipantNumber,Course")] Client client)
        //{
        //    if (id != client.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(client);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ClientExists(client.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(client);
        //}

        // GET: Clients/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.Clients == null)
        //    {
        //        return NotFound();
        //    }

        //    var client = await _context.Clients
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (client == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(client);
        //}

        // POST: Clients/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_context.Clients == null)
        //    {
        //        return Problem("Entity set 'TicketsystemContext.Clients'  is null.");
        //    }
        //    var client = await _context.Clients.FindAsync(id);
        //    if (client != null)
        //    {
        //        _context.Clients.Remove(client);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ClientExists(string id)
        //{
        //    return _context.Clients.Any(e => e.Id == id);
        //}
    }
}
