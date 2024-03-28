using Corona_system.Data;
using Corona_system.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Corona_system.Controllers
{
    public class ClientVaccinationsController : Controller
    {
        private readonly Corona_systemContext _context;

        public ClientVaccinationsController(Corona_systemContext context)
        {
            _context = context;
        }

        // GET: ClientVaccinations/Index/{clientId}
        public async Task<IActionResult> Index(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            var client = await _context.client.FindAsync(clientId);

            if (client == null)
            {
                return NotFound();
            }

            var vaccinations = await _context.vaccination
                .Where(v => v.ClientId == clientId)
                .ToListAsync();

            return View(vaccinations);
        }

        // GET: ClientVaccinations/Create/{clientId}
        public IActionResult Create(int? clientId)
        {
            if (clientId == null)
            {
                return NotFound();
            }

            return View(new vaccination { ClientId = clientId.Value });
        }

        // POST: ClientVaccinations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int clientId, [Bind("Id,VaccinationDate,VaccineManufacturer,ClientId")] vaccination vaccination)
        {
            if (ModelState.IsValid)
            {
                // Check if the client already has 4 vaccinations
                var vaccinationCount = await _context.vaccination
                                                    .Where(v => v.ClientId == clientId)
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
    }
}
