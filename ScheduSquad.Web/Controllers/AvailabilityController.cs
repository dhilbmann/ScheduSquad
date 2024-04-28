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
        public IActionResult SaveAvailability(Availability availability, Guid id)
        {
            if (_availabilityService.GetAllAvailabilities().Any(x=> x.Id == availability.Id))
            {
                UpdateAvailability(availability);
            }
            else
            {
                _availabilityService.AddAvailability(availability, new Guid("713626F9-D78E-4E68-8548-AEB073A22C63"));
            }
            return RedirectToAction("Index","Availability");
        }

        [HttpPost]
        public IActionResult UpdateAvailability(Availability availability)
        {
            _availabilityService.UpdateAvailability(availability);
            return RedirectToAction("Index","Availability");
        }

        [HttpPost]
        public IActionResult DeleteAvailability(Availability availability)
        {
            _availabilityService.DeleteAvailability(availability);
            return RedirectToAction("Index","Availability");
        }

        [HttpPost]
        public async Task<IActionResult> EditAvailability(Guid id)
        {
            //await HttpContext.
            return RedirectToAction("Index","Availability");

        }
    }

}