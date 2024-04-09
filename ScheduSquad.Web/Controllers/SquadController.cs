using Microsoft.AspNetCore.Mvc;

namespace ScheduSquad.Web.Controllers {

    public class SquadController : Controller {

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Details(Guid squadId) {
            return View("Details");
        }

        [HttpPost]
        public JsonResult GetMySquads(Guid memberId) {
            return new JsonResult(new { });
        }


    }

}