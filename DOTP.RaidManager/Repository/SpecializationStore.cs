using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DOTP.RaidManager.Repository
{
    public class SpecializationStore
    {
        private static List<Specialization> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string SPECIALIZATION_SELECT = @"
SELECT s.[ID], stc.[Class], s.[Name], s.[Role]
FROM [Specialization] s
    INNER JOIN [SpecializationToClass] stc ON stc.[Specialization] = s.[ID]
";

        public SpecializationStore()
        {
            _loaded = false;
            _cache = new List<Specialization>();;

            _lock = new ReaderWriterLock();
        }

        public Specialization ReadOneOrDefault(Func<Specialization, bool> func)
        {
            EnsureLoaded();

            foreach (var spec in _cache)
            {
                if (func(spec))
                    return spec;
            }

            return default(Specialization);
        }

        public List<Specialization> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<Specialization>();

            foreach (var entry in _cache)
            {
                newList.Add(entry);
            }

            return newList.Count > 0 ? newList : null;
        }

        public List<Specialization> ReadAll(Func<Specialization, bool> func)
        {
            EnsureLoaded();

            var newList = new List<Specialization>();

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

                    Connection.ExecuteSql(new Query(SPECIALIZATION_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            var id = int.Parse(reader[0].ToString());

                            if ((null != _cache.Find(s => id == s.ID)) && (id != 35))
                                return;

                            _cache.Add(new Specialization((int)reader[0], reader[1].ToString(), reader[2].ToString(), reader[3].ToString()));
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
