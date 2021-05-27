using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Base
{
    public class Common
    {
        public static void Set<T>(ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static TransactionOptions GetTransactionOptions(Enums.IsolationLevel isolationLevel)
        {
            return new TransactionOptions()
            {
                IsolationLevel = (IsolationLevel)isolationLevel,
                Timeout = TransactionManager.MaximumTimeout
            };
        }

        public static void SetCache(IMemoryCache iMemoryCache, string key, object value, Enums.CacheStatus status, int minutes = 0, int days = 0)
        {
            object cacheValue = null;
            if (!iMemoryCache.TryGetValue(key, out cacheValue))
            {
                var cacheEntryOptions = status == (int)Enums.CacheStatus.Sliding
                    ? new MemoryCacheEntryOptions().SetSlidingExpiration(days != 0
                        ? TimeSpan.FromDays(days)
                        : TimeSpan.FromMinutes(minutes))
                    : new MemoryCacheEntryOptions().SetAbsoluteExpiration(days != 0
                        ? TimeSpan.FromDays(days)
                        : TimeSpan.FromMinutes(minutes));

                iMemoryCache.Set(key, value, cacheEntryOptions);
            }
        }
    }
}
