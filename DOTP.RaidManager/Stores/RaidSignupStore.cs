using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Stores
{
    public class RaidSignupStore
    {
        private static List<RaidSignup> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string RAID_SIGNUP_SELECT = @"
SELECT [RaidInstanceID], [Character], [Comment], [IsRostered], [IsCancelled], [RosteredSpecialization]
FROM [DRM].[dbo].[RaidSignup]
";

        public RaidSignupStore()
        {
            _loaded = false;
            _cache = new List<RaidSignup>();

            _lock = new ReaderWriterLock();
        }

        public RaidSignup ReadOneOrDefault(Func<RaidSignup, bool> func)
        {
            EnsureLoaded();

            foreach (var spec in _cache)
            {
                if (func(spec))
                    return spec;
            }

            return default(RaidSignup);
        }

        public List<RaidSignup> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<RaidSignup>();

            foreach (var entry in _cache)
            {
                newList.Add(entry);
            }

            return newList.Count > 0 ? newList : null;
        }

        public List<RaidSignup> ReadAll(Func<RaidSignup, bool> func)
        {
            EnsureLoaded();

            var newList = new List<RaidSignup>();

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

                    Connection.ExecuteSql(new Query(RAID_SIGNUP_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            var raidInstanceID = (int)reader[0];
                            var character = reader[1].ToString();

                            if (null != _cache.Find(s => ((raidInstanceID == s.RaidInstanceID) && (character == s.Character))))
                                return;

                            var newRaidSignup = new RaidSignup()
                            {
                                RaidInstanceID = raidInstanceID,
                                Character = character,
                                Comment = reader[2].ToString(),
                                IsRostered = (bool)reader[3],
                                IsCancelled = (bool)reader[4],
                                RosteredSpecialization = (int)reader[5]
                            };

                            _cache.Add(newRaidSignup);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
