
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
    /// Represents a tooltip with a message and a specified display duration.
    /// </summary>
    public class Tooltip
    {
        /// <summary>
        /// Gets the message displayed in the tooltip.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the duration for which the tooltip is displayed.
        /// </summary>
        public TimeSpan DisplayDuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tooltip"/> class with the specified message and display duration.
        /// </summary>
        /// <param name="message">The message to display in the tooltip.</param>
        /// <param name="displayDuration">
        /// The duration for which the tooltip is displayed.
        /// If not specified, defaults to one minute.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown when the <paramref name="message"/> is null, empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the <paramref name="displayDuration"/> is negative or zero.
        /// </exception>
        public Tooltip(string message, TimeSpan? displayDuration = null)
        {
            // Validate that the message is not null, empty, or whitespace
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Tooltip-Nachricht darf nicht null, leer oder nur aus Leerzeichen bestehen.", nameof(message));
            }

            // Validate that the display duration is positive
            TimeSpan duration = displayDuration ?? TimeSpan.FromSeconds(10);
            if (duration <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(displayDuration), "Die Anzeige-Dauer muss eine positive Zeitspanne sein.");
            }

            Message = message;
            DisplayDuration = duration;
        }
    }
}
