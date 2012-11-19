using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Repository
{
    public class RoleStore
    {
        private static List<Role> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string ROLE_SELECT = "SELECT * FROM [Role]";

        public RoleStore()
        {
            _loaded = false;
            _cache = new List<Role>();

            _lock = new ReaderWriterLock();
        }

        public List<Role> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<Role>();

            foreach (var entry in _cache)
            {
                newList.Add(entry);
            }

            return newList.Count > 0 ? newList : null;
        }

        private void EnsureLoaded()
        {
            using (new ReaderLock(_lock))
            {
                if (_loaded) return;

                using (new WriterLock(_lock))
                {
                    if (_loaded) return;

                    Connection.ExecuteSql(new Query(ROLE_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            if (null != _cache.Find(c => reader[0].ToString() == c.Name))
                                return;

                            var newRole = new Role()
                            {
                                Name = reader[0].ToString()
                            };

                            _cache.Add(newRole);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
