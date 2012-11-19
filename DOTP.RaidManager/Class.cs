using DOTP.RaidManager.Repository;

namespace DOTP.RaidManager
{
    public class Class
    {
        private static ClassStore _store = null;

        public string Name
        {
            get;
            set;
        }

        public Class()
        {
            Name = null;
        }

        public static ClassStore Store
        {
            get
            {
                if (null == _store)
                    _store = new ClassStore();

                return _store;
            }
        }
    }
}
