using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwissWorkSafe.Models.Articles;

namespace SwissWorkSafe.Models.Core
{
    public class SwissWorkSafe
    {
        public Models.Settings.Settings settings { get; set; }

        public int CalculateNoticePeriod(DateTime startDate, DateTime terminationDate) { return 0; }
        public int CalculateSalaryContinuation(DateTime startDate, DateTime eventDate) { return 0; }
        public bool CheckProhibitedPeriods(DateTime terminationDate) { return false; }
        public List<Article> FindRelevantArticles(string text) { return null; }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
        }
    }
}
