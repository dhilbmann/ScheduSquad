using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ScheduSquad.Web.Models;
using ScheduSquad.Models;

namespace ScheduSquad.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        HomeViewModel vm = new HomeViewModel();

        User user_david = new User(Guid.NewGuid(), "David", "Hilbmann", "david@gmail.com", "password", new List<Availability>());
        user_david.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(4, 15, 00), new TimeSpan(5, 30, 00)));
        user_david.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(7, 30, 00), new TimeSpan(13, 30, 00)));
        user_david.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(8, 00, 00), new TimeSpan(9, 00, 00)));
        user_david.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00))); 
        vm.Squad.AddUser(user_david);

        User user_cara = new User(Guid.NewGuid(), "Cara", "Perez", "cara@gmail.com", "password", new List<Availability>());
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Sunday, new TimeSpan(8, 00, 00), new TimeSpan(9, 00, 00)));
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Sunday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00)));
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(8, 00, 00), new TimeSpan(9, 00, 00)));
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00)));
        user_cara.Availabilities.Add(new Availability(DayOfWeek.Tuesday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00))); 
        vm.Squad.AddUser(user_cara);

        User user_duncan = new User(Guid.NewGuid(), "Duncan", "Clark", "duncan@gmail.com", "password", new List<Availability>());
        user_duncan.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(8, 00, 00), new TimeSpan(9, 00, 00)));
        user_duncan.Availabilities.Add(new Availability(DayOfWeek.Monday, new TimeSpan(16, 45, 00), new TimeSpan(18, 30, 00))); 
        vm.Squad.AddUser(user_duncan);

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
