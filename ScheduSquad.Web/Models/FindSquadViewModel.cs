namespace ScheduSquad.Web.Models
{
    // Model for the Page
    public class FindSquadViewModel
    {
        public List<FindSquadModel> Squads { get; set;}
    }

    // Model for the individual squad to show on the FindSquad Page
    public class FindSquadModel
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int MemberCount { get; set; }
    }



}