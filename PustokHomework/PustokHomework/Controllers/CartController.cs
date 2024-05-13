using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokHomework.Data;
using PustokHomework.Models;
using PustokHomework.ViewModel;
using System.Net;
using System.Security.Claims;

namespace PustokHomework.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        public CartController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                List<BasketCookiesViewModel> basketProduct = new();

                if (HttpContext.Request.Cookies["Courses"] != null)
                {
                    basketProduct = JsonConvert.DeserializeObject<List<BasketCookiesViewModel>>(HttpContext.Request.Cookies["Courses"]);
                }
                else
                {
                    basketProduct = new List<BasketCookiesViewModel>();
                }
                List<BasketDetailViewModel> basketDetails = new();


                foreach (var item in basketProduct)
                {
                    var dbProduct = await _context.Books.Include(x => x.Images).FirstOrDefaultAsync(v => v.Id == item.ProductId);

                    basketDetails.Add(new BasketDetailViewModel
                    {
                        Id = dbProduct.Id,
                        Name = dbProduct.Name,
                        StockStatus = dbProduct.StockStatus,
                        Price = dbProduct.SalePrice,
                        Image = dbProduct.Images.Where(x => x.PosterStatus == true).FirstOrDefault().Name,
                        Count = item.Count,
                        Total = dbProduct.SalePrice * item.Count,

                    });
                }
                return View(basketDetails);
            }
        }

        public IActionResult DeleteBasket(int id)
        {

            List<BasketCookiesViewModel> basketProduct = JsonConvert.DeserializeObject<List<BasketCookiesViewModel>>(HttpContext.Request.Cookies["Courses"]);

            var deleteProduct = basketProduct.FirstOrDefault(x => x.ProductId == id);

            basketProduct.Remove(deleteProduct);

            HttpContext.Response.Cookies.Append("Courses", JsonConvert.SerializeObject(basketProduct));

            return RedirectToAction("index");
        }
    }
}
