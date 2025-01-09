using System.Globalization;
using System.Windows.Data;

namespace SwissWorkSafe.Models.Converters
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
    /// Converts a boolean value to a "Ja" or "Nein" string representation.
    /// </summary>
    public class BoolToYesNoConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a corresponding "Ja" or "Nein" string.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target binding type (unused).</param>
        /// <param name="parameter">An optional parameter for the converter (unused).</param>
        /// <param name="culture">The culture to use in the converter (unused).</param>
        /// <returns>"Ja" if <paramref name="value"/> is <c>true</c>; otherwise, "Nein".</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? "Ja" : "Nein";
            }
            return "Nein";
        }

        /// <summary>
        /// ConvertBack is not supported and will throw an exception if called.
        /// </summary>
        /// <param name="value">The value produced by the binding target (unused).</param>
        /// <param name="targetType">The type to convert to (unused).</param>
        /// <param name="parameter">An optional parameter for the converter (unused).</param>
        /// <param name="culture">The culture to use in the converter (unused).</param>
        /// <exception cref="NotSupportedException">Always thrown as ConvertBack is not supported.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("BoolToYesNoConverter unterstützt ConvertBack nicht.");
        }
    }
}