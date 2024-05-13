using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PustokHomework.Areas.Manage.ViewModels;
using PustokHomework.Models;
using PustokHomework.ViewModel;
using System.Security.Claims;

namespace PustokHomework.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel memberViewModel)
        {
            if (!ModelState.IsValid) return View();


            if (_userManager.Users.Any(x => x.NormalizedEmail == memberViewModel.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "Email is already taken");
                return View();
            }


            AppUser appUser = new()
            {
                UserName = memberViewModel.UserName,
                Email = memberViewModel.Email,
                FullName = memberViewModel.FullName,

            };

            var result = await _userManager.CreateAsync(appUser ,memberViewModel.Password);

            if(!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    if (err.Code == "DuplicateUserName")
                        ModelState.AddModelError("UserName", "UserName is already taken");
                    else ModelState.AddModelError("", err.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(appUser, "member");

            return RedirectToAction("index", "home");

        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel model)
        {
            AppUser? appUser = await _userManager.FindByNameAsync(model.UserName);
            if (appUser == null || !await _userManager.IsInRoleAsync(appUser, "member"))
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
            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "member")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }




        [Authorize(Roles = "member")]
        public async Task<IActionResult> MyAccount(string tab="dashboard")
        {
            var user = await _userManager.GetUserAsync(User);
           
            if (user == null)
            {
                return RedirectToAction("login", "account");
            }
            ProfileViewModel profileViewModel = new()
            {
                ProfileEditVM = new ProfileEditViewModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = user.UserName,
                }
            };

            ViewBag.Tab = tab;

            return View(profileViewModel);
        }


        [Authorize(Roles = "member")]
        [HttpPost]
        public async Task<IActionResult> MyAccount(ProfileEditViewModel profileEditViewModel ,string tab="profile")
        {
            ViewBag.Tab = tab;

            ProfileViewModel profileVM = new ProfileViewModel();
            profileVM.ProfileEditVM = profileEditViewModel;

            if (!ModelState.IsValid) return View(profileVM);

            AppUser? user = await _userManager.GetUserAsync(User);

            if (user == null) return RedirectToAction("login", "account");

            user.UserName = profileEditViewModel.UserName;
            user.Email = profileEditViewModel.Email;
            user.FullName = profileEditViewModel.FullName;

			if (_userManager.Users.Any(x => x.Id != User.FindFirstValue(ClaimTypes.NameIdentifier) && x.NormalizedEmail == profileEditViewModel.Email.ToUpper()))
			{
				ModelState.AddModelError("Email", "Email is already taken");
				return View(profileVM);
			}

			if (profileEditViewModel.NewPassword != null)
			{
				var passwordResult = await _userManager.ChangePasswordAsync(user, profileEditViewModel.CurrentPassword, profileEditViewModel.NewPassword);

				if (!passwordResult.Succeeded)
				{
					foreach (var err in passwordResult.Errors)
						ModelState.AddModelError("", err.Description);

					return View(profileVM);
				}
			}
			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
			{
				foreach (var err in result.Errors)
				{
					if (err.Code == "DuplicateUserName")
						ModelState.AddModelError("UserName", "UserName is already taken");
					else ModelState.AddModelError("", err.Description);
				}
				return View(profileVM);
			}

			await _signInManager.SignInAsync(user, false);


			return View(profileVM);
        }





    }
}
