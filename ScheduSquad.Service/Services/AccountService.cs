using ScheduSquad.Models;

namespace ScheduSquad.Service {
    
    public class AccountService
    {

        private readonly IRepository<Member> _memberRepository;

        public AccountService(IRepository<Member> memberRepo)
        {
            _memberRepository = memberRepo;
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
