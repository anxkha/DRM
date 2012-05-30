using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Stores
{
    public class RaceClassesStore
    {
        private static Dictionary<string, RaceClasses> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string RACE_TO_CLASSES_SELECT = "SELECT [Race], [Class] FROM [DRM].[dbo].[ClassToRace] ORDER BY [Race]";

        public RaceClassesStore()
        {
            _loaded = false;
            _cache = null;

            _lock = new ReaderWriterLock();
        }

        public List<RaceClasses> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<RaceClasses>();

            foreach (var entry in _cache.Values)
            {
                newList.Add(entry);
            }

            return newList.Count > 0 ? newList : null;
        }

        public RaceClasses ReadOneOrDefault(string race)
        {
            RaceClasses rc;

            EnsureLoaded();

            _cache.TryGetValue(race, out rc);

            return rc;
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
                        _cache = new Dictionary<string, RaceClasses>();

                    Connection.ExecuteSql(new Query(RACE_TO_CLASSES_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            var race = reader[0].ToString();
                            var clss = reader[1].ToString();

                            RaceClasses rc;

                            if (!_cache.TryGetValue(race, out rc))
                            {
                                rc = new RaceClasses(race);
                                _cache.Add(race, rc);
                            }

                            rc.Classes.Add(clss);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
