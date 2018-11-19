using Common.Entity;
using Common.Util;
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
    public class OpenVIPController : BaseController
    {
        // GET: OpenVIP
        public ActionResult OpenVIP()
        {
            return View();
        }
        public ActionResult OpenVIPConfirm()
        {
            return View();
        }

        public ActionResult getLevel()
        {
            string postJson = string.Empty;
            string data = string.Empty;
            GetPostResponseNoRedirect("Level", "GetLevel", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }

        public ActionResult OpenMemberInfo(int LevelID)
        {
            OpenMember_Model model = new OpenMember_Model();
            model.CustomerCode = this.CustomerCode;
            model.LevelID = LevelID;
            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;
            GetPostResponseNoRedirect("Member", "OpenMemberInfo", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
        public ActionResult GetLevelDetail()
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
                if (cookieModel.Level < 1) {
                    return Json(res);
                }

                UtilityOperate_Model model = new UtilityOperate_Model();
                
                model.ID = cookieModel.Level;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;

                if(GetPostResponseNoRedirect("Level", "GetLevelDetail", postJson, out data, true, false))
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


        public ActionResult addMemberOrder(AddMemberOrder_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            model.UserID = this.UserID;
            model.CustomerCode = this.CustomerCode;
            model.WechatOpenID = CookieUtil.GetCookieValue("TWXOD", true);

            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;

            GetPostResponseNoRedirect("Member", "addMemberOrder", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }

        public ActionResult CancelPayMember(OpeMemberOrder_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            model.Updater = this.UserID;

            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;

            GetPostResponseNoRedirect("Member", "CancelPayMember", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }


    }
}