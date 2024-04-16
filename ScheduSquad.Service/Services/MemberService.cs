using ScheduSquad.Models;

namespace ScheduSquad.Service {
    
    public class MemberService : IMemberService 
    {

        private readonly IRepository<Member> _memberRepository;

        public MemberService(IRepository<Member> memberRepo)
        {
            _memberRepository = memberRepo;
        }

        public string Test() {
            return _memberRepository.Test();
        }

        public Member GetMemberById(Guid memberId)
        {
            return _memberRepository.GetById(memberId);
        }

        public List<Member> GetAllMembers()
        {
            return _memberRepository.GetAll().ToList();
        }

        public List<Member> GetAllMembersNotInSquad(Guid squadId) {
            throw new NotImplementedException();
        }

        public List<Member> GetAllMembersInSquad(Guid squadId) {

            return _memberRepository.GetAllByParentId(squadId).ToList();
        }

        public void AddMember(Member member)
        {
            _memberRepository.Add(member);
        }

        public void UpdateMember(Member member)
        {
            _memberRepository.Update(member);
        }

        public void DeleteMember(Member member)
        {
            _memberRepository.Delete(member);
        }



    }

}

