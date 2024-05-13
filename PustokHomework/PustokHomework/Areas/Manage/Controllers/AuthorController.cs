using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokHomework.Areas.Manage.ViewModels;
using PustokHomework.Data;
using PustokHomework.Models;

namespace PustokHomework.Areas.Manage.Controllers
{
    [Authorize(Roles = "admin,super_admin")]
    [Area("manage")]
    public class AuthorController : Controller
    {
        private readonly AppDbContext _context;
        public AuthorController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IActionResult Index(int page=1)
        {
            var query = _context.Author.Include(x => x.Books);
            return View(PaginatedList<Author>.Create(query, page, 2));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Author author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }
            if (_context.Genres.Any(x => x.Name == author.FullName))
            {
                ModelState.AddModelError("Name", "Genre already exists!");
                return View(author);
            }
            _context.Author.Add(author);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
