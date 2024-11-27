namespace SwissWorkSafe.Models.Core
{
    public class Termination
    {
        public DateTime StartDate { get; set; }
        public DateTime TerminationDate { get; set; }

        public int CalculatePeriod(DateTime startDate, DateTime terminationDate) { return 0; }
        public bool CheckProhibitedPeriod(DateTime terminationDate) { return false; }
        public void FindArticles(string text) { }
    }
}
