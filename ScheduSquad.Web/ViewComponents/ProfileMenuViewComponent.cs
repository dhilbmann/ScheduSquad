using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Models;
using ScheduSquad.Service;
using ScheduSquad.Web.Models;


namespace ScheduSquad.Web.ViewComponents
{
    public class ProfileMenuViewComponent : ViewComponent
    {
        private IMemberService _memberService;
        public ProfileMenuViewComponent(IMemberService memberService) {
            _memberService = memberService;
        } 

        public IViewComponentResult Invoke() {
            Guid userGuid = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.Sid));
            
            Member loggedInUser = _memberService.GetMemberById(userGuid);

            return View(loggedInUser);
        }
    }
}