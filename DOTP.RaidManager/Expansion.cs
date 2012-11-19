using DOTP.RaidManager.Repository;

namespace DOTP.RaidManager
{
    public class Expansion
    {
        private static ExpansionStore _store = null;

        public string Name
        {
            get;
            set;
        }

        public Expansion()
        {
            Name = null;
        }

        public static ExpansionStore Store
        {
            get
            {
                if (null == _store)
                    _store = new ExpansionStore();

                return _store;
            }
        }
    }
}
