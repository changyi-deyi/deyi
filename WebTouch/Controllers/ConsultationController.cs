using Common.Entity;
using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Operate_Model;
using WebTouch.Model;

namespace WebTouch.Controllers
{
    public class ConsultationController : BaseController
    {
        // GET: Consultation
        public ActionResult Consultation()
        {
            return View();
        }
        //获取已上传图片
        public ActionResult getConsultationIma(int GroupID)
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

                GetAdvisoryDetail_Model model = new GetAdvisoryDetail_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.GroupID = GroupID;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Advisory", "GetAdvisoryIma", postJson, out data, true, false))
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


        //提交咨询
        public ActionResult ConsultationCommit(ConsultationCommit_Model input)
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

                SubmitAdvisory_Model model = new SubmitAdvisory_Model();
                model.AdvisoryIma = new List<ImageURL_Model>();

                model.CustomerCode = cookieModel.CustomerCode;
                if (input.AdvisoryIma != null)
                {
                    foreach (string ima in input.AdvisoryIma)
                    {
                        ImageURL_Model imaModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageURL_Model>(ima);
                        model.AdvisoryIma.Add(imaModel);
                    }
                }
                model.UserID = cookieModel.UserID;
                model.GroupID = input.GroupID;
                model.Content = input.Text;
                if (input.GroupID == 0)
                {
                    model.ComtinueFlg = 2;
                }
                else
                {
                    model.ComtinueFlg = 1;
                }
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Advisory", "SubmitAdvisory", postJson, out data, true, false))
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
        
        public class ConsultationCommit_Model
        {
            public int GroupID { get; set; }
            public string Text { get; set; }
            public string[] AdvisoryIma { get; set; }

        }
    }
}