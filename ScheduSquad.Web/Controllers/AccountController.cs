using Microsoft.AspNetCore.Mvc;

namespace ScheduSquad.Web.Controllers {

    public class AccountController : Controller {

  private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password) {
            return new JsonResult(new { });
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string firstName, string lastName, string email, string password) {
            return new JsonResult(new { });
        }

        [HttpPost]
        public IActionResult Logout() {
            return RedirectToAction("Login");
        }

    }

}