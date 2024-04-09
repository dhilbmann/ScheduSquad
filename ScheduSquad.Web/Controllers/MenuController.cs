using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Web.Models;
using ScheduSquad.Models;

namespace ScheduSquad.Web.Controllers;

public class MenuController : Controller
{
    private readonly ILogger<MenuController> _logger;

    public MenuController(ILogger<MenuController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        HomeViewModel vm = new HomeViewModel();

        Member user_david = new Member(Guid.NewGuid(), "David", "Hilbmann", "david@gmail.com", "password", new List<Availability>());
        user_david.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(4, 15, 00), new TimeSpan(5, 30, 00)));
        user_david.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(7, 30, 00), new TimeSpan(13, 30, 00)));
        user_david.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(8, 00, 00), new TimeSpan(9, 00, 00)));
        user_david.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00)));
        vm.Squad.AddMember(user_david);

        Member user_cara = new Member(Guid.NewGuid(), "Cara", "Perez", "cara@gmail.com", "password", new List<Availability>());
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Sunday, new TimeSpan(8, 00, 00), new TimeSpan(9, 00, 00)));
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Sunday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00)));
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(8, 00, 00), new TimeSpan(9, 00, 00)));
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00)));
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Tuesday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00)));
        vm.Squad.AddMember(user_cara);

        Member user_duncan = new Member(Guid.NewGuid(), "Duncan", "Clark", "duncan@gmail.com", "password", new List<Availability>());
        user_duncan.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(8, 00, 00), new TimeSpan(9, 00, 00)));
        user_duncan.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00)));
        vm.Squad.AddMember(user_duncan);

        return View(vm);
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
