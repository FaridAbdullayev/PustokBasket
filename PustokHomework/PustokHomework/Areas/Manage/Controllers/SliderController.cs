using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok.Helpers;
using PustokHomework.Areas.Manage.ViewModels;
using PustokHomework.Data;
using PustokHomework.Models;

namespace PustokHomework.Areas.Manage.Controllers
{
    [Authorize(Roles = "admin,super_admin")]
    [Area("manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = appDbContext;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Sliders;
            return View(PaginatedList<Slider>.Create(query, page, 3));
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (slider.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required !");
                return View();
            }
            if(!ModelState.IsValid)
            {
                return View();
            }
            slider.Image = FileManager.Save(slider.ImageFile, _webHostEnvironment.WebRootPath, "uploads/slider");
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public IActionResult Edit(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.Id == id);

            if (slider == null) return RedirectToAction("Error", "NotFound");

            return View(slider);
        }

        [HttpPost]
        public IActionResult Edit(Slider slider) 
        { 
            if(!ModelState.IsValid) return View();

            Slider existSlider = _context.Sliders.Find(slider.Id);
            if(existSlider == null) return RedirectToAction("Error", "NotFound");


            string deletedFile = null;
            if (slider.ImageFile != null)
            {
                deletedFile = existSlider.Image;
                existSlider.Image = FileManager.Save(slider.ImageFile, _webHostEnvironment.WebRootPath, "uploads/slider");

            }

            existSlider.Title1 = slider.Title1;
            existSlider.Title2 = slider.Title2;
            existSlider.Description = slider.Description;
            existSlider.BtnText = slider.BtnText;
            existSlider.BtnUrl = slider.BtnUrl;
            existSlider.Order = slider.Order;
            
            if (deletedFile != null)
            {
                FileManager.Delete(_webHostEnvironment.WebRootPath, "uploads/slider", deletedFile);
            }
            _context.SaveChanges();

            return RedirectToAction("index");
        }



       
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(m => m.Id == id);

            if (slider is null) return RedirectToAction("Error", "NotFound");

            string deletedFile = slider.Image;

            FileManager.Delete(_webHostEnvironment.WebRootPath, "uploads/slider", deletedFile);

            _context.Sliders.Remove(slider);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
