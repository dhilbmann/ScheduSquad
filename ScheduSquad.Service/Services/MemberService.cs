using System.Security.Cryptography;
using ScheduSquad.DataAccess;
using ScheduSquad.Models;

namespace ScheduSquad.Service {
    
    public class MemberService : IMemberService 
    {

        private readonly IRepository<Member> _memberRepository;
        private readonly ILoginAuthenticationService _authenticationService;
        private readonly IMembersForSquadRepository _memberForSquadRepo;
        private readonly IAvailabilityService _availabilityService;
        public MemberService(IRepository<Member> memberRepo, ILoginAuthenticationService authService, IMembersForSquadRepository membersForSquadRepo, IAvailabilityService availabilityService)
        {
            _memberRepository = memberRepo;
            _authenticationService = authService;
            _memberForSquadRepo = membersForSquadRepo;
            _availabilityService = availabilityService;
        }

        public Member GetMemberById(Guid memberId)
        {
            return _memberRepository.GetById(memberId);
        }

        public Member GetMemberByEmail(string email) {
            try
            {
                List<Member> members = _memberRepository.GetAll().ToList();
                return members.Single(x => x.Email == email);
            }
            catch (System.Exception ex)
            {
                 throw new Exception("Unable to get the member from the database.  Probably doesn't exist. " + ex.Message);
            }
        }

        public List<Member> GetAllMembers()
        {
            return _memberRepository.GetAll().OrderBy(x => x.FirstName).ToList();
        }

        public List<Member> GetAllMembersNotInSquad(Guid squadId) {
            
            return _memberForSquadRepo.GetMembersNotInSquad(squadId).OrderBy(x => x.FirstName).ToList();
        }

        public List<Member> GetAllMembersInSquad(Guid squadId) {
            return _memberRepository.GetAllByParentId(squadId).OrderBy(x => x.FirstName).ToList();
        }

        public Guid AddMember(string firstname, string lastname, string email, string password) {
            // Construct the member to pass to the other AddMember function
            Member memberToAdd = new Member(firstname, lastname, email);
            // Inserts a record in the _memberRepository
            _memberRepository.Add(memberToAdd);
            // Inserts a password into the MEMBER table via authentication service
            _authenticationService.UpdatePassword(password, memberToAdd.Id);

            return memberToAdd.Id; // Return the Guid so we don't have to get this back?  Maybe it'll be useful?
        }

        public void UpdateMember(Member member)
        {
            _memberRepository.Update(member);
        }

        public void DeleteMember(Member member)
        {
            _memberRepository.Delete(member);
        }

        public DateTime GetJoinedDateForSquadMember(Guid memberId, Guid squadId) {
            return _memberForSquadRepo.GetJoinedDateForSquadMember(memberId, squadId);
        }


    }

}

