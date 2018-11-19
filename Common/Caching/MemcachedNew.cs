using System;
using System.Collections;

using Enyim.Caching;
using Enyim.Caching.Memcached;

using Common.Safe;
using Common.Util;
using Common.Log;

namespace Common.Caching
{

    public sealed class MemcachedNew
    {
        private static readonly MemcachedClient MemcachedClient = new MemcachedClient();

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="regionName">区域</param>
        /// <param name="key">键值</param> 
        /// <returns>键值对应的缓存内容</returns>
        public static object Get(string regionName, string key)
        {
            try
            {
                key = CryptMD5.Encrypt(key);

                return MemcachedClient.Get(GetKey(regionName, key));
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex);

                return null;
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="regionName">区域</param>
        /// <param name="key">键值</param>
        /// <returns>键值对应的缓存内容</returns>
        public static T Get<T>(string regionName, string key)
        {
            try
            {
                key = CryptMD5.Encrypt(key);

                return MemcachedClient.Get<T>(GetKey(regionName, key));
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex);

                return default(T);
            }
        }

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="regionName">区域</param>
        /// <param name="key">键值</param>
        /// <param name="data">键值对应的缓存内容</param>
        /// <param name="cacheTime">缓存时间 单位：秒</param>
        /// <returns></returns>
        public static bool Set(string regionName, string key, object data, int cacheTime)
        {
            try
            {
                if (data == null)
                {
                    return false;
                }

                var collection = data as ICollection;

                if (collection != null)
                {
                    if (collection.Count <= 0)
                    {
                        return false;
                    }
                }

                if (CreateRegion(regionName) > 0)
                {
                    if (cacheTime > TimeSpan.FromDays(7).TotalSeconds)
                    {
                        cacheTime = Convert.ToInt32(TimeSpan.FromDays(7).TotalSeconds);
                    }

                    key = CryptMD5.Encrypt(key);

                    key = GetKey(regionName, key);

                    MemcachedClient.Remove(key);

                    return MemcachedClient.Store(StoreMode.Set, key, data, new TimeSpan(0, 0, cacheTime));
                }

                return false;
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex);

                return false;
            }
        }

        public static bool SetList(string regionName, string key, object data, int cacheTime)
        {
            try
            {
                if (data == null)
                {
                    return false;
                }

                if (CreateRegion(regionName) > 0)
                {
                    if (cacheTime > TimeSpan.FromDays(7).TotalSeconds)
                    {
                        cacheTime = Convert.ToInt32(TimeSpan.FromDays(7).TotalSeconds);
                    }

                    key = CryptMD5.Encrypt(key);

                    key = GetKey(regionName, key);

                    MemcachedClient.Remove(key);

                    return MemcachedClient.Store(StoreMode.Set, key, data, new TimeSpan(0, 0, cacheTime));
                }

                return false;
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex);

                return false;
            }
        }

        /// <summary>
        /// 添加缓存：默认缓存3600秒
        /// </summary>
        /// <param name="regionName">区域</param>
        /// <param name="key">键值</param>
        /// <param name="data">键值对应的缓存内容</param>
        /// <returns></returns>
        public static bool Set(string regionName, string key, object data)
        {
            return Set(regionName, key, data, 3600);
        }

        public static bool SetList(string regionName, string key, object data)
        {
            return SetList(regionName, key, data, 3600);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="regionName"></param>
        /// <param name="key">/键值</param>
        public static bool Remove(string regionName, string key)
        {
            key = CryptMD5.Encrypt(key);

            return MemcachedClient.Remove(GetKey(regionName, key));
        }

        #region [ Region ]

        /// <summary>
        /// 创建区域
        /// </summary>
        /// <param name="name">区域名称</param>
        /// <returns>区域版本号</returns>
        public static int CreateRegion(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            string regionName = name;

            object obj = MemcachedClient.Get(regionName);

            int regionVersion;

            if (obj == null)
            {
                regionVersion = StringUtils.GetDbInt(DateTime.Now.ToString("hhmmssfff"));

                if (MemcachedClient.Store(StoreMode.Set, regionName, regionVersion, TimeSpan.FromDays(7)))
                {
                    return regionVersion;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                regionVersion = StringUtils.GetDbInt(obj);
            }

            return regionVersion;
        }

        /// <summary>
        /// 清空区域
        /// </summary>
        /// <param name="regionName">区域名称</param>
        /// <returns>是否清空成功</returns>
        public static bool ClearRegion(string regionName)
        {
            if (string.IsNullOrWhiteSpace(regionName))
            {
                throw new ArgumentNullException("regionName");
            }

            int regionVersion = StringUtils.GetDbInt(DateTime.Now.ToString("hhmmssfff")); ;

            return MemcachedClient.Store(StoreMode.Set, regionName, regionVersion, TimeSpan.FromDays(7));
        }

        #endregion

        #region [ Private ]

        private static string GetKey(string regionName, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }

            string val = string.Format("{0}_{1}_{2}", regionName, GetRegionVersion(regionName), key);

            if (val.Length > 250)
            {
                throw new ArgumentException("键值长度不能超过250位");
            }

            return val;
        }

        private static int GetRegionVersion(string regionName)
        {
            object o = MemcachedClient.Get(regionName);
            if (o == null)
            {
                return StringUtils.GetDbInt(DateTime.Now.ToString("hhmmssfff"));
            }

            return (int)o;
        }

        #endregion
    }
}
