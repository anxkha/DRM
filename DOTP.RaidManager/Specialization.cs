using DOTP.RaidManager.Stores;

namespace DOTP.RaidManager
{
    public class Specialization
    {
        private static SpecializationStore _store = null;

        public int ID
        {
            get;
            private set;
        }

        public string Class
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Role
        {
            get;
            private set;
        }

        public Specialization(int id, string clss, string name, string role)
        {
            Class = clss;
            Name = name;
            Role = role;
            ID = id;
        }

        public static SpecializationStore Store
        {
            get
            {
                if (null == _store)
                    _store = new SpecializationStore();

                return _store;
            }
        }
    }
}
