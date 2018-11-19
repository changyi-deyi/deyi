using Common.Safe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Util
{

    public class Misc
    {
        /// <summary>
        /// 获取IP地址 做CDN静态加速不能用这个属性
        /// </summary>
        public static string IPAddress
        {
            get
            {
                try
                {
                    string userHostAddress;
                    if (HttpContext.Current == null)
                    {
                        return string.Empty;
                    }

                    HttpRequest request = HttpContext.Current.Request;
                    //HTTP_TRUE_CLIENT_IP蓝讯动态加速定义IP
                    if (request.ServerVariables["HTTP_TRUE_CLIENT_IP"] != null)
                    {
                        userHostAddress = request.ServerVariables["HTTP_TRUE_CLIENT_IP"];
                        return userHostAddress;
                    }


                    if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                    {
                        userHostAddress = request.ServerVariables["REMOTE_ADDR"];
                    }
                    else
                    {
                        userHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
                    }

                    if (string.IsNullOrEmpty(userHostAddress))
                    {
                        userHostAddress = request.UserHostAddress;
                    }

                    return userHostAddress;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        public static string FullIPAddress
        {
            get
            {
                try
                {
                    string userHostAddress;
                    if (HttpContext.Current == null)
                    {
                        return string.Empty;
                    }


                    HttpRequest request = HttpContext.Current.Request;
                    if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                    {
                        userHostAddress = request.ServerVariables["REMOTE_ADDR"];
                    }
                    else
                    {
                        userHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    }

                    if (string.IsNullOrEmpty(userHostAddress))
                    {
                        userHostAddress = request.UserHostAddress;
                    }

                    return userHostAddress;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 获取IP地址 做CDN静态加速不能用这个属性
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddr()
        {
            return IPAddress;
        }
        /// <summary>
        /// 获取IP地址　
        /// </summary>
        /// <param name="i">静态CDN加速i=1,动态CDN加速或不做加速　i=0</param>
        /// <returns></returns>
        public static string GetIpAddr(int i)
        {
            string userHostAddress;
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }
            HttpRequest request = HttpContext.Current.Request;
            //HTTP_TRUE_CLIENT_IP蓝讯动态加速定义IP
            if (request.ServerVariables["HTTP_TRUE_CLIENT_IP"] != null)
            {
                userHostAddress = request.ServerVariables["HTTP_TRUE_CLIENT_IP"];
                return userHostAddress;
            }

            if (request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                userHostAddress = request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                string[] arruserHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' });
                if (arruserHostAddress.Length > i)
                {
                    userHostAddress = arruserHostAddress[i];
                }
                else
                {
                    userHostAddress = arruserHostAddress[arruserHostAddress.Length - 1];
                }
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = request.UserHostAddress;
            }
            return userHostAddress;
        }


        /// <summary>
        /// 站点URL
        /// </summary>
        public static string WebUrl
        {
            get
            {
                int port = HttpContext.Current.Request.Url.Port;
                if (port == 443)
                {
                    port = 80;
                }
                return "http://" + HttpContext.Current.Request.Url.Host + (port == 80 ? "" : ":" + HttpContext.Current.Request.Url.Port.ToString()) + ApplicationPath + "/";
            }
        }
        /// <summary>
        /// 获取虚拟目录
        /// </summary>
        public static string ApplicationPath
        {
            get
            {
                string applicationPath = "/";
                if (HttpContext.Current != null)
                {
                    applicationPath = HttpContext.Current.Request.ApplicationPath;
                }
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                return applicationPath.ToLower(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// 获取手机站地址
        /// </summary>
        /// <param name="brandID"></param>
        /// <returns></returns>
        public static string WapAddr(string webAddr)
        {
            string temp = webAddr;
            temp = temp.Remove(temp.LastIndexOf("."));
            temp = temp.Remove(temp.LastIndexOf("."));

            return "http://m" + webAddr.Remove(0, temp.Length);
        }

        /// <summary>
        /// wap站获取客户唯一编号，不计入Cookie
        /// </summary>
        /// <returns></returns>
        public static string GuId1
        {
            get
            {
                string guid = string.Empty;
                guid = QueryString.SafeQ("guid");
                if (string.IsNullOrEmpty(guid))
                {
                    guid = Guid.NewGuid().ToString().Replace("-", "");
                }

                return guid;
            }
        }

        public static string GuId
        {
            get
            {
                string guid = CookieUtil.GetCookieValue("Moonbasa_Guid");// HttpContext.Current.Request.Cookies["Moonbasa_Guid"];
                if (string.IsNullOrEmpty(guid))
                {
                    guid = QueryString.SafeQ("guid");
                    if (string.IsNullOrEmpty(guid))
                    {
                        guid = Guid.NewGuid().ToString().Replace("-", "");
                    }
                    if (guid.Length > 10)
                    {
                        CookieUtil.SetCookie("Moonbasa_Guid", guid, 60);
                    }
                }

                return guid;
            }
        }

    }
}
