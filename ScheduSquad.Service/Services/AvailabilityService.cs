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

      public List<List<int>> SplitAvailabilities(List<int> availability)
        {
            List<List<int>> splitAvailabilities = new List<List<int>>();
            List<int> tempSpan = new List<int>();

            for (int i = 0; i < availability.Count; i++)
            {
                tempSpan.Add(availability[i]);

                if (availability[i] != availability[i+1] + 1)
                {
                    splitAvailabilities.Add(tempSpan);
                    tempSpan.Clear();
                }
            }

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


