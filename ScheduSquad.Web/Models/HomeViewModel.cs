using ScheduSquad.Models;

namespace ScheduSquad.Web.Models;

public class HomeViewModel
{
    public Squad Squad {get; set;} 
    public string SquadServiceTest {get; set;}
    public string AvailabilityServiceTest {get; set;}
    public string MemberServiceTest {get; set;}
    public HomeViewModel() {
        Squad = new Squad();
    }

}