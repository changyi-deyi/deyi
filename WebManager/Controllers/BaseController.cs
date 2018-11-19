using Common.Net;
using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebManager.Model;

namespace WebManager.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.Response != null)
            {
                string srtCookie = CookieUtil.GetCookieValue("WebManage", true);
                if (string.IsNullOrEmpty(srtCookie))
                {
                    requestContext.HttpContext.Response.Redirect("/Login/Login");
                }
                else
                {
                    Cookie_Model cookieModel = new Cookie_Model();
                    cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);
                    UserID = cookieModel.UserID;
                    UserCode = cookieModel.UserCode;
                    UserName = cookieModel.UserName;
                    Role = cookieModel.Role;
                    Type = cookieModel.Type;
                }
            }

            ViewBag.Role = Role;

        }

        public int UserID { get; set; }
        public int Type { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int Role { get; set; }



        public RedirectResult RedirectUrl()
        {
            return Redirect("/Login/Login");
        }
    }
}