namespace SwissWorkSafe.Models.Settings
{
    public class Colors
    {
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string AccentColor { get; set; }
        public string ThemeMode { get; set; }

        public void SetPrimaryColor(string color) { }
        public void SetThemeMode(string mode) { }
        public void ApplyDarkMode() { }
        public void ApplyLightMode() { }
    }
}
