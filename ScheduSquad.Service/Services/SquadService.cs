using ScheduSquad.Models;

namespace ScheduSquad.Service
{
    public class SquadService
    {
        private readonly IRepository<Squad> _squadRepository;

        public SquadService(IRepository<Squad> squadRepo) {
            _squadRepository = squadRepo;
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

        public void AddSquad(Squad squad) {
            _squadRepository.Add(squad);
        }

        public void UpdateSquad(Squad squad) {
            _squadRepository.Update(squad);
        }

        public void DeleteSquad(Squad squad) {
            _squadRepository.Delete(squad);
        }

        public void AddMemberToSquad(Member member, Squad squad)
        {
            throw new NotImplementedException();
        }

         public void RemoveMemberFromSquad(Member member, Squad squad)
        {
            throw new NotImplementedException();
        }
    }
}

