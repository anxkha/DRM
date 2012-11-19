using DOTP.RaidManager.Repository;
using System;

namespace DOTP.RaidManager
{
    public class RaidInstance
    {
        private static RaidInstanceStore _store = null;

        public int ID
        {
            get;
            set;
        }

        public string Raid
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public DateTime InviteTime
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public bool Archived
        {
            get;
            set;
        }

        public RaidInstance()
        {
            ID = 0;
            Raid = null;
            Name = null;
            Description = null;
            InviteTime = DateTime.Now;
            StartTime = DateTime.Now;
            Archived = false;
        }

        public static RaidInstanceStore Store
        {
            get
            {
                if (null == _store)
                    _store = new RaidInstanceStore();

                return _store;
            }
        }
    }
}
