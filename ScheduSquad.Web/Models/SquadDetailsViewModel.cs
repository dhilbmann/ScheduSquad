using ScheduSquad.Models;

namespace ScheduSquad.Web.Models

public class SquadDetailsViewModel
{
    public SquadViewModel()
    {
        members = new List<Member>();
    }

    public List<Member> members {get; set;}
}