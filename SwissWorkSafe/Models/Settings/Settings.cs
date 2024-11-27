using SwissWorkSafe.Models.Core;

namespace SwissWorkSafe.Models.Settings
{
    public class Settings
    {
        public Language Language { get; set; }
        public Tooltips? Tooltips { get; set; }
        public Colors? Colors { get; set; }
        public Autostart Autostart { get; set; }

        public void SetLanguage(Language? language) { }
        public void SetTooltips(bool enabled) { }
        public void SetColors(Colors? colors) { }
        public void SetAutostart(bool enabled) { }
    }
}
