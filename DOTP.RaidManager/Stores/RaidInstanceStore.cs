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

        private static string RAID_INSTANCE_INSERT = @"
INSERT INTO [DRM].[dbo].[RaidInstance]
    ([Raid], [Name], [Description], [InviteTime], [StartTime], [IsArchived])
VALUES
    (@Raid, @Name, @Description, @InviteTime, @StartTime, 0);
SELECT @@IDENTITY AS 'Identity'
";

        public RaidInstanceStore()
        {
            _loaded = false;
            _cache = new List<RaidInstance>();

            _lock = new ReaderWriterLock();
        }

        public bool TryCreate(RaidInstance instance, out string errorMsg)
        {
            using (new ReaderLock(_lock))
            {
                if (DateTime.Compare(instance.InviteTime, DateTime.Now) < 0)
                {
                    errorMsg = "You cannot schedule a raid in the past.";
                    return false;
                }

                if (DateTime.Compare(instance.StartTime, instance.InviteTime) < 0)
                {
                    errorMsg = "The start time cannot be before the invite time.";
                    return false;
                }

                using (new WriterLock(_lock))
                {
                    var success = false;

                    object id = Connection.ExecuteSqlScalar(new Query(RAID_INSTANCE_INSERT)
                                                                .AddParam("Raid", instance.Raid)
                                                                .AddParam("Name", instance.Name)
                                                                .AddParam("Description", instance.Description)
                                                                .AddParam("InviteTime", instance.InviteTime)
                                                                .AddParam("StartTime", instance.StartTime));

                    if (null != id)
                    {
                        instance.ID = (int)((decimal)id);
                        success = true;
                    }

                    errorMsg = "";

                    if (success)
                        _cache.Add(instance);
                    else
                        errorMsg = "Datastore failure when adding the character. Please contact the administrator.";

                    return success;
                }
            }
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

                    Connection.ExecuteSql(new Query(RAID_INSTANCE_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
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
