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
    public class MarkController : BaseController
    {

        [HttpPost]
        [ActionName("GetMark")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetMark(JObject obj)
        {
            ObjectResult<InfCustomer_Model> res = new ObjectResult<InfCustomer_Model>();
            res.Code = "0";
            res.Message = "发送失败";
            res.Data = null;

            if (obj == null)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            CustomerMessage_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerMessage_Model>(strSafeJson);

            if (model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            InfCustomer_Model result = InfCustomer_BLL.Instance.GetMark(model);

            if (result != null && result.SignStatus > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "签到状态获取成功";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("SignIn")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage SignIn(JObject obj)
        {
            ObjectResult<string> res = new ObjectResult<string>();
            res.Code = "0";
            res.Message = "发送失败";
            res.Data = null;

            if (obj == null)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            CustomerMessage_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerMessage_Model>(strSafeJson);

            if (model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            //获取客户信息
            InfCustomer_Model customer = InfCustomer_BLL.Instance.GetMark(model);

            if (customer == null)
            {
                res.Message = "签到状态获取失败";
                return toJson(res);
            }
            //签到
            int result = InfCustomer_BLL.Instance.SignIn(customer);
            if (result == 0)
            {
                res.Message = "签到失败";
                return toJson(res);
            }
            else if (result == 2)
            {
                res.Code = "2";
                res.Message = "已签到";
                return toJson(res);
            }
            else if (result == 1)
            {
                res.Code = "1";
                res.Message = "签到成功";
                return toJson(res);
            }
            return toJson(res);
        }

        [HttpPost]
        [ActionName("GetPoint")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetPoint(JObject obj)
        {
            ObjectResult<SetGetpoint_Model> res = new ObjectResult<SetGetpoint_Model>();
            res.Code = "0";
            res.Message = "发送失败";
            res.Data = null;

            SetGetpoint_Model result = SettingM_BLL.Instance.getSetPoint();


            if (result != null)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "签到状态获取成功";
            }

            return toJson(res);
        }
    }
}
