using ScheduSquad.Models;

namespace ScheduSquad.Web.Models;

public class HomeViewModel
{
    public Squad Squad {get; set;} 
    public string SquadServiceTest {get; set;}
    public string AvailabilityServiceTest {get; set;}
    public string MemberServiceTest {get; set;}

    public List<Availability> availabilities{get; set;}
    public HomeViewModel() {
        Squad = new Squad();
    }
    public List<Member> members {get; set;}
    public Member member {get; set;}
    public Squad squad {get; set;}
    public List<Squad> squads {get; set;}

}