using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.Enums;
using UrlShortener.Core.Models.Identity;
using UrlShortener.Data.ViewModels;

namespace UrlShortener.Web.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (_roleManager.Roles.Count() == 0)
            {
                var role1 = new Role();
                role1.Name = UserRolesEnum.Admin.ToString();
                await _roleManager.CreateAsync(role1);
                var role2 = new Role();
                role2.Name = UserRolesEnum.User.ToString();
                await _roleManager.CreateAsync(role2);
            }
            if (_userManager.Users.Count() == 0)
            {
                await _userManager.CreateAsync(new User() { UserName = "admin"}, "1234Admin@");
                await _userManager.AddToRoleAsync(_userManager.FindByNameAsync("admin").Result, "Admin");
                await _userManager.CreateAsync(new User() { UserName = "user" }, "1234User@");
                await _userManager.AddToRoleAsync(_userManager.FindByNameAsync("user").Result, "User");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Login, loginViewModel.Password, false, false);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(URLsController.Index), "URLs");
            }
            ModelState.AddModelError("Login", "Invalid login or password!");
            return View(loginViewModel);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(URLsController.Index), "URLs");
        }
    }
}
