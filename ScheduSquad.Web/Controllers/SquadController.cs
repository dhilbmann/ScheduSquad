using Microsoft.AspNetCore.Mvc;

namespace ScheduSquad.Web.Controllers {

    public class SquadController : Controller {

   private readonly ILogger<SquadController> _logger;

    public SquadController(ILogger<SquadController> logger)
    {
        _logger = logger;
    }


        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpGet]
        public IActionResult Details(Guid squadId) {
            return View("Details");
        }

        [HttpGet]
        public IActionResult SquadMemberList(Guid squadId) {
            return View("Details");
        }

        [HttpGet]
        public IActionResult SquadAvailabilityList(Guid squadId) {
            return View("Details");
        }

        [HttpPost]
        public JsonResult AddMemberToSquad(Guid memberId, Guid squadId) {
            return new JsonResult(new { memberId,}) { };
        }
        
        [HttpPost]
        public JsonResult RemoveMemberFromSquad(Guid memberId, Guid squadId) {
            return new JsonResult(new { memberId,}) { };
        }
    }

}