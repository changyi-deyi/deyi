using Common.Safe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Util
{

    public class CookieUtil
    {
        public const string cookieKey = "S#FQ2st&";
        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName)
        {
            return GetCookieValue(cookieName, false);
        }


        /// <summary>
        /// 获取Cookie值
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName, bool isDes)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                if (isDes)
                {
                    return CryptDES.DESDecrypt(cookie.Value, cookieKey);
                }
                else
                {
                    return cookie.Value;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary> 
        /// 设置Cookie 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        /// <param name="CookieValue">Cookie值</param> 
        /// <param name="CookieTime">Cookie过期时间(分),0为关闭页面失效</param> 
        /// <param name="isDes">Des加密</param> 
        public static void SetCookie(string cookieDomain, string CookieName, string CookieValue, double CookieTime, bool isDes)
        {
            if (isDes)
            {
                CookieValue = CryptDES.DESEncrypt(CookieValue, cookieKey);
            }

            HttpCookie myCookie = new HttpCookie(CookieName);
            DateTime now = DateTime.Now;
            myCookie.Value = CookieValue;
            if (!string.IsNullOrEmpty(cookieDomain))
            {
                myCookie.Domain = cookieDomain;
            }

            if (CookieTime != 0)
            {
                myCookie.Expires = now.AddMinutes(CookieTime);
            }

            // Set 方法首先查看 Cookie 是否已存在于集合中，如果是，则更新它。 
            //Set 方法不允许 Cookie 集合中存在重复的 Cookie。 要在 Cookie 集合中添加重复的 Cookie，请使用 Add 方法。 
            HttpContext.Current.Response.Cookies.Set(myCookie);
        }


        /// <summary> 
        /// 设置Cookie 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        /// <param name="CookieValue">Cookie值</param> 
        /// <param name="CookieTime">Cookie过期时间(分),0为关闭页面失效</param> 
        public static void SetCookie(string cookieDomain, string CookieName, string CookieValue, double CookieTime)
        {
            SetCookie(cookieDomain, CookieName, CookieValue, CookieTime, false);
        }

        /// <summary> 
        /// 设置Cookie 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        /// <param name="CookieValue">Cookie值</param> 
        /// <param name="CookieTime">Cookie过期时间(分),0为关闭页面失效</param> 
        public static void SetCookie(string CookieName, string CookieValue, double CookieTime)
        {
            SetCookie(CookieName, CookieValue, CookieTime, false);
        }

        /// <summary> 
        /// 设置Cookie 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        /// <param name="CookieValue">Cookie值</param> 
        /// <param name="CookieTime">Cookie过期时间(分),0为关闭页面失效</param> 
        /// <param name="isDes">Des加密</param> 
        public static void SetCookie(string CookieName, string CookieValue, double CookieTime, bool isDes)
        {
            SetCookie("", CookieName, CookieValue, CookieTime, isDes);
        }

        /// <summary> 
        /// 清除CookieValue 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        public static void ClearCookie(string CookieName)
        {
            ClearCookie(CookieName, "");
        }

        /// <summary> 
        /// 清除CookieValue 
        /// </summary> 
        /// <param name="CookieName">Cookie名称</param> 
        public static void ClearCookie(string CookieName, string cookieDomain)
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            if (myCookie != null)
            {
                if (cookieDomain != "")
                {
                    myCookie.Domain = cookieDomain;
                }
                DateTime now = DateTime.Now;
                myCookie.Expires = now.AddYears(-2);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public static void DeleteCookie(string CookieName, string cookieDomain = "")
        {
            HttpCookie myCookie = new HttpCookie(CookieName);
            if (myCookie != null)
            {
                if (cookieDomain != "")
                {
                    myCookie.Domain = cookieDomain;
                }
                HttpContext.Current.Response.Cookies.Remove(CookieName);
            }
        }

    }
}
