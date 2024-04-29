namespace ScheduSquad.Web.Models
{
    // Model for the Page
    public class SquadDetailsViewModel
    {
        public List<SquadDetailModel> Members { get; set;}
        public bool SquadBelongsToUser { get; set; }
        public List<List<int>> AvailabilityLists { get; set; }
        public List<String> AvailabilityStrings { get; set; }
        public Guid SquadId { get; set; }
        public Guid UserId { get; set; }
        public String SquadName { get; set; }
    }

    // Model for the individual squad to show on the FindSquad Page
    public class SquadDetailModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public int AvailabilityCount { get; set; }
        public bool IsSquadmaster { get; set; }
    }
}