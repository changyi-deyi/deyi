﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTouch.Controllers
{
    public class RedirectController : BaseController
    {
        // GET: Default
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult NoCode() {
            return View();
        }

    }
}