using ComicSystem.Data;
using ComicSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Controllers{
    public class RentalController : Controller
{
    private readonly ComicSystemDbContext _context;

    public RentalController(ComicSystemDbContext context)
    {
        _context = context;
    }

    // Tạo mới rental (khi khách hàng thuê sách)
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Rental rental, List<RentalDetail> rentalDetails)
    {
        if (ModelState.IsValid)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            foreach (var detail in rentalDetails)
            {
                detail.RentalID = rental.RentalID;
                _context.RentalDetails.Add(detail);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        return View(rental);
    }

    // Xem danh sách thuê sách
    public async Task<IActionResult> Index(DateTime startDate, DateTime endDate)
    {
        var rentals = await _context.Rentals
            .Where(r => r.RentalDate >= startDate && r.ReturnDate <= endDate)
            .Include(r => r.Customer)
            .Include(r => r.RentalDetails)
            .ThenInclude(rd => rd.ComicBook)
            .ToListAsync();

        return View(rentals);
    }
}

}