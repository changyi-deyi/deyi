using Common.Caching;
using Common.Entity;
using Common.Util;
using Common.WeChat;
using Model.Operate_Model;
using Model.Table_Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTouch.Model;

namespace WebTouch.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Login()
        {
            string openID = CookieUtil.GetCookieValue("TWXOD", true);
            if (!string.IsNullOrEmpty(openID))
            {
                Register_Model register = new Register_Model();
                register.OpenID = openID;
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
                        
                        Response.Redirect("/Home/Home");
                        Response.End();
                    }
                    else
                    {
                        // res.Message = Newtonsoft.Json.Linq.JObject.Parse(data)["Message"].ToString();
                    }
                }
            }
            return View();
        }

        // GET: Login
        public ActionResult LoginRegister(string userAgent, string mobile,int Auth)
        {
            ObjectResult<string> res = new ObjectResult<string>();
            res.Code = "0";
            res.Message = "登陆失败!";
            res.Data = "";
            Register_Model loginModel = new Register_Model();
            string openID = CookieUtil.GetCookieValue("TWXOD", true);

            loginModel.OpenID = openID;
            loginModel.BrowserInfo = userAgent;
            loginModel.Mobile = mobile;
            loginModel.Auth = Auth;

            WeChat we = new WeChat();
            string WeChatUserInfo = we.GetWeChatUserInfo(openID);

            if (!string.IsNullOrWhiteSpace(WeChatUserInfo))
            {
                WeChatInfo_Model WeChatInfo = JsonConvert.DeserializeObject<WeChatInfo_Model>(WeChatUserInfo);
                loginModel.Gender = WeChatInfo.sex;
                loginModel.Name = WeChatInfo.nickname;
            }

            string postJson = JsonConvert.SerializeObject(loginModel);
            string data = string.Empty;

            bool success = GetPostResponseNoRedirect("Login", "LoginRegister", postJson, out data, true, false);

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
                    res.Code = "1";
                    res.Message = "登陆成功!";
                    res.Data = Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["LoginStatue"].ToString();
                }
                else
                {
                    res.Message = Newtonsoft.Json.Linq.JObject.Parse(data)["Message"].ToString();
                }
            }

            return Json(res);
        }


        public ActionResult getAuthCode(string mobile) {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            UtilityOperate_Model model = new UtilityOperate_Model();

            model.mobile = mobile;
            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;

            bool success = GetPostResponseNoRedirect("Login", "GetAuthCode", postJson, out data, true, false);

            if (success)
            {
                return Content(data, "application/json; charset=utf-8");
            }
            else
            {
                return Json(res);
            }

        }

        public ActionResult DYAggrement()
        {
            return View();
        }
        public ActionResult LegalAgreement()
        {
            return View();
        }

    }
}