using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Stores
{
    public class ClassStore
    {
        private static List<Class> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string CLASS_SELECT = "SELECT * FROM [DRM].[dbo].[Class]";

        public ClassStore()
        {
            _loaded = false;
            _cache = null;

            _lock = new ReaderWriterLock();
        }

        public List<Class> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<Class>();

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

                    if (null == _cache)
                        _cache = new List<Class>();

                    Connection.ExecuteSql(new Query(CLASS_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            if (null != _cache.Find(c => reader[0].ToString() == c.Name))
                                return;

                            var newClass = new Class()
                            {
                                Name = reader[0].ToString()
                            };

                            _cache.Add(newClass);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
