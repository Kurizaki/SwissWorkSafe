
namespace SwissWorkSafe.Models.Core
{
    /*
         $$$$$$\                $$\                     $$\      $$\                     $$\        $$$$$$\             $$$$$$\
        $$  __$$\               \__|                    $$ | $\  $$ |                    $$ |      $$  __$$\           $$  __$$\
        $$ /  \__|$$\  $$\  $$\ $$\  $$$$$$$\  $$$$$$$\ $$ |$$$\ $$ | $$$$$$\   $$$$$$\  $$ |  $$\ $$ /  \__| $$$$$$\  $$ /  \__|$$$$$$\
        \$$$$$$\  $$ | $$ | $$ |$$ |$$  _____|$$  _____|$$ $$ $$\$$ |$$  __$$\ $$  __$$\ $$ | $$  |\$$$$$$\   \____$$\ $$$$\    $$  __$$\
         \____$$\ $$ | $$ | $$ |$$ |\$$$$$$\  \$$$$$$\  $$$$  _$$$$ |$$ /  $$ |$$ |  \__|$$$$$$  /  \____$$\  $$$$$$$ |$$  _|   $$$$$$$$ |
        $$\   $$ |$$ | $$ | $$ |$$ | \____$$\  \____$$\ $$$  / \$$$ |$$ |  $$ |$$ |      $$  _$$<  $$\   $$ |$$  __$$ |$$ |     $$   ____|
        \$$$$$$  |\$$$$$\$$$$  |$$ |$$$$$$$  |$$$$$$$  |$$  /   \$$ |\$$$$$$  |$$ |      $$ | \$$\ \$$$$$$  |\$$$$$$$ |$$ |     \$$$$$$$\
         \______/  \_____\____/ \__|\_______/ \_______/ \__/     \__| \______/ \__|      \__|  \__| \______/  \_______|\__|      \_______|
        Authors: Keanu Koelewijn, Rebecca Wili, Salma Tanner, Lorenzo Lai
    */
    /// <summary>
    /// Provides functionalities to calculate salary continuation durations based on employment start and event dates and scale.
    /// </summary>
    public class SalaryContinuation
    {
        /// <summary>
        /// Calculates the duration of salary continuation in days based on the employee's service years and selected scale.
        /// </summary>
        /// <param name="startDate">The employment start date.</param>
        /// <param name="eventDate">The date of the event triggering salary continuation.</param>
        /// <param name="scale">The selected scale (Basler, Berner, Zürcher).</param>
        /// <returns>The duration of salary continuation in days.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the start date is after the event date or an invalid scale is provided.
        /// </exception>
        public static int CalculateSalaryContinuationDuration(DateTime startDate, DateTime eventDate, string scale)
        {
            // Ensure the start date is not after the event date
            if (startDate > eventDate)
            {
                throw new ArgumentException("Das Startdatum darf nicht nach dem Ereignisdatum liegen.");
            }

            // Calculate complete service years
            int serviceYears = eventDate.Year - startDate.Year;
            if (eventDate.Month < startDate.Month ||
               (eventDate.Month == startDate.Month && eventDate.Day < startDate.Day))
            {
                serviceYears--;
            }

            // Initialize duration
            int continuationDays = 0;

            // Determine duration based on scale and service years
            switch (scale.ToLowerInvariant())
            {
                case "basel":
                    if (serviceYears >= 1 && serviceYears < 2)
                        continuationDays = 21;
                    else if (serviceYears == 2 || serviceYears == 3)
                        continuationDays = 60;
                    else if (serviceYears == 4)
                        continuationDays = 90;
                    else if (serviceYears >= 5 && serviceYears < 11)
                        continuationDays = 90;
                    else if (serviceYears >= 11 && serviceYears < 15)
                        continuationDays = 120;
                    else if (serviceYears >= 15 && serviceYears < 16)
                        continuationDays = 120;
                    else if (serviceYears >= 16 && serviceYears < 21)
                        continuationDays = 150;
                    else if (serviceYears >= 21 && serviceYears < 25)
                        continuationDays = 180;
                    else if (serviceYears >= 25)
                        continuationDays = 180 + ((serviceYears - 25) / 5) * 30;
                    break;

                case "bern":
                    if (serviceYears >= 1 && serviceYears < 2)
                        continuationDays = 21;
                    else if (serviceYears == 2)
                        continuationDays = 30;
                    else if (serviceYears == 3 || serviceYears == 4)
                        continuationDays = 60;
                    else if (serviceYears >= 5 && serviceYears < 10)
                        continuationDays = 90;
                    else if (serviceYears >= 10 && serviceYears < 15)
                        continuationDays = 120;
                    else if (serviceYears >= 15 && serviceYears < 16)
                        continuationDays = 150;
                    else if (serviceYears >= 16 && serviceYears < 20)
                        continuationDays = 150;
                    else if (serviceYears >= 20)
                        continuationDays = 180;
                    break;

                case "zürich":
                    if (serviceYears >= 1 && serviceYears < 2)
                        continuationDays = 21;
                    else if (serviceYears == 2)
                        continuationDays = 56;
                    else if (serviceYears == 3)
                        continuationDays = 63;
                    else if (serviceYears == 4)
                        continuationDays = 70;
                    else if (serviceYears >= 5 && serviceYears < 11)
                        continuationDays = 77 + (serviceYears - 5) * 7;
                    else if (serviceYears >= 11 && serviceYears < 16)
                        continuationDays = 119 + (serviceYears - 11) * 7;
                    else if (serviceYears >= 16 && serviceYears < 21)
                        continuationDays = 154 + (serviceYears - 16) * 7;
                    else if (serviceYears >= 21)
                        continuationDays = 189 + (serviceYears - 21) * 7;
                    break;

                default:
                    throw new ArgumentException("Ungültige Skala. Bitte wählen Sie zwischen Basler, Berner und Zürcher.");
            }

            return continuationDays;
        }

        /// <summary>
        /// Breaks down a duration in days into weeks, remaining days, months, and remaining days in the last month.
        /// </summary>
        /// <param name="duration">The total duration in days.</param>
        /// <returns>
        /// A tuple containing:
        /// - <c>Weeks</c>: The number of complete weeks.
        /// - <c>RemainingDays</c>: The remaining days after the weeks.
        /// - <c>Months</c>: The number of complete months (assuming 30 days per month).
        /// - <c>RemainingDaysInMonth</c>: The remaining days after the months.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the duration is negative.
        /// </exception>
        public static (int Weeks, int RemainingDays, int Months, int RemainingDaysInMonth)
            CalculateWeeksAndMonths(int duration)
        {
            // Ensure the duration is non-negative
            if (duration < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), "Die Dauer muss eine nicht-negative Zahl sein.");
            }

            // Calculate weeks and remaining days
            int weeks = duration / 7;
            int remainingDays = duration % 7;

            // Calculate months and remaining days in the last month
            int months = duration / 30;
            int remainingDaysInMonth = duration % 30;

            return (weeks, remainingDays, months, remainingDaysInMonth);
        }
    }
}
