using System.Globalization;
using System.Windows;
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
    /// Converts a non-empty string to <see cref="Visibility.Visible"/> and an empty or null string to <see cref="Visibility.Collapsed"/>.
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a string value to a <see cref="Visibility"/> enumeration.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="targetType">The target binding type.</param>
        /// <param name="parameter">An optional parameter for the converter.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>
        /// <see cref="Visibility.Visible"/> if the input string is not null or whitespace;
        /// otherwise, <see cref="Visibility.Collapsed"/>.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && !string.IsNullOrWhiteSpace(str))
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        /// <summary>
        /// Not implemented. Converts back from <see cref="Visibility"/> to a string.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">An optional parameter for the converter.</param>
        /// <param name="culture">The culture information.</param>
        /// <exception cref="NotImplementedException">Always thrown as conversion back is not supported.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("StringToVisibilityConverter unterstützt ConvertBack nicht.");
        }
    }
}
