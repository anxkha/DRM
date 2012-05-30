using System.Collections.Generic;

namespace DOTP.Users
{
    public class User
    {
        public int ID
        {
            get;
            private set;
        }

        public string FirstName
        {
            get;
            private set;
        }

        public string Email
        {
            get;
            private set;
        }

        public bool IsRaidTeam
        {
            get;
            set;
        }

        public bool IsAdmin
        {
            get;
            set;
        }

        public User(int id, string firstName, string email, bool isRaidTeam, bool isAdmin)
        {
            ID = id;
            FirstName = firstName;
            Email = email;
            IsRaidTeam = isRaidTeam;
            IsAdmin = isAdmin;
        }

        public static class Store
        {
            private static Dictionary<int, User> _userStore = null;

            private static void EnsureSetup()
            {
                if (null != _userStore)
                    return;

                _userStore = new Dictionary<int, User>();
            }

            public static void Add(int id, User user)
            {
                EnsureSetup();

                User curUser;

                if (!_userStore.TryGetValue(id, out curUser))
                {
                    _userStore.Add(id, user);
                }
                else
                {
                    _userStore[id] = user;
                }
            }

            public static User GetByID(int id)
            {
                EnsureSetup();

                User user;

                _userStore.TryGetValue(id, out user);

                return user;
            }

            public static User GetByEmail(string email)
            {
                EnsureSetup();

                foreach (var user in _userStore.Values)
                {
                    if (user.Email == email)
                        return user;
                }

                return null;
            }
        }
    }
}
