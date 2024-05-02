using ScheduSquad.Models;

namespace ScheduSquad.Service
{

    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _addAvailabilityRepo;

        private readonly IRepository<Availability> _availabilityRepository;

        public AvailabilityService(IRepository<Availability> availabilityRepo, IAvailabilityRepository addAvailabilityRepo)
        {
            _availabilityRepository = availabilityRepo;
            _addAvailabilityRepo = addAvailabilityRepo;
        }


        public Availability GetAvailabilityById(Guid availabilityId)
        {
            return _availabilityRepository.GetById(availabilityId);
        }

        public List<Availability> GetAllAvailabilities()
        {
            return _availabilityRepository.GetAll().OrderBy(x => (int)x.DayOfWeek).ThenBy(x => x.StartTime).ToList();
        }

        public List<Availability> GetAllAvailabilitiesBelongingToMember(Guid memberId)
        {
            return _availabilityRepository.GetAllByParentId(memberId).OrderBy(x => (int)x.DayOfWeek).ThenBy(x => x.StartTime).ToList();
        }

        public void AddAvailability(Availability availability)
        {
            _availabilityRepository.Add(availability);
        }

        public void AddAvailability(Availability availability, Guid id)
        {
            _addAvailabilityRepo.Add(availability, id);
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
                List<int> codes = u.GetAllAvailabilityCodes();
                if (codes.Count > 0) lists.Add(codes);
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

      public List<List<int>> SplitAvailabilities(List<int> availability)
        {
            List<List<int>> splitAvailabilities = new List<List<int>>();
            if (availability.Count == 0)
            {
                return splitAvailabilities;
            }

            List<int> tempSpan = new List<int>();

            for (int i = 0; i < availability.Count; i++)
            {
                if (tempSpan.Count() >=1 )
                {
                    if (availability[i] != tempSpan[tempSpan.Count() - 1] + 1)
                    {
                        splitAvailabilities.Add(new List<int>(tempSpan));
                        tempSpan.Clear();
                    }
                }

                tempSpan.Add(availability[i]);
            }

            splitAvailabilities.Add(new List<int>(tempSpan));
            splitAvailabilities.Sort((left, right) => left[0].CompareTo(right[0]));
            return splitAvailabilities;
        }

        public String GetHumanReadableAvailabilityString(List<int> availability)
        {
            int startCode = availability [0];
            int endCode = availability[availability.Count-1];

            int startTimeCode = startCode % 100;
            int endTimeCode = endCode % 100;

            DayOfWeek availableDay = (DayOfWeek)((startCode - startTimeCode) / 100 - 1);

            int startInterval = startTimeCode * 15;
            int endInterval = endTimeCode * 15;

            int startHours = startInterval / 60;
            int endHours = endInterval / 60;

            int startMinutes = startInterval % 60;
            int endMinutes = endInterval % 60;

            TimeOnly startTime = new TimeOnly(startHours, startMinutes);
            TimeOnly endTime = new TimeOnly(endHours, endMinutes);

            String humanReadable = String.Format
            (
                "On {0} from {1} to {2}.",
                availableDay.ToString(),
                startTime.ToShortTimeString(),
                endTime.ToShortTimeString()
            );

            return humanReadable;
        }
    }
}


