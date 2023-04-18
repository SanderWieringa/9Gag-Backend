using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace PostService.Extensions
{
    public static class DistributedCacheExtensions
    {
        static int day = 86400;
        static int minute = 60;

        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
           string recordId,
           T data,
           TimeSpan? absoluteExpireTime = null,
           TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();

            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(minute);
            options.SlidingExpiration = unusedExpireTime;

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);

            if (jsonData is null)
            {
                return default(T);
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
