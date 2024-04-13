using ScheduSquad.Models;

namespace ScheduSquad.Service
{

    public class AvailabilityService : IAvailabilityService
    {

        private readonly IRepository<Availability> _availabilityRepository;

        public AvailabilityService(IRepository<Availability> availabilityRepo)
        {
            _availabilityRepository = availabilityRepo;
        }

        public string Test() {
            return _availabilityRepository.Test();
        }

        public Availability GetAvailabilityById(Guid availabilityId)
        {
            return _availabilityRepository.GetById(availabilityId);
        }

        public List<Availability> GetAllAvailabilities()
        {
            return _availabilityRepository.GetAll().ToList();
        }

        public List<Availability> GetAllAvailabilitiesBelongingToMember(Guid memberId)
        {
            return _availabilityRepository.GetAllByParentId(memberId).ToList();
        }

        public void AddAvailability(Availability availability)
        {
            _availabilityRepository.Add(availability);
        }

        public void UpdateAvailability(Availability availability)
        {
            _availabilityRepository.Update(availability);
        }

        public void DeleteAvailability(Availability availability)
        {
            _availabilityRepository.Delete(availability);
        }

        /// <summary>
        /// Function that pulls all availability codes from users in a squad and finds the unique set of codes.
        /// </summary>
        /// <param name="squad"></param>
        /// <returns>List of Integers; unique availability codes</returns>
        public List<int> GetCommonAvailabilityCodes(Squad squad)
        {
            List<List<int>> lists = new List<List<int>>();
            foreach (Member u in squad.Members)
            {
                lists.Add(u.GetAllAvailabilityCodes());
            }
            return GetCommonAvailabilityCodes(lists);
        }

        public List<int> GetCommonAvailabilityCodes(List<List<int>> lists)
        {
            if (lists == null || lists.Count == 0)
            {
                return new List<int>();
            }

            // Create a dictionary to store value frequencies
            Dictionary<int, int> frequencyMap = new Dictionary<int, int>();

            // Iterate through each list and count frequency of values
            foreach (List<int> list in lists)
            {
                foreach (int value in list)
                {
                    if (!frequencyMap.ContainsKey(value))
                    {
                        frequencyMap[value] = 1;
                    }
                    else
                    {
                        frequencyMap[value]++;
                    }
                }
            }

            // Find values that appear in all lists
            List<int> commonValues = new List<int>();
            foreach (var kvp in frequencyMap)
            {
                if (kvp.Value == lists.Count)
                {
                    commonValues.Add(kvp.Key);
                }
            }

            return commonValues;
        }

      
    }
}

