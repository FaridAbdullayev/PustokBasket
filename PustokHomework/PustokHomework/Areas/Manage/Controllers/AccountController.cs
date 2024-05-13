using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PustokHomework.Areas.Manage.ViewModels;
using PustokHomework.Models;


namespace PustokHomework.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> CreateAdmin()
        {
            AppUser appUser = new AppUser
            {
                UserName = "admin"
            };

            var r = await _userManager.CreateAsync(appUser,"admin123");
            await _userManager.AddToRoleAsync(appUser, "super_admin");
            return Json(r);
        }



        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model , string returnUrl)
        {
            AppUser appUser = await _userManager.FindByNameAsync(model.UserName);

            if (appUser == null || (!await _userManager.IsInRoleAsync(appUser, "admin") && !await _userManager.IsInRoleAsync(appUser, "super_admin")))
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, model.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View();
            }


            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("index", "dashboard");
        }


        public async Task<IActionResult> CreateRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole("super_admin"));
            await _roleManager.CreateAsync(new IdentityRole("admin"));
            await _roleManager.CreateAsync(new IdentityRole("member"));

            return Ok();
        }

        [Authorize(Roles = "super_admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index","home" ,new { area = "" });
        }
    }
}
