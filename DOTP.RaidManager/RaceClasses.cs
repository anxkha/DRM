using DOTP.RaidManager.Repository;
using System.Collections.Generic;

namespace DOTP.RaidManager
{
    public class RaceClasses
    {
        private static RaceClassesStore _store = null;

        public string Race
        {
            get;
            set;
        }

        public List<string> Classes
        {
            get;
            private set;
        }

        public RaceClasses()
        {
            Race = null;
            Classes = new List<string>();
        }

        public RaceClasses(string race)
        {
            Race = race;
            Classes = new List<string>();
        }

        public static RaceClassesStore Store
        {
            get
            {
                if (null == _store)
                    _store = new RaceClassesStore();

                return _store;
            }
        }
    }
}
