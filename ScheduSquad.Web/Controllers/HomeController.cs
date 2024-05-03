using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Web.Models;
using ScheduSquad.Models;
using ScheduSquad.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ScheduSquad.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IAvailabilityService _availabilityService;
    private readonly ISquadService _squadService;
    private readonly IMemberService _memberService;

    public HomeController(ILogger<HomeController> logger, IAvailabilityService availabilityService, ISquadService squadService, IMemberService memberService)
    {
        _availabilityService = availabilityService;
        _squadService = squadService;
        _memberService = memberService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.Sid));
        HomeViewModel vm = new HomeViewModel();
        Member m = _memberService.GetMemberById(userId);
        vm.Name = String.Format("{0} {1}", m.FirstName, m.LastName);
        vm.MyAvailabilities = _availabilityService.GetAllAvailabilitiesBelongingToMember(userId);
        vm.MySquads = _squadService.GetAllSquadsBelongingToMember(userId);
        return View(vm);
    }

    [HttpGet]
    public IActionResult Menu()
    {
        return View();
    }
}
