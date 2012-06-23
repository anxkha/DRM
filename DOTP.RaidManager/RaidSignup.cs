using DOTP.RaidManager.Stores;
using System;

namespace DOTP.RaidManager
{
    public class RaidSignup
    {
        private static RaidSignupStore _store = null;

        public int RaidInstanceID
        {
            get;
            set;
        }

        public string Character
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }

        public bool IsRostered
        {
            get;
            set;
        }

        public bool IsCancelled
        {
            get;
            set;
        }

        public int RosteredSpecialization
        {
            get;
            set;
        }

        public DateTime SignupDate
        {
            get;
            set;
        }

        public RaidSignup()
        {
            RaidInstanceID = 0;
            Character = null;
            Comment = null;
            IsRostered = false;
            IsCancelled = true;
            RosteredSpecialization = 0;
            SignupDate = DateTime.Now;
        }

        public static RaidSignupStore Store
        {
            get
            {
                if (null == _store)
                    _store = new RaidSignupStore();

                return _store;
            }
        }
    }
}
