using System.Threading.Tasks;
using Infozdrav.Web.Data;
using Infozdrav.Web.Data.Manage;
using Infozdrav.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Infozdrav.Web.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager,
                              SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.Enabled)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

                    if (result.Succeeded)
                        return Redirect(returnUrl ?? "/"); // Success.
                    else
                        ModelState.AddModelError(nameof(LoginViewModel.Email), "Invalid email or password");
                }
                else
                    ModelState.AddModelError(nameof(LoginViewModel.Email), "Invalid email or password");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model); // Failure
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}