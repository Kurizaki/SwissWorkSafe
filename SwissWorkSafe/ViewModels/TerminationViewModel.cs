using System.Windows.Input;
using SwissWorkSafe.Commands;
using SwissWorkSafe.Models.Core;

namespace SwissWorkSafe.ViewModels
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
    /// ViewModel for managing termination details and calculations.
    /// </summary>
    public class TerminationViewModel : ViewModelBase
    {
        private DateTime _startDate = DateTime.Today;
        private DateTime _terminationDate = DateTime.Today.AddMonths(1);
        private int _absenceDays;
        private string _absenceReason;
        private DateTime? _calculatedEndDate;
        private bool _isExtended;
        private bool _isTerminationInvalid; // Indicates if the termination is invalid
        private string _errorMessage;
        private DateTime? _reasonEndDate; // End date for the reason of absence
        private int _yearsOfService = 1; // Default value

        /// <summary>
        /// Gets the TooltipViewModel containing tooltips for the UI.
        /// </summary>
        public TooltipViewModel TooltipViewModel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminationViewModel"/> class.
        /// </summary>
        /// <param name="navigateAction">Action to handle navigation.</param>
        public TerminationViewModel(Action<string> navigateAction)
            : base(navigateAction)
        {
            CalculateCommand = new RelayCommand(ExecuteCalculate, CanExecuteCalculate);

            // Initialize tooltips with relevant messages
            TooltipViewModel = new TooltipViewModel(new List<Tooltip>
            {
                new Tooltip("Tragen Sie hier das Datum ein, an dem das Arbeitsverhältnis begonnen hat."),
                new Tooltip("Geben Sie das offizielle Datum ein, an dem die Kündigung ausgesprochen wird."),
                new Tooltip("Klicken Sie auf diese Schaltfläche, um das Enddatum der Kündigungsfrist zu ermitteln."),
                new Tooltip("Optional: Geben Sie das Enddatum für den Abwesenheitsgrund ein.")
            });
        }

        /// <summary>
        /// Gets or sets the employment start date.
        /// </summary>
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the termination date.
        /// </summary>
        public DateTime TerminationDate
        {
            get => _terminationDate;
            set
            {
                if (_terminationDate != value)
                {
                    _terminationDate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of absence days.
        /// </summary>
        public int AbsenceDays
        {
            get => _absenceDays;
            set
            {
                if (_absenceDays != value)
                {
                    _absenceDays = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the reason for absence.
        /// </summary>
        public string AbsenceReason
        {
            get => _absenceReason;
            set
            {
                if (_absenceReason != value)
                {
                    _absenceReason = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the end date for the reason of absence.
        /// </summary>
        public DateTime? ReasonEndDate
        {
            get => _reasonEndDate;
            set
            {
                if (_reasonEndDate != value)
                {
                    _reasonEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the calculated end date of the termination period.
        /// </summary>
        public DateTime? CalculatedEndDate
        {
            get => _calculatedEndDate;
            private set
            {
                if (_calculatedEndDate != value)
                {
                    _calculatedEndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the termination deadline is extended.
        /// </summary>
        public bool IsExtended
        {
            get => _isExtended;
            private set
            {
                if (_isExtended != value)
                {
                    _isExtended = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Indicates whether the termination is invalid.
        /// </summary>
        public bool IsTerminationInvalid
        {
            get => _isTerminationInvalid;
            private set
            {
                if (_isTerminationInvalid != value)
                {
                    _isTerminationInvalid = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the error message to display in case of calculation failures.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of years of service.
        /// </summary>
        public int YearsOfService
        {
            get => _yearsOfService;
            set
            {
                if (_yearsOfService != value)
                {
                    _yearsOfService = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the command to trigger termination calculations.
        /// </summary>
        public ICommand CalculateCommand { get; }

        /// <summary>
        /// Determines whether the calculate command can execute.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns><c>true</c> if StartDate is on or before TerminationDate and AbsenceReason is provided; otherwise, <c>false</c>.</returns>
        private bool CanExecuteCalculate(object parameter)
        {
            return StartDate <= TerminationDate && !string.IsNullOrWhiteSpace(AbsenceReason);
        }

        /// <summary>
        /// Executes the termination calculation and updates relevant properties.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void ExecuteCalculate(object parameter)
        {
            try
            {
                // Validate that the absence reason is provided
                if (string.IsNullOrWhiteSpace(AbsenceReason))
                {
                    throw new ArgumentException("Der Abwesenheitsgrund darf nicht leer sein.");
                }

                // Normalize the absence reason
                string normalizedAbsenceReason = AbsenceReason.Trim().ToLowerInvariant();

                // Initialize the Termination object
                var termination = new Termination(
                    startDate: StartDate,
                    terminationDate: TerminationDate,
                    sickDays: AbsenceDays,
                    absenceReason: normalizedAbsenceReason,
                    reasonStartDate: GetReasonStartDate(normalizedAbsenceReason),
                    yearsOfService: YearsOfService
                );

                // Set the optional ReasonEndDate, if provided
                if (ReasonEndDate.HasValue)
                {
                    termination.SetAbsenceDetails(
                        reason: AbsenceReason,
                        startDate: GetReasonStartDate(normalizedAbsenceReason),
                        endDate: ReasonEndDate
                    );
                }

                // Perform calculations
                CalculatedEndDate = termination.CalculateTerminationDeadline();
                IsExtended = termination.MustBeExtended(TerminationDate);
                IsTerminationInvalid = termination.IsTerminationInvalid();

                // Clear any existing error messages
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                // Handle and display errors
                ErrorMessage = $"Fehler: {ex.Message}";
                CalculatedEndDate = null;
                IsExtended = false;
                IsTerminationInvalid = false;
            }
        }

        /// <summary>
        /// Determines the ReasonStartDate based on the absence reason.
        /// </summary>
        /// <param name="absenceReason">The absence reason in lowercase.</param>
        /// <returns>The start date for the absence reason.</returns>
        /// <exception cref="ArgumentException">Thrown if the absence reason requires a specific ReasonStartDate and none is provided.</exception>
        private DateTime GetReasonStartDate(string absenceReason)
        {
            // Implement specific logic to determine ReasonStartDate based on absence reason
            switch (absenceReason)
            {
                case "militärdienst":
                case "betreuungsurlaub":
                    // Ensure that StartDate is set
                    if (StartDate == default)
                    {
                        throw new ArgumentException($"Ein ReasonStartDate ist erforderlich für den Abwesenheitsgrund '{absenceReason}'.");
                    }
                    return StartDate;

                case "schwangerschaft":
                case "hilfsaktion":
                case "krankheit":
                case "unfall":
                    // These reasons might not require a specific ReasonStartDate or have different logic
                    return StartDate; // Adjust as necessary

                default:
                    throw new ArgumentException($"Der Abwesenheitsgrund '{absenceReason}' ist nicht erkannt.");
            }
        }
    }
}
