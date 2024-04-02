using ScheduSquad.Models;

namespace ScheduSquad.Models
{
    public class Availability
    {

        public Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        private TimeSpan _startTime;
        public TimeSpan StartTime
        {
            get
            { return _startTime; }
            set
            {
                if (AreMinutesValid(value))
                {
                    _startTime = value;
                }
                else
                {
                    throw new ArgumentException("StartTime is Invalid.  Time must end in :00, :15, :30, or :45.");
                }
            }
        }
        private TimeSpan _endTime;
        public TimeSpan EndTime
        {
            get
            { return _endTime; }
            set
            {
                if (AreMinutesValid(value))
                {
                    _endTime = value;
                }
                else
                {
                    throw new ArgumentException("EndTime is Invalid.  Time must end in :00, :15, :30, or :45.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="endTime"></param>
        public Availability(DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime) : this(Guid.NewGuid(), dayOfWeek, startTime, endTime) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="startTime"></param>
        /// <param name="dayOfWeek"></param>
        /// <param name="endTime"></param>
        public Availability(Guid id, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {

            try
            {
                Id = id;
                DayOfWeek = dayOfWeek;
                StartTime = startTime;
                EndTime = endTime;

                if (startTime > endTime) throw new ArgumentException("End Time must be later than the Start Time");
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException("Unable to Initialize Availability.", ex);
            }
        }
    

    /// <summary>
    /// Evaluates the Start/End Date/Time of Availability and returns all the Availability Codes for a Week (Sunday-Saturday)
    /// </summary>
    /// <returns>Returns enumerable list of integers representing the availability codes</returns>
    public List<int> GetAvailabilityCodes()
    {
        // Create list 
        List<int> codes = new List<int>();
        // Calculate Start Code
        var startCode = (((int)DayOfWeek + 1) * 100) + (((StartTime.Hours * 60) + StartTime.Minutes) / 15);
        // Calculate End Code
        var endCode = (((int)DayOfWeek + 1) * 100) + (((EndTime.Hours * 60) + EndTime.Minutes) / 15);
        // Insert into list, starting at start code and breaking out at end code
        for (int i = startCode; i < endCode; i++)
        {
            codes.Add(i);
        }
        // return
        return codes;
    }

    /// <summary>
    /// Validates if the Minutes on the Availability is valid.  Minutes need to be in incremements of 15 (:00, :15, :30, :45)
    /// </summary>
    /// <param name="time">A TimeSpan Object to evaluate</param>
    /// <returns></returns>
    private bool AreMinutesValid(TimeSpan time)
    {
        // Validates that the TimeSpan (Minutes) is :00, :15, :30, :45
        return (time.Minutes % 15) != 0 ? false : true;
    }
}
}