using ScheduSquad.Models;
using ScheduSquad.DataAccess;

namespace ScheduSquad.Service
{
    public class SquadService : ISquadService
    {
        private readonly IRepository<Squad> _squadRepository;
        private readonly ISquadMemberRepository _squadMemberRepo;
        private readonly IMemberService _memberService;

        public SquadService(IRepository<Squad> squadRepo, ISquadMemberRepository squadMemberRepo, IMemberService memberService) {
            _squadRepository = squadRepo;
            _squadMemberRepo = squadMemberRepo;
            _memberService = memberService;
            
        }

        public Squad GetSquadById(Guid squadId) {
            Squad s = _squadRepository.GetById(squadId);
            s.Members = _memberService.GetAllMembersInSquad(squadId);
            return s;
        }

        public List<Squad> GetAllSquads() {
            List<Squad> squads = _squadRepository.GetAll().ToList();
             foreach(Squad s in squads) {
                s.Members = _memberService.GetAllMembersInSquad(s.Id);
            }
            return squads;
        }

        public List<Squad> GetAllSquadsBelongingToMember(Guid memberId) {
            List<Squad> squads = _squadRepository.GetAllByParentId(memberId).ToList();
             foreach(Squad s in squads) {
                s.Members = _memberService.GetAllMembersInSquad(s.Id);
            }
            return squads;
        }
        

        public List<Squad> GetAllSquadsNotBelongingToMember(Guid memberId) {
            // Get all the squads
            List<Squad> squadList = this.GetAllSquads();
           
            // Find all the squads that doesn't have the member with the provided id
            return squadList.Where(squad => !squad.Members.Any(member => member.Id == memberId)).ToList();
        }

        public void AddSquad(Squad squad) {
            _squadRepository.Add(squad);
        }


        public void AddSquad(Guid squadMasterId, string squadName, string description, string location) {
            // Get SquadMaster by Id via MemberService
            Member squadMaster = _memberService.GetMemberById(squadMasterId);
            // If SquadMaster is Null, then throw exception.
            if (squadMaster == null) { throw new Exception("Unable to create squad! (SquadMaster was not found)"); }
            // Instantiate a new Squad
            Squad s = new Squad(squadMaster, squadName, description, location);
            try {
                // Add Squad via Repo
                _squadRepository.Add(s);
                _squadMemberRepo.AddMemberToSquad(squadMaster.Id, s.Id, true);
            } catch (Exception ex) { // Uh oh.  
                throw new Exception("Failed to create a new Squad.  " + ex);
            }
        }

        public void UpdateSquad(Squad squad) {
            _squadRepository.Update(squad);
        }

        public void DeleteSquad(Squad squad) {
            _squadRepository.Delete(squad);
        }

        public void AddMemberToSquad(Guid memberId, Guid squadId, bool isSquadMaster)
        {
            Member memberToAdd = _memberService.GetMemberById(memberId);
            Squad squadToAddTo = _squadRepository.GetById(squadId);

            AddMemberToSquad(memberToAdd, squadToAddTo, isSquadMaster);
        }

        public void AddMemberToSquad(Member member, Squad squad, bool isSquadMaster)
        {
            _squadMemberRepo.AddMemberToSquad(member.Id, squad.Id, isSquadMaster);
        }


         public void RemoveMemberFromSquad(Member member, Squad squad)
        {
            _squadMemberRepo.RemoveMemberFromSquad(member.Id, squad.Id);
        }

        public void RemoveMemberFromSquad(Guid memberId, Guid squadId) {
            Member memberToRemove = _memberService.GetMemberById(memberId);
            Squad squadToRemoveFrom = _squadRepository.GetById(squadId);

            _squadMemberRepo.RemoveMemberFromSquad(memberToRemove.Id, squadToRemoveFrom.Id);
        }
  
    }
}

