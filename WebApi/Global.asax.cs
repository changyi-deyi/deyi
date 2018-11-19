using Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApi
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LogUtil.StartLog4Net();
            GlobalConfiguration.Configure(WebApiConfig.Register);
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