using ComicSystem.Data;
using ComicSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ComicSystemDbContext _context;

        public RentalsController(ComicSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rentals = await _context.Rentals.Include(r => r.Customer).ToListAsync();
            return View(rentals);
        }
        public async Task<IActionResult> Details(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Customer)  // Ensure the customer info is included
                .FirstOrDefaultAsync(r => r.RentalID == id);

            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }
        public IActionResult Create()
        {
            ViewBag.Customers = _context.Customers.ToList(); // Get a list of customers
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rental rental)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Customers = _context.Customers.ToList(); // Ensure the customer list is still available
            return View(rental);
        }


        // public IActionResult Create()
        // {
        //     ViewData["ComicBooks"] = new SelectList(_context.ComicBooks, "ComicBookID", "Title");
        //     ViewData["Customers"] = new SelectList(_context.Customers, "CustomerID", "Fullname");
        //     return View();
        // }

        // [HttpPost]
        // public async Task<IActionResult> Create(Rental rental, List<RentalDetail> rentalDetails)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         _context.Rentals.Add(rental);
        //         await _context.SaveChangesAsync();

        //         foreach (var detail in rentalDetails)
        //         {
        //             detail.RentalID = rental.RentalID;
        //             _context.RentalDetails.Add(detail);
        //         }
        //         await _context.SaveChangesAsync();

        //         return RedirectToAction(nameof(Index));
        //     }
        //     ViewData["ComicBooks"] = new SelectList(_context.ComicBooks, "ComicBookID", "Title");
        //     ViewData["Customers"] = new SelectList(_context.Customers, "CustomerID", "Fullname");
        //     return View(rental);
        // }
    }

}