
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Text.RegularExpressions;

namespace Common.Caching
{

    public class MemoryCache
    {
        static ObjectCache Cache
        {
            get
            {
                return System.Runtime.Caching.MemoryCache.Default;
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键值</param>
        /// <returns>键值对应的缓存内容</returns>
        public static T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="data">键值对应的缓存内容</param>
        /// <param name="cacheTime">缓存时间</param>
        public static void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// 是否存在缓存内容
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>是否存在</returns>
        public static bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">/键值</param>
        public static void Remove(string key)
        {
            Cache.Remove(key);
        }

        /// <summary>
        /// 删除缓存根据正则
        /// </summary>
        /// <param name="pattern">pattern</param>
        public static void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = new List<String>();

            foreach (var item in Cache)
                if (regex.IsMatch(item.Key))
                    keysToRemove.Add(item.Key);

            foreach (string key in keysToRemove)
            {
                Remove(key);
            }
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public static void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }
    }
}
