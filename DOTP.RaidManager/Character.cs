using DOTP.RaidManager.Repository;

namespace DOTP.RaidManager
{
    public class Character
    {
        private static CharacterStore _store = null;

        public string Name
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public string Class
        {
            get;
            set;
        }

        public string Race
        {
            get;
            set;
        }

        public int AccountID
        {
            get;
            set;
        }

        public int PrimarySpecialization
        {
            get;
            set;
        }

        public int SecondarySpecialization
        {
            get;
            set;
        }

        public Character()
        {
            Name = "";
            Level = 0;
            Class = null;
            Race = null;
            AccountID = 0;
            PrimarySpecialization = 0;
            SecondarySpecialization = 0;
        }

        public static CharacterStore Store
        {
            get
            {
                if (null == _store)
                    _store = new CharacterStore();

                return _store;
            }
        }
    }
}
