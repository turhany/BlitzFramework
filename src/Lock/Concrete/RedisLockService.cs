using System;
using System.Collections.Generic;
using System.Net;
using BlitzFramework.Constants;
using BlitzFramework.Lock.Abstract;
using Microsoft.Extensions.Configuration;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ConvertToNullCoalescingCompoundAssignment

namespace BlitzFramework.Lock.Concrete
{
    public class RedisLockService : ILockService
    {
        static readonly Object RedLockFactory = new Object();
        protected static RedLockEndPoint PasswordedServer { get; private set; }
        readonly TimeSpan _expiry = TimeSpan.FromSeconds(15);
        private static RedLockFactory _redisLockFactory { get; set; }

        public RedisLockService(IConfiguration configuration)
        {
            PasswordedServer = new RedLockEndPoint
            {
                EndPoint = new DnsEndPoint(configuration[FrameworkConstants.RedLockHostAddress], int.Parse(configuration[FrameworkConstants.RedLockHostPort])),
                Password = configuration[FrameworkConstants.RedLockHostPassword],
                Ssl = true
            };
        }

        static RedLockFactory RedisLockFactory
        {
            get
            {
                lock (RedLockFactory)
                {
                    return _redisLockFactory ?? (_redisLockFactory = new RedLockFactory(new RedLockConfiguration(new List<RedLockEndPoint> { PasswordedServer })));
                }
            }
        }

        public IDisposable CreateLock(string key)
        {
            int i = 0;
            IRedLock locked = null;
            while (i < 10)
            {
                i++;
                var resource = key;

                locked = RedisLockFactory.CreateLock(resource, _expiry);

                if (locked.IsAcquired)
                {
                    return locked;
                }
            }
            return locked;
        }
    }
}