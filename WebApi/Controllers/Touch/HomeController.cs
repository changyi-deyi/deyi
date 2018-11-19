using Common.Caching;
using Common.Entity;
using Common.Util;
using Model.Operate_Model;
using Newtonsoft.Json.Linq;
using System;
using BLL;
using Model.Table_Model;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Authorize;

namespace WebApi.Controllers.Touch
{
    public class HomeController : BaseController
    {
        [HttpPost]
        [ActionName("GetImage")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetImage(JObject obj)
        {
            ObjectResult<List<InfBanner_Model>> res = new ObjectResult<List<InfBanner_Model>>();
            res.Code ="0";
            res.Message = "获取图片失败";
            res.Data = null;

            List<InfBanner_Model> result = InfBanner_BLL.Instance.GetImage();
            
            if (result != null && result.Count > 0)
            {
                foreach(InfBanner_Model item in result)
                {
                    item.ImageURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.ImageURL;
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "获取图片成功";
            }

            return toJson(res);
        }

    }
}
