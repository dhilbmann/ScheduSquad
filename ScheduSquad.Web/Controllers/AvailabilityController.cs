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
            id = new Guid("9F9D518F-AA15-412A-AF2B-E6D83D9DCB04");
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

        [HttpGet]
        public IActionResult GetAvailabilityReadRow(Guid availabilityId)
        {
            Availability availability = _availabilityService.GetAvailabilityById(availabilityId);//return from service
            return PartialView("AvailabilityReadRow",availability);
        }

        [HttpGet]
        public IActionResult GetAvailabilityEditRow(Guid availabilityId)
        {
            Availability availability = _availabilityService.GetAvailabilityById(availabilityId);//return from service
            return PartialView("AvailabilityEditRow",availability);
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