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
    public class DoctorListController : BaseController
    {
        // GET: DoctorList
        public ActionResult DoctorList()
        {
            return View();
        }
        public ActionResult DoctorDetail()
        {
            return View();
        }
        //DoctorFaceService
        public ActionResult DoctorFaceService()
        {
            return View();
        }
        // 抽取科室信息
        public ActionResult getDepartment()
        {
            string postJson = string.Empty;
            string data = string.Empty;
            GetPostResponseNoRedirect("Doctor", "GetDepartment", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
        // 找医生
        public ActionResult getDoctorList(int DepartmentID, string ServerCode)
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

                Doctor_Model model = new Doctor_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                if (DepartmentID != 0) {
                    model.DepartmentID = DepartmentID;
                }
                if (!string.IsNullOrEmpty(ServerCode)) {
                    model.ServiceCode = ServerCode;
                    model.service = true;
                }
                else
                {
                    model.service = false;
                }
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Doctor", "GetDoctorList", postJson, out data, true, false))
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
        // 医生详情
        public ActionResult getDoctorDetail(string DoctorCode)
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

                DoctorDetailIn_Model model = new DoctorDetailIn_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.DoctorCode = DoctorCode;
                model.LevelID = cookieModel.Level;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Doctor", "GetDoctorDetail", postJson, out data, true, false))
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
        // 医生收藏
        public ActionResult followOrCancel(string DoctorCode)
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

                DoctorDetailIn_Model model = new DoctorDetailIn_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.DoctorCode = DoctorCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Doctor", "FollowOrCancel", postJson, out data, true, false))
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
        // 是否会员
        public ActionResult getMember()
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
                res.Code = "1";
                res.Message = "操作成功!";
                if (cookieModel.Level == 0) {
                    res.Data = false;
                } 
                else
                {
                    res.Data = true;
                }
                return Json(res);
            }
            else
            {
                return Json(res);
            }
        }
        // 服务可用优惠券信息
        public ActionResult buyServiceInfo(string ServerCode)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                Doctor_Model model = new Doctor_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.ServiceCode = ServerCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Service", "BuyServiceInfo", postJson, out data, true, false))
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


        public ActionResult AddServiceOrder(AddServiceOrder_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;
            
            model.UserID = this.UserID;
            model.CustomerCode = this.CustomerCode;
            model.LevelID = this.Level;
            model.WechatOpenID = CookieUtil.GetCookieValue("TWXOD", true);

            if (model.AmountType == 1)
            {
                model.AmountType = 2;
            }
            else
            {
                model.AmountType = 1;
            }
            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;

            if(GetPostResponseNoRedirect("Order", "AddServiceOrder", postJson, out data, true, false))
            {
                return Content(data, "application/json; charset=utf-8");
            }
            else
            {
                return Json(res);
            }
        }


    }
}