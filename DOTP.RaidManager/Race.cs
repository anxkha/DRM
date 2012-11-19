using DOTP.RaidManager.Repository;

namespace DOTP.RaidManager
{
    public class Race
    {
        private static RaceStore _store = null;

        public string Name
        {
            get;
            set;
        }

        public Race()
        {
            Name = null;
        }

        public static RaceStore Store
        {
            get
            {
                if (null == _store)
                    _store = new RaceStore();

                return _store;
            }
        }
    }
}
