using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Stores
{
    public class RaceStore
    {
        private static List<Race> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string RACE_SELECT = "SELECT * FROM [DRM].[dbo].[Race]";

        public RaceStore()
        {
            _loaded = false;
            _cache = null;

            _lock = new ReaderWriterLock();
        }

        public List<Race> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<Race>();

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
                        _cache = new List<Race>();

                    Connection.ExecuteSql(new Query(RACE_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            if (null != _cache.Find(c => reader[0].ToString() == c.Name))
                                return;

                            var newRace = new Race()
                            {
                                Name = reader[0].ToString()
                            };

                            _cache.Add(newRace);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
