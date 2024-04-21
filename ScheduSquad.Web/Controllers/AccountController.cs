using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ScheduSquad.Web.Models;

namespace ScheduSquad.Web.Controllers
{

    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/Availability")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/Availability")
        {
            // Validate user credentials
            if (model.Username == "admin@email.com" && model.Password == "password")
            {
                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                };

                // Create identity object
                var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create authentication properties
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe, // Remember user if requested
                    RedirectUri = returnUrl
                };

                // Sign in the user
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(userIdentity),
                    authProperties);

                // Redirect the user to the return URL or home page
                return LocalRedirect(returnUrl);
            }

            // If login fails, return to the login page with an error message
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to the home page or login page
            return RedirectToAction("Index", "Home");
        }
    }
}