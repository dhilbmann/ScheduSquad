using ScheduSquad.Models;

namespace ScheduSquad.Service {

public class AvailabilityService {

/// <summary>
/// Function that pulls all availability codes from users in a squad and finds the unique set of codes.
/// </summary>
/// <param name="squad"></param>
/// <returns>List of Integers; unique availability codes</returns>
    public List<int> GetCommonAvailabilityCodes(Squad squad) {
        List<List<int>> lists = new List<List<int>>();
        foreach (User u in squad.Users)
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

