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
    public class MyHeathRecordListController : BaseController
    {
        // GET: MyHeathRecordList
        public ActionResult MyHeathRecordList()
        {
            return View();
        }
        // 健康档案_提交
        public ActionResult CreatHealthRecord()
        {
            return View();
        }
        // 健康档案_详情
        public ActionResult MyHeathRecordDetail()
        {
            return View();
        }
        // 健康档案
        public ActionResult getProfile(int Type)
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

                ProfileMessage_Model model = new ProfileMessage_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.Type = Type;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Profile", "GetProfile", postJson, out data, true, false))
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
        // 家庭信息
        public ActionResult getMemberFamily()
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

                ProfileMessage_Model model = new ProfileMessage_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Member", "GetMemberFamily", postJson, out data, true, false))
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
        // 建立档案
        public ActionResult profileUpload(ProfileUpload_Model model, string[] ProfileImage, int ID)
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

                model.CustomerCode = cookieModel.CustomerCode;
                model.UserID = cookieModel.UserID;

                model.UploadImage = new List<ImageURL_Model>();
                foreach (string ima in ProfileImage)
                {
                    ImageURL_Model imaModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageURL_Model>(ima);
                    model.UploadImage.Add(imaModel);
                }

                if (model.UploadImage.Count == 0)
                {
                    res.Message = "至少上传一张图片!";
                    return Json(res);
                }

                if(ID == 0)
                {
                    model.RelatedCustomerCode = cookieModel.CustomerCode;
                }
                else
                {
                    model.RelatedCustomerCode = ID.ToString();
                }

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Profile", "ProfileUpload", postJson, out data, true, false))
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
        // 健康档案详情
        public ActionResult getProfileDetail(CustomerProfile_Model model)
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

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Profile", "GetProfileDetail", postJson, out data, true, false))
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
        //DeleteProfileIma
        public ActionResult deleteProfileIma(DeleteProfileIma_Model model)
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
                model.UserID = cookieModel.UserID;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Profile", "DeleteProfileIma", postJson, out data, true, false))
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