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

            _signups = RaidSignup.Store.ReadAll(ID);
            _raid = Raid.Store.ReadOneOrDefault(r => r.Name == _raidInstance.Raid);

            return true;
        }

        public List<RaidSignup> GetTankSignups()
        {
            var tanks = new List<RaidSignup>();

            foreach (RaidSignup signup in GetRosteredCharacters())
            {
                Specialization spec = GetRosteredSpecialization(signup);
                if ("Tank" == spec.Role)
                    tanks.Add(signup);
            }

            return tanks;
        }

        public List<RaidSignup> GetHealerSignups()
        {
            var healers = new List<RaidSignup>();

            foreach (RaidSignup signup in GetRosteredCharacters())
            {
                Specialization spec = GetRosteredSpecialization(signup);
                if ("Healer" == spec.Role)
                    healers.Add(signup);
            }

            return healers;
        }

        public List<RaidSignup> GetMeleeSignups()
        {
            var melee = new List<RaidSignup>();

            foreach (RaidSignup signup in GetRosteredCharacters())
            {
                Specialization spec = GetRosteredSpecialization(signup);
                if ("Melee" == spec.Role)
                    melee.Add(signup);
            }

            return melee;
        }

        public List<RaidSignup> GetRangedSignups()
        {
            var ranged = new List<RaidSignup>();

            foreach (RaidSignup signup in GetRosteredCharacters())
            {
                Specialization spec = GetRosteredSpecialization(signup);
                if ("Ranged" == spec.Role)
                    ranged.Add(signup);
            }

            return ranged;
        }

        private Specialization GetRosteredSpecialization(RaidSignup signup)
        {
            var character = Character.Store.ReadOneOrDefault(c => c.Name == signup.Character);
            var specId = 1 == signup.RosteredSpecialization ? character.PrimarySpecialization : character.SecondarySpecialization;
            return Specialization.Store.ReadOneOrDefault(s => s.ID == specId);
        }

        public List<RaidSignup> GetRosteredCharacters()
        {
            return Signups.FindAll(s => s.IsRostered);
        }
    }
}
