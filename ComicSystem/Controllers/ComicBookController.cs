using Microsoft.AspNetCore.Mvc;
using ComicSystem.Data;
using ComicSystem.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Controllers
{
    public class ComicBookController : Controller
    {
        private readonly ComicSystemDbContext _context;

        public ComicBookController(ComicSystemDbContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách
        public async Task<IActionResult> Index()
        {
            return View(await _context.ComicBooks.ToListAsync());
        }

        // Tạo mới
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComicBook comicBook)
        {
            if (ModelState.IsValid)
            {
                _context.ComicBooks.Add(comicBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comicBook);
        }

        // Sửa
        public async Task<IActionResult> Edit(int id)
        {
            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook == null) return NotFound();
            return View(comicBook);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ComicBook comicBook)
        {
            if (ModelState.IsValid)
            {
                _context.ComicBooks.Update(comicBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comicBook);
        }

        // Xóa
        public async Task<IActionResult> Delete(int id)
        {
            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook != null)
            {
                _context.ComicBooks.Remove(comicBook);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
