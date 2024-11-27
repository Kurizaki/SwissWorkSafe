namespace SwissWorkSafe.Models.Settings
{
    public class Language
    {
        public string LanguageCode { get; set; }
        public string Region { get; set; }

        public void SetLanguage(string languageCode) { }
        public string GetLanguage() { return null; }
    }
}
