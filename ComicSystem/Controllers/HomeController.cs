using Microsoft.AspNetCore.Mvc;
using ComicSystem.Data;
using ComicSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace ComicSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ComicSystemDbContext _context;

        public HomeController(ComicSystemDbContext context)
        {
            _context = context;
        }

        // Comic Book Management

        // Display the list of comic books
        public async Task<IActionResult> Index()
        {
            var comicBooks = await _context.ComicBooks.ToListAsync();
            return View(comicBooks);
        }

        // Create new comic book (GET)
        public IActionResult CreateComicBook()
        {
            return View();
        }

        // Create new comic book (POST)
        [HttpPost]
        public async Task<IActionResult> CreateComicBook(ComicBook comicBook)
        {
            if (ModelState.IsValid)
            {
                _context.ComicBooks.Add(comicBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comicBook);
        }

        // Edit comic book (GET)
        public async Task<IActionResult> EditComicBook(int id)
        {
            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook == null) return NotFound();
            return View(comicBook);
        }

        // Edit comic book (POST)
        [HttpPost]
        public async Task<IActionResult> EditComicBook(ComicBook comicBook)
        {
            if (ModelState.IsValid)
            {
                _context.ComicBooks.Update(comicBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comicBook);
        }

        // Delete comic book
        [HttpPost]
        public async Task<IActionResult> DeleteComicBook(int comicBookID)
        {
            var comicBook = await _context.ComicBooks.FindAsync(comicBookID);
            if (comicBook != null)
            {
                _context.ComicBooks.Remove(comicBook);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Customer Management

        // Register new customer (GET)
        public IActionResult RegisterCustomer()
        {
            return View();
        }

        // Register new customer (POST)
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // Rental Management

        // Create a new rental (GET)
        public IActionResult CreateRental()
        {
            ViewData["ComicBooks"] = new SelectList(_context.ComicBooks, "ComicBookID", "Title");
            ViewData["Customers"] = new SelectList(_context.Customers, "CustomerID", "Fullname");
            return View();
        }

        // Create a new rental (POST)
        [HttpPost]
        public async Task<IActionResult> CreateRental(Rental rental, List<RentalDetail> rentalDetails)
        {
            if (ModelState.IsValid)
            {
                // Add the rental to Rentals table
                _context.Rentals.Add(rental);
                await _context.SaveChangesAsync();

                // Add rental details (comic books rented)
                foreach (var detail in rentalDetails)
                {
                    detail.RentalID = rental.RentalID;
                    _context.RentalDetails.Add(detail);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(RentalIndex));
            }
            ViewData["ComicBooks"] = new SelectList(_context.ComicBooks, "ComicBookID", "Title", rentalDetails?.FirstOrDefault()?.ComicBookID);
            ViewData["Customers"] = new SelectList(_context.Customers, "CustomerID", "Fullname", rental.CustomerID);
            return View(rental);
        }

        // View the rental list based on date range
        public async Task<IActionResult> RentalIndex(DateTime startDate, DateTime endDate)
        {
            var rentals = await _context.Rentals
                .Where(r => r.RentalDate >= startDate && r.ReturnDate <= endDate)
                .Include(r => r.Customer)
                .Include(r => r.RentalDetails)
                .ThenInclude(rd => rd.ComicBook)
                .ToListAsync();

            return View(rentals);
        }

        // Error Handling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
