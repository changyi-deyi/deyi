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
    public class ServiceListController : BaseController
    {
        // GET: ServiceList
        public ActionResult ServiceList()
        {
            return View();
        }
        // GET: ServiceList
        public ActionResult ServiceDetail()
        {
            return View();
        }
        // 取得服务列表
        public ActionResult getService()
        {
            string postJson = string.Empty;
            string data = string.Empty;
            GetPostResponseNoRedirect("Service", "GetService", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
        //就医服务详情
        public ActionResult getServiceDetail(string ServiceCode)
        {
            ServiceDetail_Model model = new ServiceDetail_Model();

            model.ServiceCode = ServiceCode;
            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;
            GetPostResponseNoRedirect("Service", "GetServiceDetail", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
    }
}