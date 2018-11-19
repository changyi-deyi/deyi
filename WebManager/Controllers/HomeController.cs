using Common.Entity;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common.Util;
using WebManager.Model;
using Newtonsoft.Json;

namespace WebManager.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


    }
}