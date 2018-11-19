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
    public class HomeController :  BaseController
    {
        // GET: Home
        public ActionResult Home()
        {
            return View();
        }
        //首页图片轮播
        public ActionResult getImage()
        {
            string postJson = string.Empty;
            string data = string.Empty;

            bool success = GetPostResponseNoRedirect("Home", "GetImage", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
        // 签到状态取得
        public ActionResult getMark()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

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
            else {
                return Json(res);
            }
        }
        // 未读消息状态取得
        public ActionResult getReadFlag()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                CustomerMessage_Model model = new CustomerMessage_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                //获取签到状态
                if(GetPostResponseNoRedirect("Message", "GetReadFlag", postJson, out data, true, false))
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
    }
}