using DOTP.RaidManager.Repository;

namespace DOTP.RaidManager
{
    public class Settings
    {
        public int TimeZone { get; set; }

        public string GuildName { get; set; }

        public string GuildAbbreviation { get; set; }

        public Settings()
        {
            TimeZone = 0;
            GuildName = null;
            GuildAbbreviation = null;
        }

        public SettingsStore Store
        {
            get
            {
                if (null == _store)
                    _store = new SettingsStore();

                return _store;
            }
        }

        private static SettingsStore _store = null;
    }
}
