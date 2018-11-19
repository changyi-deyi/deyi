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
    public class FeedbackController : BaseController
    {
        [HttpPost]
        [ActionName("SubmitFeedback")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage SubmitFeedback(JObject obj)
        {

            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "意见反馈提交失败";
            res.Data = false;

            if (obj == null)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            if (string.IsNullOrEmpty(strSafeJson))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            Feedback_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Feedback_Model>(strSafeJson);

            if (model.UserID == 0 || string.IsNullOrEmpty(model.Content))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = InfFeedback_BLL.Instance.SubmitFeedback(model);
            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "意见反馈提交成功";
            }

            return toJson(res);
        }
    }
}
