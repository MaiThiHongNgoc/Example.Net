using ComicSystem.Data;
using ComicSystem.Models;
using Microsoft.AspNetCore.Mvc;
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

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            var rentals = await _context.Rentals
                .Include(r => r.Customer) // Include the Customer data
                .Include(r => r.RentalDetails) // Include the related RentalDetails
                    .ThenInclude(rd => rd.ComicBook) // Then Include the related ComicBook for each RentalDetail
                .ToListAsync();

            return View(rentals);
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.Customer)
                .Include(r => r.RentalDetails)
                .ThenInclude(rd => rd.ComicBook)
                .FirstOrDefaultAsync(r => r.RentalID == id);

            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            ViewBag.Customers = _context.Customers.ToList(); // Populate customer list
            ViewBag.ComicBooks = _context.ComicBooks.ToList(); // Populate comic book list
            return View();
        }

        // POST: Rentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rental rental, List<RentalDetail> rentalDetails)
        {
            if (ModelState.IsValid)
            {
                // Add the rental first and save to generate RentalID
                _context.Add(rental);
                await _context.SaveChangesAsync();

                // Ensure each RentalDetail is linked to the Rental
                foreach (var detail in rentalDetails)
                {
                    detail.RentalID = rental.RentalID;  // Set RentalID for each detail
                    _context.Add(detail);  // Add RentalDetail to the context
                }

                await _context.SaveChangesAsync();  // Commit changes for rental and details

                return RedirectToAction(nameof(Index)); // Redirect to Rentals index
            }

            // If the model is invalid, repopulate the dropdown lists and return to the view
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.ComicBooks = _context.ComicBooks.ToList();
            return View(rental);
        }
    }
}
