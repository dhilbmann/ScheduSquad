using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Web.Models;
using ScheduSquad.Models;
using ScheduSquad.Service;
using Microsoft.AspNetCore.Authorization;

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
        return View();
    }

    [HttpGet]
    public IActionResult Menu()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ProfileMenu()
    {
        return View();
    }

    [HttpGet]
    public IActionResult SquadMenu()
    {
        return View();
    }

}
