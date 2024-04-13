using ScheduSquad.Models;

namespace ScheduSquad.Service
{

    public interface ISquadService
    {
        public Squad GetSquadById(Guid squadId) ;
        public List<Squad> GetAllSquads();
        public List<Squad> GetAllSquadsBelongingToMember(Guid memberId);
        public void AddSquad(Squad squad);
        public void UpdateSquad(Squad squad);
        public void DeleteSquad(Squad squad);
        public void AddMemberToSquad(Member member, Squad squad);
        public void RemoveMemberFromSquad(Member member, Squad squad);
        public string Test();

    }
}





