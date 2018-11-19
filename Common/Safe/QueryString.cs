using Common.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Safe
{

    public class QueryString
    {
        /// <summary>
        /// 接收字符串并转为Int型
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static int IntSafeQ(string key)
        {
            return IntSafeQ(key, 0);
        }
        /// <summary>
        /// 接收字符串并转为Int型
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static int IntSafeQ(string key, int defaultvale)
        {
            return StringUtils.GetDbInt((HttpContext.Current.Request.QueryString[key]), defaultvale);
        }

        public static long LongSafeQ(string key, long defaultvale = 0)
        {
            return StringUtils.GetDbLong(HttpContext.Current.Request.QueryString[key], defaultvale);
        }

        /// <summary>
        /// 接收字符串转为String
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public static string SafeQ(string key)
        {
            return SafeQ(key, 0, 50);
        }

        /// <summary>
        /// 接收字符串转为String
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string SafeQ(string key, int len)
        {
            return SafeQ(key, 0, len);
        }

        /// <summary>
        /// 接收字符串转为String
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="type">是否转为小写 1转为小写</param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string SafeQ(string key, int type, int len)
        {
            string obj = StringUtils.GetDbString(HttpContext.Current.Request.QueryString[key]).Trim();
            if (obj.Length > len)
            {
                return "";
            }
            if (type == 1)
            {
                obj = obj.ToString().ToLower();
            }
            obj = StringUtils.SafeCode(obj);
            return obj;
        }

        /// <summary>
        /// 获取网站默认编码参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string defaultRequest(string key)
        {
            return EncodeRequest(key, Encoding.Default);
        }
        /// <summary>
        /// 获取UTF-8编码参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string utf8Request(string key)
        {
            return EncodeRequest(key, Encoding.GetEncoding("UTF-8"));
        }
        /// <summary>
        /// 获取GB2312编码参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string gb2312Request(string key)
        {
            return EncodeRequest(key, Encoding.GetEncoding("GB2312"));
        }

        /// <summary>
        /// 获取编码参数(并过滤危险字符)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeRequest(string key, Encoding encoding)
        {
            string q = HttpContext.Current.Request.Url.Query;
            NameValueCollection nv = HttpUtility.ParseQueryString(q, encoding);
            //return nv[key];
            return StringUtils.SafeCode(nv[key]);
        }
    }
}
