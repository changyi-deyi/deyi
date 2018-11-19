using Common.Net;
using Common.Safe;
using Common.Util;
using Common.WeChat;
using Model.Operate_Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTouch.Model;

namespace WebTouch.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.Response != null)
            {

                string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
                if (!string.IsNullOrEmpty(srtCookie))
                {
                    Cookie_Model cookieModel = new Cookie_Model();
                    cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                    UserID = cookieModel.UserID;
                    Time = DateTime.Now.ToLocalTime();
                    UserName = cookieModel.UserName;
                    IsSigned = cookieModel.IsSigned;
                    Level = cookieModel.Level;
                    CustomerCode = cookieModel.CustomerCode;
                    
                }
                else
                {
                    WeChatOpenID = CookieUtil.GetCookieValue("TWXOD", true);
                    if (string.IsNullOrEmpty(WeChatOpenID))
                    {
                        string WeChatCode = QueryString.SafeQ("code");
                        if (!string.IsNullOrWhiteSpace(WeChatCode))
                        {
                            WeChat we = new WeChat();
                            string strWeChatOpenIDJson = we.CodeToToken(WeChatCode);
                            if (!string.IsNullOrWhiteSpace(strWeChatOpenIDJson))
                            {
                                var obj = Newtonsoft.Json.Linq.JObject.Parse(strWeChatOpenIDJson);
                                if (obj["openid"] != null || !string.IsNullOrWhiteSpace(StringUtils.GetDbString(obj["openid"])))
                                {
                                    string weChatOpenID = StringUtils.GetDbString(obj["openid"]);
                                    string weChatUnionID = StringUtils.GetDbString(obj["unionid"]);
                                    //string weChatOpenID = "oEG8xuKG9o6sGlL90uZWG_J6aagE";
                                    //string weChatUnionID = "oBpNrwVOOup6498J9r2h7m1P2JKY"; 
                                    CookieUtil.SetCookie("TWXOD", weChatOpenID, 0, true);
                                    CookieUtil.SetCookie("TWXUD", weChatUnionID, 0, true);
                                }
                            }
                        }
                        WeChatOpenID = CookieUtil.GetCookieValue("TWXOD", true);
                    }
                    Register_Model register = new Register_Model();
                    register.OpenID = WeChatOpenID;
                    string postJson = JsonConvert.SerializeObject(register);
                    string data = string.Empty;

                    bool success = GetPostResponseNoRedirect("Login", "LoginRegisterWithOpenID", postJson, out data, true, false);

                    if (success)
                    {
                        if (Newtonsoft.Json.Linq.JObject.Parse(data)["Code"].ToString() != "0")
                        {
                            Cookie_Model cookie = new Cookie_Model();
                            cookie.UserID = int.Parse(Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["UserID"].ToString());
                            cookie.UserName = Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["Name"].ToString();
                            cookie.CustomerCode = Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["CustomerCode"].ToString();
                            if (!string.IsNullOrEmpty(Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["MemberCode"].ToString()))
                            {
                                cookie.MemberCode = Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["MemberCode"].ToString();
                                cookie.Level = int.Parse(Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["LevelID"].ToString());
                            }
                            string SignStatus = Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["SignStatus"].ToString();
                            if (SignStatus == "1")
                            {
                                cookie.IsSigned = false;
                            }
                            else
                            {
                                cookie.IsSigned = true;
                            }
                            CookieUtil.SetCookie("WebTouch", JsonConvert.SerializeObject(cookie), 0, true);

                            UserID = cookie.UserID;
                            Time = DateTime.Now.ToLocalTime();
                            UserName = cookie.UserName;
                            IsSigned = cookie.IsSigned;
                            Level = cookie.Level;
                            CustomerCode = cookie.CustomerCode;

                            requestContext.HttpContext.Response.Redirect("/Home/Home");
                            requestContext.HttpContext.Response.End();
                        }
                        else
                        {
                            if (requestContext.HttpContext.Request.CurrentExecutionFilePath != "/Login/Login") {

                                requestContext.HttpContext.Response.Redirect("/Login/Login");
                                requestContext.HttpContext.Response.End();
                            }
                        }
                    }


                }
                
                ViewBag.QiNiuPath = ConfigurationManager.AppSettings["QiNiuPath"];
            }
        }

        public BaseController()
        {
        }

        public int UserID { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }
        public string OpenID { get; set; }
        public bool IsSigned { get; set; }
        public int Level { get; set; }

        public string CustomerCode { get; set; }
        public string WeChatOpenID { get; set; }


        public RedirectResult RedirectUrl(string code, string url, bool isAjax = true)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                switch (code)
                {
                    case "10013":
                        url = "/Login/Login";
                        break;
                    case "401":
                        url = "/Login/Login";
                        break;
                    case "500":
                        url = "/Login/Login";
                        break;
                    case "404":
                        url = "/Login/Login";
                        break;
                    case "10020":
                        if (this.Request != null)
                        {
                            url = "/Login/Login?fu=" + this.Request.Url.PathAndQuery.Replace("&", "%26");
                        }
                        else
                        {
                            url = "/Login/Login";
                        }
                        break;
                    default:
                        url = "/Login/Login";
                        break;
                }
            }


            return Redirect(url);
        }

        /// 
        /// </summary>
        /// <param name="strController"></param>
        /// <param name="strAction"></param>
        /// <param name="strParam"></param>
        /// <param name="Data"></param>
        /// <param name="isAjax"></param>
        /// <returns>Data=10013 RedirectUrl("/") Data=401</returns>
        public bool GetPostResponseNoRedirect(string strController, string strAction, string strParam, out string Data, bool isAjax = true, bool isNeedLogin = true)
        {
            string urlPath = "";
            if (this.Request != null)
            {
                urlPath = this.Request.Url.PathAndQuery;
            }

            string PostUrl = StringUtils.GetDbString(ConfigurationManager.AppSettings["PostUrl"]) + strController + "/" + strAction;
            if (string.IsNullOrEmpty(strController))
            {
                Data = "10013";
                return false;
            }

            if (string.IsNullOrEmpty(strAction))
            {
                Data = "10013";
                return false;
            }

            Cookie_Model cookieModel = new Cookie_Model();
            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            if (isNeedLogin)
            {
                if (string.IsNullOrEmpty(srtCookie))
                {
                    Data = "10020";
                    return false;
                }

                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                if (cookieModel == null)
                {
                    Data = "10020";
                    return false;
                }
            }

            NameValueCollection headers = null;
            string key = StringUtils.GetDbString(ConfigurationManager.AppSettings["EncryptKey"]);
            key = string.IsNullOrEmpty(key) ? "DYYICANG" : key;
            string finalKey = (strAction + strParam + key).ToUpper();
            string token = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(finalKey, "MD5");
            headers = new NameValueCollection();
            headers.Add("Accept-Languag", "zh-cn");
            headers.Add("Accept-Charset", "UTF-8");
            headers.Add("US", cookieModel.UserID.ToString());//4675           
            headers.Add("ME", strAction.ToUpper());
            headers.Add("TI", DateTime.Now.ToLocalTime().ToString());
            headers.Add("Authorization", token);

            bool isSuccess = false;
            HttpStatusCode resCode = NetUtil.GetResponse(PostUrl, strParam, out Data, headers);

            if (Request != null)
            {
                isAjax = Request.IsAjaxRequest();
            }
            else
            {
                isAjax = false;
            }

            if (isAjax)
            {
                switch (resCode)
                {
                    case HttpStatusCode.OK:
                        //200
                        isSuccess = true;
                        break;
                    case HttpStatusCode.Unauthorized:
                        //401错误 验证不通过 清除Cookie 返回登陆页面
                        isSuccess = false;
                        Data = "401";
                        HttpCookie myCookie = new HttpCookie("WebTouch");
                        if (myCookie != null)
                        {
                            myCookie.Domain = "";
                            DateTime now = DateTime.Now;
                            myCookie.Expires = now.AddYears(-2);
                            Response.Clear();
                            this.Response.BufferOutput = true;
                            Response.Cookies.Add(myCookie);
                        }
                        break;
                    case HttpStatusCode.InternalServerError:
                        //500错误 
                        Data = "500";
                        isSuccess = false;
                        break;
                    case HttpStatusCode.NotFound:
                        Data = "404";
                        isSuccess = false;
                        break;
                    default:
                        isSuccess = false;
                        break;
                }
            }
            else
            {
                switch (resCode)
                {
                    case HttpStatusCode.OK:
                        //200
                        isSuccess = true;
                        break;
                    case HttpStatusCode.Unauthorized:
                        //401错误 验证不通过 清除Cookie 返回登陆页面
                        CookieUtil.ClearCookie("WebTouch");
                        break;
                    case HttpStatusCode.InternalServerError:
                        //500错误 
                        Data = "500";
                        isSuccess = false;
                        break;
                    case HttpStatusCode.NotFound:
                        Data = "404";
                        isSuccess = false;
                        break;
                    default:
                        Data = "404";
                        break;
                }
            }

            return isSuccess;
        }
    }
}