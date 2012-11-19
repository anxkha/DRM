using System;
using System.Threading;

namespace DOTP.RaidManager.Threading
{
    public class WriterLock : IDisposable
    {
        private ReaderWriterLock _lock;
        private LockCookie _cookie;
        private bool _wasUpgraded;

        public WriterLock(ReaderWriterLock rwLock)
        {
            if (null == rwLock) throw new Exception("Don't pass a null ReaderWriterLock object!");

            _lock = rwLock;

            if (_lock.IsReaderLockHeld)
                UpgradeLock();
            else
                CreateNewLock();
        }

        public void Dispose()
        {
            if (_wasUpgraded)
                _lock.DowngradeFromWriterLock(ref _cookie);
            else
                _lock.ReleaseWriterLock();
        }

        private void UpgradeLock()
        {
            _wasUpgraded = true;
            _cookie = _lock.UpgradeToWriterLock(Timeout.Infinite);
        }

        private void CreateNewLock()
        {
            _wasUpgraded = false;
            _lock.AcquireWriterLock(Timeout.Infinite);
        }
    }
}
