using System;

namespace BlitzFramework.Lock.Abstract
{
    public interface ILockService
    {
        IDisposable CreateLock(string key);
    }
}