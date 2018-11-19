using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Caching
{
    public class CDNCache
    {
        /// <summary>
        /// 设置Vary头为Accept-Encoding
        /// </summary>
        public static void SetVary()
        {
            HttpContext.Current.Response.Cache.SetOmitVaryStar(true);
        }

        /// <summary>
        /// 设置CDN缓存参数,默认为120秒
        /// </summary>
        /// <param name="Second"></param>
        public static void SetCDNCache()
        {
            SetCDNCache(120);
        }

        /// <summary>
        /// 设置CDN缓存参数
        /// </summary>
        /// <param name="Second">缓存秒数</param>
        public static void SetCDNCache(int Second)
        {
            HttpContext.Current.Response.Cache.SetOmitVaryStar(true);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddSeconds(Second));
            HttpContext.Current.Response.Cache.SetLastModified(DateTime.Now);
        }
    }
}
