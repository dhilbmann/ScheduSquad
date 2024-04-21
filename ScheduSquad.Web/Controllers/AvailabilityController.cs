using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Models;
using ScheduSquad.Service;
using ScheduSquad.Web.Models;

namespace ScheduSquad.Web.Controllers
{

    public class AvailabilityController : Controller
    {

        private readonly ILogger<AvailabilityController> _logger;
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(ILogger<AvailabilityController> logger, IAvailabilityService availabilityService)
        {
            _logger = logger;
            _availabilityService = availabilityService;
        }

        [HttpGet]
        public IActionResult Index(Guid id)
        {
            id = new Guid("6349C8C7-F12A-43A7-B70F-95ACBD388752");
            AvailabilityViewModel vm = new AvailabilityViewModel();

            vm.availabilities = _availabilityService.GetAllAvailabilitiesBelongingToMember(id);

            return View(vm);
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

        [HttpPost]
        public async Task<IActionResult> EditAvailability(Guid id)
        {
            //await HttpContext.
            return RedirectToAction("Index","Availability");

        }
    }

}