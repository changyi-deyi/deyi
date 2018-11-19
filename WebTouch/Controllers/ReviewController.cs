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
    public class ReviewController : BaseController
    {
        // GET: Review
        public ActionResult Review()
        {
            return View();
        }
        public ActionResult ApplyReview()
        {
            return View();
        }
        // 复查订单
        public ActionResult getServiceOrder(int Flag, int Tag)
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

                OrderSelect_Model model = new OrderSelect_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.flag = Flag;
                if (Tag != 0) {
                    model.tag = Tag;
                }
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Order", "GetServiceOrder", postJson, out data, true, false))
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