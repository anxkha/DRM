using System.Collections.Generic;

namespace DOTP.RaidManager
{
    public class RaidDetails
    {
        private RaidInstance _raidInstance;
        private Raid _raid;
        private List<RaidSignup> _signups;

        public RaidDetails(int id)
        {
            ID = id;
        }

        public int ID
        {
            get;
            private set;
        }

        public RaidInstance RaidInstance
        {
            get { return _raidInstance; }
        }

        public Raid Raid
        {
            get { return _raid; }
        }

        public List<RaidSignup> Signups
        {
            get { return _signups; }
        }

        public bool Initialize()
        {
            _raidInstance = RaidInstance.Store.ReadOneOrDefault(ri => ri.ID == ID);

            if (null == _raidInstance)
                return false;

            _signups = RaidSignup.Store.ReadAll(rs => rs.RaidInstanceID == ID);
            _raid = Raid.Store.ReadOneOrDefault(r => r.Name == _raidInstance.Raid);

            return true;
        }
    }
}
