using ScheduSquad.Models;

namespace ScheduSquad.Service
{
    public class SquadService : ISquadService
    {
        private readonly IRepository<Squad> _squadRepository;
        private readonly IMemberService _memberService;

        public SquadService(IRepository<Squad> squadRepo, IMemberService memberService) {
            _squadRepository = squadRepo;
            _memberService = memberService;
        }

        public string Test() {
            return _squadRepository.Test();
        }

        public Squad GetSquadById(Guid squadId) {
           return _squadRepository.GetById(squadId);
        }

        public List<Squad> GetAllSquads() {
            return _squadRepository.GetAll().ToList();
        }

        public List<Squad> GetAllSquadsBelongingToMember(Guid memberId) {
            return _squadRepository.GetAllByParentId(memberId).ToList();
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

        public void AddMemberToSquad(Guid memberId, Guid squadId)
        {
            Member memberToAdd = _memberService.GetMemberById(memberId);
            Squad squadToAddTo = _squadRepository.GetById(squadId);

            AddMemberToSquad(memberToAdd, squadToAddTo);
        }

        public void AddMemberToSquad(Member member, Squad squad)
        {
            // TODO: No Repo function to call for this process.
            throw new NotImplementedException();
        }


         public void RemoveMemberFromSquad(Member member, Squad squad)
        {
            throw new NotImplementedException();
        }

  
    }
}

