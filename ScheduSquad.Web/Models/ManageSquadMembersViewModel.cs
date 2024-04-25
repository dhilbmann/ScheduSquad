using ScheduSquad.Models;

namespace ScheduSquad.Web.Models
{
    /// <summary>
    /// Has properties for displaying information about who (and who isn't) in a squad.
    /// </summary>
    public class ManageSquadMembersViewModel
    {
        public Guid SquadId { get; set; }
        public string SquadName { get; set; }
        public string SquadDescription { get; set; }
        public Member SquadMaster { get; set; }
        public List<MemberDetailsForSquad> MembersInSquad { get; set; }

        public List<MemberDetailsForSquad> MembersNotInSquad { get; set; }

        public ManageSquadMembersViewModel()
        {
            // this.MembersInSquad = new List<MemberDetailsForSquad>();
            // this.MembersNotInSquad = new List<MemberDetailsForSquad>();
            // this.SquadMaster = new Member();
        }
    }

    /// <summary>
    /// Has properties needed for displaying information in tables on the manage squad page
    /// </summary>
    public class MemberDetailsForSquad
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime JoinedDate { get; set; }
        public int AvailabilityCount { get; set; }
        public int SquadCount { get; set; }

    }
}