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
    public class MessageController : BaseController
    {
        [HttpPost]
        [ActionName("DeleteMessage")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage DeleteMessage(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "消息删除失败";
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

            MessageDelete_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageDelete_Model>(strSafeJson);

            if (model.MessageID==0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = OpeCustomerMessage_BLL.Instance.DeleteMessage(model);


            if (result != 0)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "消息删除成功";
            }

            return toJson(res);
        }
        

        [HttpPost]
        [ActionName("GetCustomerMessage")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetCustomerMessage(JObject obj)
        {
            ObjectResult<List<OpeCustomerMessage_Model>> res = new ObjectResult<List<OpeCustomerMessage_Model>>();
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

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            List<OpeCustomerMessage_Model> result = OpeCustomerMessage_BLL.Instance.GetMessage(model);

            if (result != null && result.Count > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "发送成功";
            }
            else if (result.Count == 0)
            {
                res.Code = "2";
                res.Message = "暂无消息";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("GetReadFlag")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetReadFlag(JObject obj)
        {
            ObjectResult<int> res = new ObjectResult<int>();
            res.Code = "0";
            res.Message = "发送失败";
            res.Data = 0;

            if (obj == null)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            CustomerMessage_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerMessage_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = OpeCustomerMessage_BLL.Instance.GetReadMessage(model);

            if (result > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "有未读消息";
            }
            else
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "无未读消息";
            }

            return toJson(res);
        }
    }
}
