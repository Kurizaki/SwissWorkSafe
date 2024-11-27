namespace SwissWorkSafe.Models.Core
{
    public class SalaryContinuation
    {
        public DateTime StartDate { get; set; }
        public DateTime EventDate { get; set; }
        public int Duration { get; set; }

        public int CalculateDuration(DateTime startDate, DateTime eventDate) { return 0; }
        public bool CheckSalaryContinuationEligibility() { return false; }
    }
}
