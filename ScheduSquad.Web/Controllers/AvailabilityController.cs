using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Models;

namespace ScheduSquad.Web.Controllers {

    public class AvailabilityController : Controller {

        [HttpGet]
        public IActionResult Index(Guid id) {
            return View();
        }

        [HttpGet]
        public IActionResult GetAvailabilityTable(Guid memberId) {
            return PartialView("AvailabilityTable");
        }

        [HttpGet]
        public IActionResult GetMemberAvailabilities(Guid memberId) {
            return PartialView("AvailabilityTable");
        }

        public JsonResult SaveAvailability(Availability availability) {
            return new JsonResult(new {});
        }

        public JsonResult UpdateAvailability(Availability availability) {
            return new JsonResult(new {});
        }

        public JsonResult DeleteAvailability(Availability availability) {
            return new JsonResult(new {});
        }
    }

}