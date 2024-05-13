using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using PustokHomework.Data;
using PustokHomework.Models;
using PustokHomework.ViewModel;
using System.Diagnostics;
using System.Security.Claims;

namespace PustokHomework.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel
            {
                Sliders = _context.Sliders.ToList(),
                Features = _context.Features.ToList(),
                FeaturedBooks = _context.Books.Include(x => x.Author).Include(x => x.Images.Where(bi => bi.PosterStatus != null)).Where(x => x.IsFeatured).Take(25).ToList(),
                NewBooks = _context.Books.Include(x => x.Author).Include(x => x.Images.Where(bi => bi.PosterStatus != null)).Where(x => x.IsNew).Take(25).ToList(),
                DiscountedBooks = _context.Books.Include(x => x.Author).Include(x => x.Images.Where(bi => bi.PosterStatus != null)).Where(x => x.DiscountPercent > 0).OrderByDescending(x => x.DiscountPercent).Take(25).ToList(),
            };
            return View(model);
        }


        public IActionResult Detail(int id)
        {
            var book = _context.Books.Include(x => x.Author).Include(a => a.Genre).Include(c => c.Images.Where(x => x.PosterStatus == null)).Include(c => c.BookTags).ThenInclude(bt => bt.Tag).FirstOrDefault(x => x.Id == id);

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBasket(int id)
        {

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var existingBasketEntity = _context.Baskets.FirstOrDefault(b => b.UserId == userId && b.ProductId == id);

                if (existingBasketEntity != null)
                {
                    existingBasketEntity.Count++;
                }
                else
                {
                    Basket basketEntity = new Basket
                    {
                        UserId = userId,
                        ProductId = id,
                        Count = 1
                    };

                    _context.Baskets.Add(basketEntity);
                }

                await _context.SaveChangesAsync();
            }

            else
            {
                List<BasketCookiesViewModel> cookies = null;
                Book book = _context.Books.FirstOrDefault(x => x.Id == id);

                if (HttpContext.Request.Cookies["Courses"] != null)
                {
                    cookies = JsonConvert.DeserializeObject<List<BasketCookiesViewModel>>(HttpContext.Request.Cookies["Courses"]);
                }
                else
                {
                    cookies = new List<BasketCookiesViewModel>();
                }

                var existCookies = cookies.FirstOrDefault(x => x.ProductId == id);

                if (existCookies != null)
                {
                    existCookies.Count++;
                }
                else
                {

                    BasketCookiesViewModel basket = new BasketCookiesViewModel()
                    {
                        ProductId = book.Id,
                        Count = 1,
                    };

                    cookies.Add(basket);
                }


                HttpContext.Response.Cookies.Append("Courses", JsonConvert.SerializeObject(cookies));
            }
            return RedirectToAction("index");


        }





        public ActionResult GetCookies()
        {
            var item = HttpContext.Request.Cookies["Courses"];

            return Content(item);
        }

    }
}
