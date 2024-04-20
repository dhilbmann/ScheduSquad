using System.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Models;
using ScheduSquad.Service;
using ScheduSquad.Web.Models;

namespace ScheduSquad.Web.Controllers
{

    [Authorize]
    public class SquadController : Controller
    {

        private readonly ILogger<SquadController> _logger;
        private readonly ISquadService _squadService;
        private readonly IMemberService _memberService;

        public SquadController(ILogger<SquadController> logger, ISquadService squadService, IMemberService memberService)
        {
            _logger = logger;
            _squadService = squadService;
            _memberService = memberService;
        }

        // GET: /Squad/CreateSquad
        public IActionResult CreateSquad()
        {
            CreateSquadViewModel vm = new CreateSquadViewModel();
            return View("Create", vm);
        }

        // POST: /Squad/CreateSquad
        [HttpPost]
        public IActionResult CreateSquad(CreateSquadViewModel model)
        {
            // If there aren't any errors on the model
            if (ModelState.IsValid)
            {
                Guid userGuid; // Id of LoggedIn User
                // Validates the Id stored on the HttpContext.User object's claim, and stored the Guid in userGuid
                if (Guid.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.Sid), out userGuid))
                {
                    // Attempt to add squad via service
                    _squadService.AddSquad(userGuid, model.SquadName, model.Description, model.Location);
                    // Redirect to squad details page
                    return RedirectToAction("Index", "Home"); // TODO: Redirect to Squad Details Page when built.
                }
                else
                {
                    // Didn't get a valid memberId from HttpContext.  Fail creation.
                    ModelState.AddModelError(String.Empty, "Unable to create squad.  (Unable to get SquadMaster Id)");
                }
            }

            // If model validation fails, return the view with errors
            return View("Create", model);
        }

        // GET: /Squad/FindSquad
        public IActionResult FindSquad()
        {
            // Instantiate a new model for the page
            FindSquadViewModel vm = new FindSquadViewModel();

            Guid userGuid; // Id of LoggedIn User
            // Validates the Id stored on the HttpContext.User object's claim, and stored the Guid in userGuid
            if (Guid.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.Sid), out userGuid))
            {
                // Get all the squads and map them to the view models
                vm.Squads = MapToViewModels(_squadService.GetAllSquadsNotBelongingToMember(userGuid));
            }
            else
            {
                // Couldn't get the user id, so we couldn't find squads.
                vm.Squads = new List<FindSquadModel>();
                ModelState.AddModelError(String.Empty, "Unable to get list of squads.  (Could not get member id)");
            }
            // return page with the list of squads
            return View(vm);
        }

        // POST: /Squad/JoinSquad
        [HttpPost]
        public IActionResult JoinSquad(Guid squadId)
        {
            try
            {
                // TODO: We don't have a service/repo for putting members in/out of squads!  :[
                // _squadService.AddMemberToSquad()
                return Json(new { success = true });
            }
            catch (Exception)
            {
                // Failed to join squad
                return Json(new { success = false });
            }
        }


        // Helper method to map List of Squad objects to the shortened model that is used on the page
        // (Doing this because the Squad knows about Members and SquadLeader, but this info isn't needed
        // on the page.)
        private List<FindSquadModel> MapToViewModels(List<Squad> squads)
        {
            // 
            List<FindSquadModel> list = new List<FindSquadModel>();
            foreach (Squad s in squads)
            {
                list.Add(new FindSquadModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Location = s.Location,
                    Description = s.Description,
                    MemberCount = s.Members.Count,
                });
            }

            return list;
        }
    }

}
