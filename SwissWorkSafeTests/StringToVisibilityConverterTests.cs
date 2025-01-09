using System.Globalization;
using System.Windows;
using SwissWorkSafe.Models.Converters;

namespace SwissWorkSafeTests;

[TestClass]
public class StringToVisibilityConverterTests
{
    private StringToVisibilityConverter _converter;

    [TestInitialize]
    public void Setup()
    {
        _converter = new StringToVisibilityConverter();
    }

    [TestMethod]
    public void Convert_NonEmptyString_ReturnsVisible()
    {
        // Arrange
        var input = "Hello World";
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = _converter.Convert(input, typeof(Visibility), null, culture);

        // Assert
        Assert.AreEqual(Visibility.Visible, result, "Nicht-leere Zeichenkette sollte Visibility.Visible zurückgeben.");
    }

    [TestMethod]
    public void Convert_EmptyString_ReturnsCollapsed()
    {
        // Arrange
        var input = "";
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = _converter.Convert(input, typeof(Visibility), null, culture);

        // Assert
        Assert.AreEqual(Visibility.Collapsed, result, "Leere Zeichenkette sollte Visibility.Collapsed zurückgeben.");
    }

    [TestMethod]
    public void Convert_Null_ReturnsCollapsed()
    {
        // Arrange
        string input = null;
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = _converter.Convert(input, typeof(Visibility), null, culture);

        // Assert
        Assert.AreEqual(Visibility.Collapsed, result, "Null-Wert sollte Visibility.Collapsed zurückgeben.");
    }

    [TestMethod]
    public void Convert_WhitespaceString_ReturnsCollapsed()
    {
        // Arrange
        var input = "   ";
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = _converter.Convert(input, typeof(Visibility), null, culture);

        // Assert
        Assert.AreEqual(Visibility.Collapsed, result, "Zeichenkette mit nur Leerzeichen sollte Visibility.Collapsed zurückgeben.");
    }

    [TestMethod]
    public void Convert_NonStringType_ReturnsCollapsed()
    {
        // Arrange
        var input = 12345; // Integer-Eingabe
        var culture = CultureInfo.InvariantCulture;

        // Act
        var result = _converter.Convert(input, typeof(Visibility), null, culture);

        // Assert
        Assert.AreEqual(Visibility.Collapsed, result, "Nicht-Zeichenketten-Typ sollte Visibility.Collapsed zurückgeben.");
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void ConvertBack_ThrowsNotImplementedException()
    {
        // Arrange
        var input = Visibility.Visible;
        var culture = CultureInfo.InvariantCulture;

        // Act
        _converter.ConvertBack(input, typeof(string), null, culture);

        // Assert wird durch ExpectedException behandelt
    }


}