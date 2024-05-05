using System.Security.Claims;
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
            AvailabilityViewModel vm = new AvailabilityViewModel();

            Guid userGuid; // Id of LoggedIn User
            // Validates the Id stored on the HttpContext.User object's claim, and stored the Guid in userGuid
            if (Guid.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.Sid), out userGuid))
            {
                vm.availabilities = _availabilityService.GetAllAvailabilitiesBelongingToMember(userGuid);
            }

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
            Availability availability = new Availability();

            if (availabilityId != new Guid("00000000-0000-0000-0000-000000000000"))
            {
                availability = _availabilityService.GetAvailabilityById(availabilityId);//return from service
            }
            else 
            {
                availability.Id = Guid.NewGuid();
            }

            EditRowViewModel vm = new EditRowViewModel();
            vm.StartTime = availability.StartTime;
            vm.EndTime = availability.EndTime;
            vm.AvailabilityId = availability.Id;
            vm.DayOfWeek = (int)availability.DayOfWeek;
            
            return PartialView("AvailabilityEditRow",vm);
        }

        [HttpPost]
        public IActionResult SaveAvailability(Availability availability)
        {
            if (availability.StartTime <= availability.EndTime)
            {
                if (_availabilityService.GetAllAvailabilities().Any(x=> x.Id == availability.Id))
                {
                    UpdateAvailability(availability);
                }
                else
                {
                    Guid userGuid; // Id of LoggedIn User
                    // Validates the Id stored on the HttpContext.User object's claim, and stored the Guid in userGuid
                    if (Guid.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.Sid), out userGuid)){
                        _availabilityService.AddAvailability(availability, userGuid);
                        return RedirectToAction("Index", "Home"); 
                    }
                }
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
        public IActionResult EditAvailability(Guid id)
        {
            //await HttpContext.
            return RedirectToAction("Index","Availability");

        }
    }

}