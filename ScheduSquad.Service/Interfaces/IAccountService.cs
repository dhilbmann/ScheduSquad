using ScheduSquad.Models;

namespace ScheduSquad.Service
{

    public interface IMemberService
    {
        public Member GetMemberById(Guid memberId);
        public Member GetMemberByEmail(string email);
       public List<Member> GetAllMembers();
        public List<Member> GetAllMembersNotInSquad(Guid squadId) ;
        public List<Member> GetAllMembersInSquad(Guid squadId);
        public Guid AddMember(string firstName, string lastName, string email, string password);
        public void UpdateMember(Member member);
        public void DeleteMember(Member member);
        public DateTime GetJoinedDateForSquadMember (Guid memberId, Guid squadId);
    }
}





