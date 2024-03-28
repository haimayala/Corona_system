using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Corona_system.Data;
using Corona_system.Models;
using Corona_system.ViewModels;

namespace Corona_system.Controllers
{
    public class vaccinationsController : Controller
    {
        private readonly Corona_systemContext _context;

        public vaccinationsController(Corona_systemContext context)
        {
            _context = context;
        }

        // GET: vaccinations
        public async Task<IActionResult> Index()
        {
            return _context.vaccination != null ?
                        View(await _context.vaccination.ToListAsync()) :
                        Problem("Entity set 'WebApplication11Context.vaccination'  is null.");
        }
        // GET: vaccinations/CreateForClient
        public IActionResult CreateForClient(int clientId)
        {
            var viewModel = new vaccination();
            viewModel.ClientId = clientId;
            return View(viewModel);
        }


        public async Task<IActionResult> Index1(int id)
        {
            // Retrieve client information
            var client = await _context.client.FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            // Retrieve vaccinations for the client
            var vaccinations = await _context.vaccination.Where(v => v.ClientId == id).ToListAsync();

            // Create InformationViewModel to pass both client and vaccinations to the view
            var viewModel = new InformationViewModel
            {
                Client = client,
                Vaccinations = vaccinations
            };

            return View(viewModel);
        }



        // GET: vaccinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.vaccination == null)
            {
                return NotFound();
            }

            var vaccination = await _context.vaccination
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vaccination == null)
            {
                return NotFound();
            }

            return View(vaccination);
        }

        // GET: vaccinations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: vaccinations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VaccinationDate,VaccineManufacturer,ClientId")] vaccination vaccination)
        {
            if (ModelState.IsValid)
            {
                // Check if the client already has 4 vaccinations
                var vaccinationCount = await _context.vaccination
                                                    .Where(v => v.ClientId == vaccination.ClientId)
                                                    .CountAsync();

                if (vaccinationCount >= 4)
                {
                    ModelState.AddModelError(string.Empty, "A client can have maximum 4 vaccinations.");
                    return View(vaccination);
                }

                // Otherwise, proceed to add the new vaccination
                _context.Add(vaccination);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "clients");
            }
            return View(vaccination);
        }



        // GET: vaccinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.vaccination == null)
            {
                return NotFound();
            }

            var vaccination = await _context.vaccination.FindAsync(id);
            if (vaccination == null)
            {
                return NotFound();
            }
            return View(vaccination);
        }

        // POST: vaccinations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VaccinationDate,VaccineManufacturer,ClientId")] vaccination vaccination)
        {
            if (id != vaccination.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vaccination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!vaccinationExists(vaccination.Id))
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
            return View(vaccination);
        }

        // GET: vaccinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.vaccination == null)
            {
                return NotFound();
            }

            var vaccination = await _context.vaccination
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vaccination == null)
            {
                return NotFound();
            }

            return View(vaccination);
        }

        // POST: vaccinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.vaccination == null)
            {
                return Problem("Entity set 'WebApplication11Context.vaccination'  is null.");
            }
            var vaccination = await _context.vaccination.FindAsync(id);
            if (vaccination != null)
            {
                _context.vaccination.Remove(vaccination);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool vaccinationExists(int id)
        {
            return (_context.vaccination?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
