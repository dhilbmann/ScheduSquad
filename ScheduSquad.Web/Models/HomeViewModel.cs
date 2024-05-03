using ScheduSquad.Models;

namespace ScheduSquad.Web.Models;

public class HomeViewModel
{
    public string Name { get; set; }

    public List<Squad> MySquads { get; set; }
    public List<Availability> MyAvailabilities{ get; set; }
    public HomeViewModel() {
        MySquads = new List<Squad>();
        MyAvailabilities = new List<Availability>();
        Name = String.Empty;
    }

}