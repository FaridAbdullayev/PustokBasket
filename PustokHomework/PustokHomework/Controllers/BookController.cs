using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokHomework.Data;
using PustokHomework.Models;

namespace PustokHomework.Controllers
{
    public class BookController : Controller
    {
        private readonly AppDbContext _context;

        public BookController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IActionResult GetBookById(int id)
        {
            Book book = _context.Books
                .Include(x=>x.Genre)
                .Include(x=>x.Images
                .Where(x=>x.PosterStatus == true))
                .FirstOrDefault(x=>x.Id == id);
            return PartialView("_BookModelPartial",book);
        }
    }
}
