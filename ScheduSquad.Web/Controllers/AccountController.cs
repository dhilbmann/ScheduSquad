using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ScheduSquad.Web.Models;
using ScheduSquad.Service;
using Microsoft.AspNetCore.Identity;
using ScheduSquad.Models;

namespace ScheduSquad.Web.Controllers
{

    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;
        private readonly IMemberService _memberService;
        private readonly ILoginAuthenticationService _authService;

        public AccountController(ILogger<AccountController> logger, IMemberService memberService, ILoginAuthenticationService authService)
        {
            _logger = logger;
            _memberService = memberService;
            _authService = authService;
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
            Member member = null;
            try
            {
                // Attempt to get the user from the database
                member = _memberService.GetMemberByEmail(model.Email);
            }
            catch (System.Exception ex)
            {
                 // TODO: Handle this better than swallowing the error.
            }
                                    
            // if the user is not null and if the password checks out fine... 
            if (member != null && _authService.CheckPassword(model.Password, member.Id))
            {

                // Create claims for the user
                var claims = new List<Claim>
                    {
                        // These would need to be updated with the user object properties from above
                        new Claim(ClaimTypes.Sid, member.Id.ToString()),
                        new Claim(ClaimTypes.Name, member.FirstName),
                        new Claim(ClaimTypes.Surname, member.LastName),
                        new Claim(ClaimTypes.Email, member.Email)
                    };

                // Create identity object
                var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create authentication properties
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe, // Remember user if requested
                    RedirectUri = returnUrl
                };

                // Sign in the user; stores claims in accountManager
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

        // GET: /Account/CreateAccount
        public IActionResult Create()
        {
            CreateAccountViewModel vm = new CreateAccountViewModel();
            return View(vm);
        }

        // POST: /Account/CreateAccount
        [HttpPost]
        public IActionResult Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if passwords match
                if (model.Password != model.ConfirmPassword)
                {
                    ViewData["ErrorMessage"] = "Passwords do not match.";
                    return View(model);
                }

                // Perform account creation logic here (e.g., save to database)
                _memberService.AddMember(model.FirstName, model.LastName, model.Email, model.Password);

                return RedirectToAction("Login", "Account");
            }

            // If model validation fails, return the view with errors
            return View(model);
        }
    }
}