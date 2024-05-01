namespace ScheduSquad.Web.Models
{
    public class EditRowViewModel
    {
        public Guid AvailabilityId {get; set;}

        public TimeSpan StartTime {get; set;}

        public TimeSpan EndTime {get; set;}

        public int DayOfWeek {get; set;}
        
    }
}