using ScheduSquad.Models;

namespace ScheduSquad.Web.Models;

public class HomeViewModel
{
    public Squad Squad {get; set;} 

    public HomeViewModel() {
        Squad = new Squad();
    }

}