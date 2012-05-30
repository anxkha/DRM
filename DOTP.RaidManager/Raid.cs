using DOTP.RaidManager.Stores;
using System.Collections.Generic;

namespace DOTP.RaidManager
{
    public class Raid
    {
        private static RaidStore _store = null;

        public string Name
        {
            get;
            set;
        }

        public string Expansion
        {
            get;
            set;
        }

        public int MaxPlayers
        {
            get;
            set;
        }

        public int MinimumLevel
        {
            get;
            set;
        }

        public int NumberOfBosses
        {
            get;
            set;
        }

        public Raid()
        {
            Name = null;
            Expansion = null;
            MaxPlayers = 0;
            MinimumLevel = 0;
            NumberOfBosses = 0;
        }

        public static RaidStore Store
        {
            get
            {
                if (null == _store)
                    _store = new RaidStore();

                return _store;
            }
        }
    }
}
