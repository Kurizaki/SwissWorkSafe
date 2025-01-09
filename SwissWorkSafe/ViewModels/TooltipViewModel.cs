// TooltipViewModel.cs
using System.Timers;
using SwissWorkSafe.Models.Core;
using Timer = System.Timers.Timer;

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
    /// ViewModel responsible for managing and displaying tooltips in the UI.
    /// </summary>
    public class TooltipViewModel : ViewModelBase, IDisposable
    {
        private Tooltip _currentTooltip;
        private readonly List<Tooltip> _tooltips;
        private int _tooltipIndex;
        private readonly Timer _tooltipTimer;
        private bool _disposed = false; // To detect redundant calls

        /// <summary>
        /// Gets or sets the currently displayed tooltip.
        /// </summary>
        public Tooltip CurrentTooltip
        {
            get => _currentTooltip;
            private set
            {
                if (_currentTooltip != value)
                {
                    _currentTooltip = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipViewModel"/> class with a list of tooltips.
        /// </summary>
        /// <param name="tooltips">A list of <see cref="Tooltip"/> instances to cycle through.</param>
        /// <exception cref="ArgumentException">Thrown when the tooltip list is null or empty.</exception>
        public TooltipViewModel(List<Tooltip> tooltips)
        {
            if (tooltips == null || tooltips.Count == 0)
                throw new ArgumentException("Tooltip list cannot be null or empty.", nameof(tooltips));

            _tooltips = tooltips;
            _tooltipIndex = 0;
            CurrentTooltip = _tooltips[_tooltipIndex];

            // Initialize the timer with the display time of the first tooltip
            _tooltipTimer = new Timer(_tooltips[_tooltipIndex].DisplayDuration.TotalMilliseconds);
            _tooltipTimer.Elapsed += OnTooltipTimerElapsed;
            _tooltipTimer.AutoReset = false; // Prevent overlapping
            _tooltipTimer.Start();
        }

        /// <summary>
        /// Event handler for the tooltip timer's Elapsed event. Switches to the next tooltip.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Event data.</param>
        private void OnTooltipTimerElapsed(object sender, ElapsedEventArgs e)
        {
            SwitchTooltip();
        }

        /// <summary>
        /// Switches to the next tooltip in the list and resets the timer.
        /// </summary>
        private void SwitchTooltip()
        {
            _tooltipIndex = (_tooltipIndex + 1) % _tooltips.Count;
            CurrentTooltip = _tooltips[_tooltipIndex];

            // Reset the timer with the display duration of the new tooltip
            _tooltipTimer.Interval = _tooltips[_tooltipIndex].DisplayDuration.TotalMilliseconds;
            _tooltipTimer.Start();
        }

        /// <summary>
        /// Stops the tooltip timer.
        /// </summary>
        public void StopTimer()
        {
            _tooltipTimer?.Stop();
        }

        /// <summary>
        /// Starts the tooltip timer.
        /// </summary>
        public void StartTimer()
        {
            if (_tooltipTimer != null && !_tooltipTimer.Enabled)
            {
                _tooltipTimer.Start();
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="TooltipViewModel"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="TooltipViewModel"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources
                    _tooltipTimer?.Stop();
                    _tooltipTimer?.Dispose();
                }

                // Note: No unmanaged resources to release

                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor to ensure resources are released.
        /// </summary>
        ~TooltipViewModel()
        {
            Dispose(false);
        }
    }
}
