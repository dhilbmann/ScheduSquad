using ScheduSquad.Models;

namespace ScheduSquad.Service
{

    public interface IAvailabilityService
    {
        public Availability GetAvailabilityById(Guid availabilityId);
        public List<Availability> GetAllAvailabilities();
        public List<Availability> GetAllAvailabilitiesBelongingToMember(Guid memberId);
        public void AddAvailability(Availability availability);
        public void AddAvailability(Availability availability, Guid id);
        public void UpdateAvailability(Availability availability);
        public void DeleteAvailability(Availability availability);
        public List<int> GetCommonAvailabilityCodes(Squad squad);
        public List<int> GetCommonAvailabilityCodes(List<List<int>> lists);
        public List<List<int>> SplitAvailabilities(List<int> availability);
        public String GetHumanReadableAvailabilityString(List<int> availability);
    }
}





