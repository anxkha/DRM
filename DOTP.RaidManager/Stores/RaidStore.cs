using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Stores
{
    public class RaidStore
    {
        private static List<Raid> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string RAID_SELECT = @"
SELECT [Name], [Expansion], [MaxPlayers], [MinimumLevel], [NumberOfBosses]
FROM [DRM].[dbo].[Raid]
";

        public RaidStore()
        {
            _loaded = false;
            _cache = new List<Raid>();

            _lock = new ReaderWriterLock();
        }

        public Raid ReadOneOrDefault(Func<Raid, bool> func)
        {
            EnsureLoaded();

            foreach (var spec in _cache)
            {
                if (func(spec))
                    return spec;
            }

            return default(Raid);
        }

        public List<Raid> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<Raid>();

            foreach (var entry in _cache)
            {
                newList.Add(entry);
            }

            return newList.Count > 0 ? newList : null;
        }

        public List<Raid> ReadAll(Func<Raid, bool> func)
        {
            EnsureLoaded();

            var newList = new List<Raid>();

            foreach (var entry in _cache)
            {
                if (func(entry))
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

                    Connection.ExecuteSql(new Query(RAID_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            if (null != _cache.Find(s => reader[0].ToString() == s.Name))
                                return;

                            var newRaid = new Raid()
                            {
                                Name = reader[0].ToString(),
                                Expansion = reader[1].ToString(),
                                MaxPlayers = (int)reader[2],
                                MinimumLevel = (int)reader[3],
                                NumberOfBosses = (int)reader[4]
                            };

                            _cache.Add(newRaid);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
