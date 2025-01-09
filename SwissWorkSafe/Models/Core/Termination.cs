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
         \______/  \_____\____/ \__|\_______/ \_______/ \__/     \__| \______/ \__|      \__|  \__| \______/  \_______|\__|      \_______/
    Authors: Keanu Koelewijn, Rebecca Wili, Salma Tanner, Lorenzo Lai
    */
    /// <summary>
    /// Represents the termination details and provides functionalities to calculate termination deadlines.
    /// </summary>
    public class Termination
    {
        /// <summary>
        /// Gets the employment start date.
        /// </summary>
        public DateTime StartDate { get; }

        /// <summary>
        /// Gets the termination date.
        /// </summary>
        public DateTime TerminationDate { get; }

        /// <summary>
        /// Gets the number of sick days.
        /// </summary>
        public int SickDays { get; }

        /// <summary>
        /// Gets the reason for absence.
        /// </summary>
        public string AbsenceReason { get; private set; }  // Added private set

        /// <summary>
        /// Gets the start date of the reason for absence, if applicable.
        /// </summary>
        public DateTime? ReasonStartDate { get; private set; }  // Added private set

        /// <summary>
        /// Gets the end date of the reason for absence, if applicable.
        /// </summary>
        public DateTime? ReasonEndDate { get; private set; }  // Added private set

        /// <summary>
        /// Gets the number of years of service.
        /// </summary>
        public int YearsOfService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Termination"/> class with the specified parameters.
        /// </summary>
        /// <param name="startDate">The employment start date.</param>
        /// <param name="terminationDate">The termination date.</param>
        /// <param name="sickDays">The number of sick days.</param>
        /// <param name="absenceReason">The reason for absence.</param>
        /// <param name="reasonStartDate">The start date of the reason for absence, if applicable.</param>
        /// <param name="yearsOfService">The number of years of service.</param>
        /// <param name="reasonEndDate">The end date of the reason for absence, if applicable.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the start date is after the termination date or when the absence reason is invalid.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when sick days or years of service are negative.
        /// </exception>
        public Termination(
            DateTime startDate,
            DateTime terminationDate,
            int sickDays,
            string absenceReason,
            DateTime? reasonStartDate,
            int yearsOfService = 1,
            DateTime? reasonEndDate = null) // Added reasonEndDate parameter
        {
            // Validate that the start date is not after the termination date
            if (startDate > terminationDate)
            {
                throw new ArgumentException("The start date cannot be after the termination date.");
            }

            // Validate that sick days are non-negative
            if (sickDays < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sickDays), "Sick days cannot be negative.");
            }

            // Validate that absence reason is provided
            if (string.IsNullOrWhiteSpace(absenceReason))
            {
                throw new ArgumentException("The absence reason cannot be empty.", nameof(absenceReason));
            }

            // Normalize and validate the absence reason against allowed reasons
            AbsenceReason = absenceReason.Trim().ToLowerInvariant();
            string[] allowedReasons = { "militärdienst", "krankheit", "unfall", "schwangerschaft", "betreuungsurlaub", "hilfsaktion" };
            if (Array.IndexOf(allowedReasons, AbsenceReason) < 0)
            {
                throw new ArgumentException(
                    $"The absence reason '{absenceReason}' is not recognized. Please choose a valid reason from the allowed list.",
                    nameof(absenceReason));
            }

            // Validate that years of service are non-negative
            if (yearsOfService < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(yearsOfService), "Years of service cannot be negative.");
            }

            // For specific absence reasons, ReasonStartDate is required
            if ((AbsenceReason == "militärdienst" || AbsenceReason == "betreuungsurlaub") && !reasonStartDate.HasValue)
            {
                throw new ArgumentException(
                    $"A reason start date is required for the absence reason '{absenceReason}'.",
                    nameof(reasonStartDate));
            }

            // If ReasonEndDate is provided, ensure it's after ReasonStartDate
            if (reasonEndDate.HasValue && reasonStartDate.HasValue)
            {
                if (reasonEndDate.Value < reasonStartDate.Value)
                {
                    throw new ArgumentException("The ReasonEndDate cannot be earlier than ReasonStartDate.", nameof(reasonEndDate));
                }

                // Ensure ReasonEndDate does not exceed TerminationDate
                if (reasonEndDate.Value > terminationDate)
                {
                    throw new ArgumentException("The ReasonEndDate cannot be after the TerminationDate.", nameof(reasonEndDate));
                }
            }

            // If ReasonStartDate is provided, ensure it's before TerminationDate
            if (reasonStartDate.HasValue && reasonStartDate.Value > terminationDate)
            {
                throw new ArgumentException("The ReasonStartDate cannot be after the TerminationDate.", nameof(reasonStartDate));
            }

            StartDate = startDate;
            TerminationDate = terminationDate;
            SickDays = sickDays;
            ReasonStartDate = reasonStartDate;
            ReasonEndDate = reasonEndDate;
            YearsOfService = yearsOfService;
        }

        /// <summary>
        /// Sets the absence details, such as reason, start date, and optional end date.
        /// </summary>
        /// <param name="reason">The reason for absence (e.g., "militärdienst", "krankheit").</param>
        /// <param name="startDate">The start date of the absence.</param>
        /// <param name="endDate">The optional end date of the absence.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the absence reason is invalid or dates are inconsistent.
        /// </exception>
        public void SetAbsenceDetails(string reason, DateTime startDate, DateTime? endDate = null)
        {
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("The absence reason cannot be empty.", nameof(reason));
            }

            string normalizedReason = reason.Trim().ToLowerInvariant();
            string[] allowedReasons = { "militärdienst", "krankheit", "unfall", "schwangerschaft", "betreuungsurlaub", "hilfsaktion" };
            if (Array.IndexOf(allowedReasons, normalizedReason) < 0)
            {
                throw new ArgumentException(
                    $"The absence reason '{reason}' is not recognized. Please choose a valid reason from the allowed list.",
                    nameof(reason));
            }

            // If endDate is provided, ensure it's after startDate
            if (endDate.HasValue && endDate.Value < startDate)
            {
                throw new ArgumentException("The ReasonEndDate cannot be earlier than ReasonStartDate.", nameof(endDate));
            }

            // If endDate is provided, ensure it does not exceed TerminationDate
            if (endDate.HasValue && endDate.Value > TerminationDate)
            {
                throw new ArgumentException("The ReasonEndDate cannot be after the TerminationDate.", nameof(endDate));
            }

            // Ensure startDate is not after TerminationDate
            if (startDate > TerminationDate)
            {
                throw new ArgumentException("The ReasonStartDate cannot be after the TerminationDate.", nameof(startDate));
            }

            AbsenceReason = normalizedReason;
            ReasonStartDate = startDate;
            ReasonEndDate = endDate;
        }

        /// <summary>
        /// Calculates the termination deadline based on the provided details.
        /// </summary>
        /// <returns>The calculated termination deadline as a <see cref="DateTime"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when there are inconsistencies in the dates or invalid sick days.
        /// </exception>
        public DateTime CalculateTerminationDeadline()
        {
            // Ensure termination date is after the start date
            if (TerminationDate < StartDate)
            {
                throw new InvalidOperationException("The termination date must be after the start date.");
            }

            // Ensure sick days are non-negative
            if (SickDays < 0)
            {
                throw new InvalidOperationException("Sick days cannot be negative.");
            }

            // Calculate the duration of employment in months
            TimeSpan employmentDuration = TerminationDate - StartDate;
            int months = (int)(employmentDuration.TotalDays / 30);

            // Determine the termination period in months based on employment duration
            int terminationPeriodMonths = months switch
            {
                < 12 => 1,              // Less than 1 year: 1 month
                <= 120 => 2,            // 1 to 10 years: 2 months
                _ => 3                   // More than 10 years: 3 months
            };

            // Adjust termination date by adding sick days
            DateTime adjustedTerminationDate;
            try
            {
                adjustedTerminationDate = TerminationDate.AddDays(SickDays);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException("The number of sick days results in an invalid date.", ex);
            }

            // Extend termination deadline if required
            if (MustBeExtended(adjustedTerminationDate))
            {
                adjustedTerminationDate = CalculateExtension(adjustedTerminationDate);
            }

            // Add the termination period to the adjusted termination date
            DateTime endTerminationPeriod;
            try
            {
                endTerminationPeriod = adjustedTerminationDate.AddMonths(terminationPeriodMonths);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException("Calculating the termination period results in an invalid date.", ex);
            }

            // Return the last day of the termination period month
            try
            {
                return new DateTime(
                    endTerminationPeriod.Year,
                    endTerminationPeriod.Month,
                    DateTime.DaysInMonth(endTerminationPeriod.Year, endTerminationPeriod.Month));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException("The end of the termination period falls on an invalid date.", ex);
            }
        }

        /// <summary>
        /// Determines whether the termination deadline must be extended based on specific criteria.
        /// </summary>
        /// <param name="terminationNoticeDate">The adjusted termination notice date.</param>
        /// <returns><c>true</c> if the termination deadline must be extended; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the termination notice date is before the start date.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when years of service are negative or the absence reason is unknown.
        /// </exception>
        public bool MustBeExtended(DateTime terminationNoticeDate)
        {
            // Ensure termination notice date is after the start date
            if (terminationNoticeDate < StartDate)
            {
                throw new ArgumentException("The termination notice date must be after the start date.", nameof(terminationNoticeDate));
            }

            // Ensure years of service are non-negative
            if (YearsOfService < 0)
            {
                throw new InvalidOperationException("Years of service cannot be negative.");
            }

            return AbsenceReason switch
            {
                "militärdienst" => CheckMilitaryServiceExtension(terminationNoticeDate),
                "krankheit" or "unfall" => CheckHealthOrAccidentExtension(terminationNoticeDate),
                "schwangerschaft" => CheckPregnancyExtension(terminationNoticeDate),
                "betreuungsurlaub" => CheckCareLeaveExtension(terminationNoticeDate),
                "hilfsaktion" => CheckAidActionExtension(terminationNoticeDate),
                _ => throw new InvalidOperationException($"Unknown absence reason: {AbsenceReason}."),
            };
        }

        /// <summary>
        /// Calculates the termination deadline extension based on specific criteria.
        /// </summary>
        /// <param name="terminationNoticeDate">The adjusted termination notice date.</param>
        /// <returns>The extended termination notice date as a <see cref="DateTime"/>.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the termination notice date is before the start date.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when required information is missing or the absence reason is unknown.
        /// </exception>
        public DateTime CalculateExtension(DateTime terminationNoticeDate)
        {
            // Ensure termination notice date is after the start date
            if (terminationNoticeDate < StartDate)
            {
                throw new ArgumentException("The termination notice date must be after the start date.", nameof(terminationNoticeDate));
            }

            // Validate that ReasonEndDate is not after TerminationDate
            if (ReasonEndDate.HasValue && ReasonEndDate.Value > TerminationDate)
            {
                throw new InvalidOperationException("The ReasonEndDate cannot be after the TerminationDate.");
            }

            return AbsenceReason switch
            {
                "krankheit" or "unfall" => CalculateHealthOrAccidentExtension(terminationNoticeDate),
                "schwangerschaft" => terminationNoticeDate.AddDays(16 * 7), // 16 weeks
                "militärdienst" => CalculateMilitaryServiceExtension(),
                "betreuungsurlaub" => CalculateCareLeaveExtension(),
                "hilfsaktion" => terminationNoticeDate.AddDays(7 * 4), // 4 weeks
                _ => throw new InvalidOperationException($"Unknown absence reason: {AbsenceReason}."),
            };
        }

        /// <summary>
        /// Determines if the termination is invalid based on specific criteria.
        /// </summary>
        /// <returns><c>true</c>, if the termination is invalid; otherwise, <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when required information is missing or inconsistent.
        /// </exception>
        public bool IsTerminationInvalid()
        {
            // Check if the termination occurred during a protection period
            if (AbsenceReason switch
            {
                "militärdienst" => IsDuringMilitaryService(),
                "krankheit" or "unfall" => IsDuringHealthOrAccidentProtection(),
                "schwangerschaft" => IsDuringPregnancyProtection(),
                "betreuungsurlaub" => IsDuringCareLeave(),
                "hilfsaktion" => IsDuringAidActionProtection(),
                _ => false
            })
            {
                return true;
            }

            // Additional conditions for invalid termination can be added here (e.g., discrimination, contract breach)
            return false;
        }

        #region Private Helper Methods for Invalidity Checks

        /// <summary>
        /// Checks if the termination occurred during a military service protection period.
        /// </summary>
        private bool IsDuringMilitaryService()
        {
            if (ReasonStartDate.HasValue)
            {
                DateTime militaryServiceEnd = ReasonStartDate.Value.AddDays(11); // Assumed duration
                DateTime bufferPeriodStart = ReasonStartDate.Value.AddDays(-28); // 4 weeks before
                DateTime bufferPeriodEnd = militaryServiceEnd.AddDays(28); // 4 weeks after

                return TerminationDate >= bufferPeriodStart && TerminationDate <= bufferPeriodEnd;
            }

            return false;
        }

        /// <summary>
        /// Checks if the termination occurred during a health or accident protection period.
        /// </summary>
        private bool IsDuringHealthOrAccidentProtection()
        {
            if (ReasonStartDate.HasValue && ReasonEndDate.HasValue)
            {
                DateTime protectionEnd = ReasonEndDate.Value.AddDays(
                    YearsOfService switch
                    {
                        < 2 => 30,
                        < 6 => 90,
                        _ => 180
                    });

                return TerminationDate <= protectionEnd;
            }

            return false;
        }

        /// <summary>
        /// Checks if the termination occurred during the pregnancy protection period.
        /// </summary>
        private bool IsDuringPregnancyProtection()
        {
            if (ReasonStartDate.HasValue) // Assumption: ReasonStartDate = due date or childbirth date
            {
                DateTime pregnancyProtectionEnd = ReasonStartDate.Value.AddDays(16 * 7); // 16 weeks post-childbirth
                return TerminationDate <= pregnancyProtectionEnd;
            }

            return false;
        }

        /// <summary>
        /// Checks if the termination occurred during a care leave protection period.
        /// </summary>
        private bool IsDuringCareLeave()
        {
            if (ReasonStartDate.HasValue)
            {
                DateTime careLeaveEnd = ReasonStartDate.Value.AddMonths(6);
                return TerminationDate <= careLeaveEnd;
            }

            return false;
        }

        /// <summary>
        /// Checks if the termination occurred during an aid action protection period.
        /// </summary>
        private bool IsDuringAidActionProtection()
        {
            if (ReasonEndDate.HasValue)
            {
                DateTime bufferPeriodEnd = ReasonEndDate.Value.AddDays(7 * 4); // 4-week rule
                return TerminationDate <= bufferPeriodEnd;
            }

            return false;
        }

        #endregion

        #region Private Helper Methods for Extension Checks

        /// <summary>
        /// Checks if the termination deadline must be extended due to military service.
        /// </summary>
        /// <param name="terminationNoticeDate">The adjusted termination notice date.</param>
        /// <returns><c>true</c> if extension is required; otherwise, <c>false</c>.</returns>
        private bool CheckMilitaryServiceExtension(DateTime terminationNoticeDate)
        {
            if (ReasonStartDate.HasValue)
            {
                DateTime militaryServiceEnd = ReasonStartDate.Value.AddDays(11);
                DateTime bufferPeriodStart = ReasonStartDate.Value.AddDays(-28); // 4 weeks before
                DateTime bufferPeriodEnd = militaryServiceEnd.AddDays(28); // 4 weeks after

                return terminationNoticeDate >= bufferPeriodStart && terminationNoticeDate <= bufferPeriodEnd;
            }

            return false;
        }

        /// <summary>
        /// Checks if the termination deadline must be extended due to health-related reasons.
        /// </summary>
        /// <param name="terminationNoticeDate">The adjusted termination notice date.</param>
        /// <returns><c>true</c> if extension is required; otherwise, <c>false</c>.</returns>
        private bool CheckHealthOrAccidentExtension(DateTime terminationNoticeDate)
        {
            if (YearsOfService < 2 && terminationNoticeDate <= TerminationDate.AddDays(30))
                return true;
            if (YearsOfService < 6 && terminationNoticeDate <= TerminationDate.AddDays(90))
                return true;
            if (YearsOfService >= 6 && terminationNoticeDate <= TerminationDate.AddDays(180))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if the termination deadline must be extended due to pregnancy.
        /// </summary>
        /// <param name="terminationNoticeDate">The adjusted termination notice date.</param>
        /// <returns><c>true</c> if extension is required; otherwise, <c>false</c>.</returns>
        private bool CheckPregnancyExtension(DateTime terminationNoticeDate)
        {
            if (ReasonStartDate.HasValue)
            {
                DateTime postpartumEnd = ReasonStartDate.Value.AddDays(16 * 7); // 16 weeks after childbirth
                return terminationNoticeDate <= postpartumEnd;
            }

            return false;
        }

        /// <summary>
        /// Checks if the termination deadline must be extended due to care leave.
        /// </summary>
        /// <param name="terminationNoticeDate">The adjusted termination notice date.</param>
        /// <returns><c>true</c> if extension is required; otherwise, <c>false</c>.</returns>
        private bool CheckCareLeaveExtension(DateTime terminationNoticeDate)
        {
            if (ReasonStartDate.HasValue)
            {
                DateTime careEndMax = ReasonStartDate.Value.AddMonths(6);
                return terminationNoticeDate <= careEndMax;
            }

            return false;
        }

        /// <summary>
        /// Checks if the termination deadline must be extended due to participation in aid actions.
        /// </summary>
        /// <param name="terminationNoticeDate">The adjusted termination notice date.</param>
        /// <returns><c>true</c> if extension is required; otherwise, <c>false</c>.</returns>
        private bool CheckAidActionExtension(DateTime terminationNoticeDate)
        {
            if (ReasonEndDate.HasValue)
            {
                DateTime bufferPeriodEnd = ReasonEndDate.Value.AddDays(7 * 4); // 4-week rule
                return terminationNoticeDate <= bufferPeriodEnd;
            }

            return false;
        }

        #endregion

        #region Private Helper Methods for Calculating Extensions

        /// <summary>
        /// Calculates the extension period for military service.
        /// </summary>
        /// <returns>The extended termination notice date as a <see cref="DateTime"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the reason start date is missing or the extended date exceeds the termination date.
        /// </exception>
        private DateTime CalculateMilitaryServiceExtension()
        {
            if (ReasonStartDate.HasValue)
            {
                DateTime militaryServiceEnd = ReasonStartDate.Value.AddDays(11);
                DateTime extendedDate = militaryServiceEnd.AddDays(28); // 4 weeks after end

                // Ensure that the extended date does not exceed TerminationDate
                if (extendedDate > TerminationDate)
                {
                    throw new InvalidOperationException("The extended termination date cannot be after the TerminationDate.");
                }

                return extendedDate;
            }
            else
            {
                throw new InvalidOperationException("Reason start date is required for absence reason 'militärdienst'.");
            }
        }

        /// <summary>
        /// Calculates the extension period for care leave.
        /// </summary>
        /// <returns>The extended termination notice date as a <see cref="DateTime"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the reason start date is missing or the extended date exceeds the termination date.
        /// </exception>
        private DateTime CalculateCareLeaveExtension()
        {
            if (ReasonStartDate.HasValue)
            {
                DateTime careLeaveEnd = ReasonStartDate.Value.AddMonths(6);
                if (careLeaveEnd > TerminationDate)
                {
                    throw new InvalidOperationException("The care leave extension cannot exceed the TerminationDate.");
                }

                return careLeaveEnd;
            }
            else
            {
                throw new InvalidOperationException("Reason start date is required for absence reason 'betreuungsurlaub'.");
            }
        }

        /// <summary>
        /// Calculates the extension period for health or accident-related absences.
        /// </summary>
        /// <param name="terminationNoticeDate">The adjusted termination notice date.</param>
        /// <returns>The extended termination notice date as a <see cref="DateTime"/>.</returns>
        private DateTime CalculateHealthOrAccidentExtension(DateTime terminationNoticeDate)
        {
            if (YearsOfService < 2)
                return terminationNoticeDate.AddDays(30);
            if (YearsOfService < 6)
                return terminationNoticeDate.AddDays(90);
            if (YearsOfService >= 6)
                return terminationNoticeDate.AddDays(180);

            // If no condition matches, return the original date
            return terminationNoticeDate;
        }

        #endregion
    }
}
