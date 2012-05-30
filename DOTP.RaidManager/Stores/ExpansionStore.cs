using DOTP.Database;
using DOTP.RaidManager.Threading;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace DOTP.RaidManager.Stores
{
    public class ExpansionStore
    {
        private static List<Expansion> _cache;
        private bool _loaded;
        private ReaderWriterLock _lock;

        private static string EXPANSION_SELECT = "SELECT * FROM [DRM].[dbo].[Expansion]";

        public ExpansionStore()
        {
            _loaded = false;
            _cache = null;

            _lock = new ReaderWriterLock();
        }

        public List<Expansion> ReadAll()
        {
            EnsureLoaded();

            var newList = new List<Expansion>();

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
                        _cache = new List<Expansion>();

                    Connection.ExecuteSql(new Query(EXPANSION_SELECT), delegate(SqlDataReader reader)
                    {
                        while (reader.Read())
                        {
                            if (null != _cache.Find(c => reader[0].ToString() == c.Name))
                                return;

                            var newExpansion = new Expansion()
                            {
                                Name = reader[0].ToString()
                            };

                            _cache.Add(newExpansion);
                        }
                    });

                    _loaded = true;
                }
            }
        }
    }
}
