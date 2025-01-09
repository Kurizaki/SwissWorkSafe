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
    /// ViewModel for calculating salary continuation based on start and event dates.
    /// </summary>
    public class SalaryContinuationViewModel : ViewModelBase
    {
        private DateTime _startDate = DateTime.Today;
        private DateTime _eventDate = DateTime.Today.AddMonths(1);
        private int _duration;
        private string _result;
        private string _weeksResult;
        private string _monthsResult;
        private string _errorMessage; // Property for error messages

        // New private fields for the scale
        private string _selectedScale;
        private List<string> _scales;

        /// <summary>
        /// Gets the TooltipViewModel containing tooltips for the UI.
        /// </summary>
        public TooltipViewModel TooltipViewModel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryContinuationViewModel"/> class.
        /// </summary>
        /// <param name="navigateAction">Action to handle navigation.</param>
        public SalaryContinuationViewModel(Action<string> navigateAction)
            : base(navigateAction)
        {
            // Initialize the CalculateCommand with the CalculateSalaryContinuation method
            CalculateCommand = new RelayCommand(parameter => CalculateSalaryContinuation());

            // Initialize tooltips with relevant messages
            TooltipViewModel = new TooltipViewModel(new List<Tooltip>
            {
                new Tooltip("Wählen Sie das Datum aus, ab dem Ihr Anspruch auf Lohnfortzahlung beginnt."),
                new Tooltip("Geben Sie das Datum des Ereignisses ein, das den Anspruch auf Lohnfortzahlung auslöst."),
                new Tooltip("Klicken Sie hier, um die Dauer und den Anspruch auf Lohnfortzahlung basierend auf den eingegebenen Daten zu berechnen."),
                new Tooltip("Wählen Sie die Skala (Basler, Berner, Zürcher) aus, um die entsprechenden Berechnungsregeln anzuwenden.") // New tooltip for the scale
            });

            // Initialize the list of scales
            Scales = new List<string> { "Basel", "Bern", "Zürich" };

            // Set a default scale (optional)
            SelectedScale = Scales[0];
        }

        /// <summary>
        /// Gets or sets the start date of employment.
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
        /// Gets or sets the event date that triggers salary continuation.
        /// </summary>
        public DateTime EventDate
        {
            get => _eventDate;
            set
            {
                if (_eventDate != value)
                {
                    _eventDate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the duration of salary continuation in days.
        /// </summary>
        public int Duration
        {
            get => _duration;
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the result message displaying the total duration.
        /// </summary>
        public string Result
        {
            get => _result;
            set
            {
                if (_result != value)
                {
                    _result = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the result message displaying weeks and remaining days.
        /// </summary>
        public string WeeksResult
        {
            get => _weeksResult;
            set
            {
                if (_weeksResult != value)
                {
                    _weeksResult = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the result message displaying months and remaining days.
        /// </summary>
        public string MonthsResult
        {
            get => _monthsResult;
            set
            {
                if (_monthsResult != value)
                {
                    _monthsResult = value;
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
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of available scales for selection.
        /// </summary>
        public List<string> Scales
        {
            get => _scales;
            set
            {
                if (_scales != value)
                {
                    _scales = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently selected scale.
        /// </summary>
        public string SelectedScale
        {
            get => _selectedScale;
            set
            {
                if (_selectedScale != value)
                {
                    _selectedScale = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the command to trigger salary continuation calculation.
        /// </summary>
        public ICommand CalculateCommand { get; }

        /// <summary>
        /// Calculates the salary continuation duration and updates the result properties.
        /// </summary>
        private void CalculateSalaryContinuation()
        {
            try
            {
                // Check if a scale is selected
                if (string.IsNullOrWhiteSpace(SelectedScale))
                {
                    throw new ArgumentException("Bitte wählen Sie eine gültige Skala aus.");
                }

                // Calculate the total duration in days based on start and event dates and selected scale
                Duration = SalaryContinuation.CalculateSalaryContinuationDuration(StartDate, EventDate, SelectedScale);

                // Break down the duration into weeks and months
                var (weeks, remainingDays, months, remainingDaysInMonth) = SalaryContinuation.CalculateWeeksAndMonths(Duration);

                // Update the result properties with formatted strings
                WeeksResult = $"{weeks} Wochen und {remainingDays} Tage";
                MonthsResult = $"{months} Monate und {remainingDaysInMonth} Tage";
                Result = $"Dauer: {Duration} Tage";

                // Clear any existing error messages
                ErrorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                // Handle and display errors
                ErrorMessage = $"Fehler: {ex.Message}";
                Result = string.Empty;
                WeeksResult = string.Empty;
                MonthsResult = string.Empty;
            }
        }
    }
}
