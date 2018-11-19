using Common.Entity;
using Common.Util;
using Model.Operate_Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTouch.Model;

namespace WebTouch.Controllers
{
    public class MyMarkController : BaseController
    {
        // GET: MyMark
        public ActionResult MyMark()
        {
            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                CustomerMessage_Model model = new CustomerMessage_Model();

                model.UserID = cookieModel.UserID;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                //获取签到状态
                bool success = GetPostResponseNoRedirect("Mark", "GetMark", postJson, out data, true, false);
                if (success)
                {
                    string SignStatus = Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["SignStatus"].ToString();
                    if (SignStatus == "1")
                    {
                        cookieModel.IsSigned = false;
                    }
                    else if (SignStatus == "2")
                    {
                        cookieModel.IsSigned = true;
                    }

                    CookieUtil.SetCookie("WebTouch", JsonConvert.SerializeObject(cookieModel), 0, true);
                }
                
            }

            return View();
        }

        public ActionResult getMark()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":37,\"Level\":1,\"UserName\":\"\",\"CustomerCode\":\"C201811120000001\",\"MemberCode\":\"M201811130000001\",\"IsSigned\":false}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                CustomerMessage_Model model = new CustomerMessage_Model();

                model.UserID = cookieModel.UserID;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                //获取签到状态
                if(GetPostResponseNoRedirect("Mark", "GetMark", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 签到
        public ActionResult signIn() 
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":37,\"Level\":1,\"UserName\":\"\",\"CustomerCode\":\"C201811120000001\",\"MemberCode\":\"M201811130000001\",\"IsSigned\":false}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                CustomerMessage_Model model = new CustomerMessage_Model();

                model.UserID = cookieModel.UserID;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                //获取签到状态
                bool success = GetPostResponseNoRedirect("Mark", "SignIn", postJson, out data, true, false);
                if (success)
                {
                    string resCode = Newtonsoft.Json.Linq.JObject.Parse(data)["Code"].ToString();
                    // 签到完成后重新获取签到状态
                    if (resCode == "1") {
                        data = string.Empty;
                        GetPostResponseNoRedirect("Mark", "GetMark", postJson, out data, true, false);
                    }
                }
                return Content(data, "application/json; charset=utf-8");
            }
            else
            {
                return Json(res);
            }
        }
        // 会员信息
        public ActionResult getMember()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":37,\"Level\":1,\"UserName\":\"\",\"CustomerCode\":\"C201811120000001\",\"MemberCode\":\"M201811130000001\",\"IsSigned\":false}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                CustomerMessage_Model model = new CustomerMessage_Model();
                model.CustomerCode = cookieModel.CustomerCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Member", "GetMember", postJson, out data, true, false))
                {
                    cookieModel.Level = int.Parse(Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["LevelID"].ToString());

                    CookieUtil.SetCookie("WebTouch", JsonConvert.SerializeObject(cookieModel), 0, true);
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 会员等级
        public ActionResult getMemberLevel(int LevelID)
        {
            MemberLevel_Model model = new MemberLevel_Model();
            model.LevelID = LevelID;
            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;
            GetPostResponseNoRedirect("Member", "GetMemberLevel", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
        // 优惠券取得
        public ActionResult getCoupon()
        {
            string postJson = string.Empty;
            string data = string.Empty;
            GetPostResponseNoRedirect("Coupon", "GetCoupon", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }

        // 兑换信息取得
        public ActionResult getPoint()
        {
            string postJson = string.Empty;
            string data = string.Empty;
            GetPostResponseNoRedirect("Mark", "GetPoint", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
    }
}