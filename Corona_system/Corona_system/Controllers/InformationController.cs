using Corona_system.Data;
using Corona_system.Models;
using Corona_system.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Corona_system.Controllers
{
    public class InformationController : Controller
    {
        private readonly Corona_systemContext _context;

        public InformationController(Corona_systemContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            // Get client details
            var client = _context.client.FirstOrDefault(c => c.Id == id);

            if (client == null)
            {
                return NotFound(); // If client not found, return 404 Not Found
            }

            // Get vaccinations for the client
            var vaccinations = _context.vaccination.Where(v => v.ClientId == id).ToList();

            // Combine client and vaccination data into a view model
            var viewModel = new InformationViewModel
            {
                Client = client,
                Vaccinations = vaccinations
            };

            return View(viewModel);
        }
    }
}
