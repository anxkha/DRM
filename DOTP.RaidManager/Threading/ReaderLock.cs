using System;
using System.Threading;

namespace DOTP.RaidManager.Threading
{
    public class ReaderLock : IDisposable
    {
        private ReaderWriterLock _lock;

        public ReaderLock(ReaderWriterLock rwLock)
        {
            if (null == rwLock) throw new Exception("Don't pass a null ReaderWriterLock object!");

            _lock = rwLock;
            _lock.AcquireReaderLock(Timeout.Infinite);
        }

        public void Dispose()
        {
            _lock.ReleaseReaderLock();
        }
    }
}
