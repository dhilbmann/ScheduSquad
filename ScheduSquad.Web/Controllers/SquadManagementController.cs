using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScheduSquad.DataAccess;
using ScheduSquad.Models;
using ScheduSquad.Service;
using ScheduSquad.Web.Models;

namespace ScheduSquad.Web.Controllers
{

    [Authorize]
    public class SquadManagement : Controller
    {

        private readonly ILogger<SquadController> _logger;
        private readonly ISquadService _squadService;
        private readonly IMemberService _memberService;
        private readonly IAvailabilityService _availabilityService;


        public SquadManagement(ILogger<SquadController> logger, ISquadService squadService, IMemberService memberService, IAvailabilityService availabilityService)
        {
            _logger = logger;
            _squadService = squadService;
            _memberService = memberService;
            _availabilityService = availabilityService;
        }

        // GET: /SquadManagement/Members
        public IActionResult Members(Guid squadId)
        {
            // Instantiate a view model for the page
            ManageSquadMembersViewModel vm = new ManageSquadMembersViewModel();

            // Get the squad for the page
            Squad thisSquad = _squadService.GetSquadById(squadId);
            // Set some properties on the VM from the squad
            vm.SquadId = squadId;
            vm.SquadMaster = thisSquad.SquadMaster;
            vm.SquadName = thisSquad.Name;

            //Need to get all the members not in the squad
            List<Member> membersNotInSquad = _memberService.GetAllMembersNotInSquad(thisSquad.Id);
            List<Member> membersInSquad = thisSquad.Members;
            // Convert members in/not in squad into MemberDetailsForSquad objects and assign them to their properties on the vm
            vm.MembersInSquad = MapToViewModels(membersInSquad, squadId, true); // true because we need joined date
            vm.MembersNotInSquad = MapToViewModels(membersNotInSquad, squadId, false); // false because we don't need joined date

            return View(vm);
        }

        // Used to turn Member objects into MemberDetailsForSquad.
        private List<MemberDetailsForSquad> MapToViewModels(List<Member> members, Guid squadId, bool inSquad)
        {
            // Instantiate list
            List<MemberDetailsForSquad> list = new List<MemberDetailsForSquad>();
            // Itterate over members
            foreach (Member m in members)
            {
                // TODO: Bad code.  Too many calls to the database and it needs to do this for EVERY USER in the system.  Make stored proc later.
                // Get list of squads the user belongs to
                List<Squad> squads = _squadService.GetAllSquadsBelongingToMember(m.Id);
                // Get count of squads
                int squadCount = squads.Count();
                // Get count of availabilities
                int availabilityCount = _availabilityService.GetAllAvailabilitiesBelongingToMember(m.Id).Count();
                // Build object and add to list
                list.Add(new MemberDetailsForSquad
                {
                    Id = m.Id,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Email = m.Email,
                    JoinedDate = (inSquad) ? _memberService.GetJoinedDateForSquadMember(m.Id, squadId) : DateTime.MinValue,
                    AvailabilityCount = availabilityCount,
                    SquadCount = squadCount
                });
            }

            return list;
        }

        public IActionResult AddMember(Guid memberId, Guid squadId)
        {
            try
            {
                _squadService.AddMemberToSquad(memberId, squadId, false);
                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = String.Format("Failed to add member to squad.  {0}", ex.Message) });
            }

        }

         public IActionResult RemoveMember(Guid memberId, Guid squadId)
        {
            try
            {
                _squadService.RemoveMemberFromSquad(memberId, squadId);
                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, message = String.Format("Failed to remove member from squad.  {0}", ex.Message) });
            }

        }

    }

}


