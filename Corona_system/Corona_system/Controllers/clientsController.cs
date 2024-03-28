using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Corona_system.Data;
using Corona_system.Models;

namespace Corona_system.Controllers
{
    public class clientsController : Controller
    {
        private readonly Corona_systemContext _context;

        public clientsController(Corona_systemContext context)
        {
            _context = context;
        }

        // GET: clients
        public async Task<IActionResult> Index()
        {
              return _context.client != null ? 
                          View(await _context.client.ToListAsync()) :
                          Problem("Entity set 'Corona_systemContext.client'  is null.");
        }

        // GET: clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.client == null)
            {
                return NotFound();
            }

            var client = await _context.client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,first_name,last_name,dateOfBirth,imageUrl,city_name,street,numStreet,phone,Mobile_Phone,reciving,recovery")] client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.client == null)
            {
                return NotFound();
            }

            var client = await _context.client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,first_name,last_name,dateOfBirth,imageUrl,city_name,street,numStreet,phone,Mobile_Phone,reciving,recovery")] client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!clientExists(client.Id))
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
            return View(client);
        }

        // GET: clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.client == null)
            {
                return NotFound();
            }

            var client = await _context.client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.client == null)
            {
                return Problem("Entity set 'Corona_systemContext.client'  is null.");
            }
            var client = await _context.client.FindAsync(id);
            if (client != null)
            {
                _context.client.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool clientExists(int id)
        {
          return (_context.client?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
