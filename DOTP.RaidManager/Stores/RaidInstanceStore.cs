using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Stores
{
    public class RaidInstanceStore
    {
        private static List<RaidInstance> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string RAID_INSTANCE_SELECT = @"
SELECT [ID], [Raid], [Name], [Description], [InviteTime], [StartTime], [IsArchived]
FROM [DRM].[dbo].[RaidInstance]
";

        public RaidInstanceStore()
        {
            _loaded = false;
            _cache = null;

            _lock = new ReaderWriterLock();
        }

        public RaidInstance ReadOneOrDefault(Func<RaidInstance, bool> func)
        {
            EnsureLoaded();

            foreach (var spec in _cache)
            {
                if (func(spec))
                    return spec;
            }

            return default(RaidInstance);
        }

        public List<RaidInstance> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<RaidInstance>();

            foreach (var entry in _cache)
            {
                newList.Add(entry);
            }

            return newList.Count > 0 ? newList : null;
        }

        public List<RaidInstance> ReadAll(Func<RaidInstance, bool> func)
        {
            EnsureLoaded();

            var newList = new List<RaidInstance>();

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

                    if (null == _cache)
                        _cache = new List<RaidInstance>();

                    Connection.ExecuteSql(new Query(RAID_INSTANCE_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            if (null != _cache.Find(s => reader[0].ToString() == s.Name))
                                return;

                            var newRaidInstance = new RaidInstance()
                            {
                                ID = (int)reader[0],
                                Raid = reader[1].ToString(),
                                Name = reader[2].ToString(),
                                Description = reader[3].ToString(),
                                InviteTime = (DateTime)reader[4],
                                StartTime = (DateTime)reader[5],
                                Archived = (bool)reader[6]
                            };

                            _cache.Add(newRaidInstance);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
