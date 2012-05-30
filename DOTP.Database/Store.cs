using System;
using System.Collections.Generic;

namespace DOTP.Database
{
    public static class Store<TDataType>
    {
        //protected static List<TDataType> m_Cache = new List<TDataType>();
        //protected static bool m_bLoaded = false;

        //protected static abstract void Load()
        //{
        //}

        //public static TDataType ReadOneOrDefault(Func<TDataType, bool> func)
        //{
        //    if (!m_bLoaded) Load();

        //    foreach (var entry in m_Cache)
        //    {
        //        if (func(entry))
        //            return entry;
        //    }

        //    return default(TDataType);
        //}

        //public static List<TDataType> ReadAll(Func<TDataType, bool> func)
        //{
        //    if (!m_bLoaded) Load();

        //    var newList = new List<TDataType>();

        //    foreach (var entry in m_Cache)
        //    {
        //        if (func(entry))
        //            newList.Add(entry);
        //    }

        //    return newList.Count > 0 ? newList : null;
        //}
    }
}
