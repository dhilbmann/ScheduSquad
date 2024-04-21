using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Models;
using ScheduSquad.Service;
using ScheduSquad.Web.Models;


namespace ScheduSquad.Web.ViewComponents
{
    public class SquadMenuViewComponent : ViewComponent
    {
        private readonly ISquadService _squadService;
        public SquadMenuViewComponent(ISquadService squadService) {
            _squadService = squadService;
        } 

        public IViewComponentResult Invoke() {
            List<SquadMenuItem> squads = new List<SquadMenuItem>();

            Guid userGuid; // Id of LoggedIn User
            if (Guid.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.Sid), out userGuid))

                foreach (Squad s in _squadService.GetAllSquadsBelongingToMember(userGuid))
                {
                    squads.Add(new SquadMenuItem()
                    {
                        Id = s.Id.ToString(),
                        SquadName = s.Name,
                        IsSquadmaster = (s.SquadMaster.Id == userGuid)
                    });
                }
            return View(squads);
        }
    }
}