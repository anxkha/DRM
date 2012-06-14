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

        private static string RAID_SIGNUP_ADD = @"
INSERT INTO [DRM].[dbo].[RaidSignup]
    ([RaidInstanceID], [Character], [Comment], [IsRostered], [IsCancelled], [RosteredSpecialization])
VALUES
    (@RaidInstanceID, @Character, @Comment, 0, 0, @RosteredSpecialization)
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

        public bool TryCreate(RaidSignup signup, out string errorMsg)
        {
            EnsureLoaded();

            if (200 < signup.Comment.Length)
            {
                errorMsg = "A comment longer than 200 was provided. This should not happen.";
                return false;
            }

            if ((1 > signup.RosteredSpecialization) || (2 < signup.RosteredSpecialization))
            {
                errorMsg = "An invalid specialization choice was provided. This should not happen.";
                return false;
            }

            using (new ReaderLock(_lock))
            {
                if (null != ReadOneOrDefault(rs => (rs.RaidInstanceID == signup.RaidInstanceID) && (rs.Character == signup.Character)))
                {
                    errorMsg = "A signup for this character for this raid already exists.";
                    return false;
                }

                if (null == Character.Store.ReadOneOrDefault(c => c.Name == signup.Character))
                {
                    errorMsg = "A signup cannot be created for a non-existant character.";
                    return false;
                }

                using (new WriterLock(_lock))
                {
                    if (null != ReadOneOrDefault(rs => (rs.RaidInstanceID == signup.RaidInstanceID) && (rs.Character == signup.Character)))
                    {
                        errorMsg = "A signup for this character for this raid already exists.";
                        return false;
                    }

                    if (null == Character.Store.ReadOneOrDefault(c => c.Name == signup.Character))
                    {
                        errorMsg = "A signup cannot be created for a non-existant character.";
                        return false;
                    }

                    var success = false;

                    Connection.ExecuteSql(new Query(RAID_SIGNUP_ADD)
                                            .AddParam("RaidInstanceID", signup.RaidInstanceID)
                                            .AddParam("Character", signup.Character)
                                            .AddParam("Comment", signup.Comment)
                                            .AddParam("RosteredSpecialization", signup.RosteredSpecialization), delegate(SqlDataReader reader)
                    {
                        if (0 == reader.RecordsAffected)
                            return;

                        success = true;
                    });

                    errorMsg = "";

                    if (success)
                        _cache.Add(signup);
                    else
                        errorMsg = "Datastore failure when creating the signup. Please contact the administrator.";

                    return success;
                }
            }
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
