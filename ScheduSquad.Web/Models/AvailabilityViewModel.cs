using ScheduSquad.Models;

namespace ScheduSquad.Web.Models;

public class AvailabilityViewModel {

    public AvailabilityViewModel(){
        availabilities = new List<Availability>();
    }

    public List<Availability> availabilities {get; set;}

}