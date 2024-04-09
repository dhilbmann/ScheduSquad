using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Models;

namespace ScheduSquad.Web.Controllers
{

    public class AvailabilityController : Controller
    {

        private readonly ILogger<AvailabilityController> _logger;

        public AvailabilityController(ILogger<AvailabilityController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(Guid id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAvailabilityTable(Guid memberId)
        {
            return PartialView("AvailabilityTable");
        }

        [HttpGet]
        public IActionResult GetMemberAvailabilities(Guid memberId)
        {
            return PartialView("AvailabilityTable");
        }

        [HttpPost]
        public JsonResult SaveAvailability(Availability availability)
        {
            return new JsonResult(new { });
        }

        [HttpPost]
        public JsonResult UpdateAvailability(Availability availability)
        {
            return new JsonResult(new { });
        }

        [HttpPost]
        public JsonResult DeleteAvailability(Availability availability)
        {
            return new JsonResult(new { });
        }
    }

}