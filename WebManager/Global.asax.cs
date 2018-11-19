using Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebManager
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LogUtil.StartLog4Net();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            Response.Clear();
            Server.ClearError();
            if (ex != null)
            {
                LogUtil.Log(ex);
            }
        }
    }
}