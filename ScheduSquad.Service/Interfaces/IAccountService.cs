using ScheduSquad.Models;

namespace ScheduSquad.Service
{

    public interface IMemberService
    {
        public Member GetMemberById(Guid memberId);
       public List<Member> GetAllMembers();
        public List<Member> GetAllMembersNotInSquad(Guid squadId) ;
        public List<Member> GetAllMembersInSquad(Guid squadId);
        public void AddMember(Member member);
        public void UpdateMember(Member member);
        public void DeleteMember(Member member);
        public string Test();

    }
}





