using DOTP.RaidManager.Stores;

namespace DOTP.RaidManager
{
    public class Role
    {
        private static RoleStore _store = null;

        public string Name
        {
            get;
            set;
        }

        public Role()
        {
            Name = null;
        }

        public static RoleStore Store
        {
            get
            {
                if (null == _store)
                    _store = new RoleStore();

                return _store;
            }
        }
    }
}
