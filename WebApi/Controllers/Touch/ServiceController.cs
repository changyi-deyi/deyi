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
    public class ServiceController : BaseController
    {
        [HttpPost]
        [ActionName("GetService")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetService(JObject obj)
        {
            ObjectResult<List<InfService_Model>> res = new ObjectResult<List<InfService_Model>>();
            res.Code = "0";
            res.Message = "服务信息获取失败";
            res.Data = null;
            
            List<InfService_Model> result = InfService_BLL.Instance.GetService();

            if (result != null && result.Count > 0)
            {
                foreach(InfService_Model item in result)
                {
                    if (!string.IsNullOrEmpty(item.ListImageURL))
                    {
                        item.ListImageURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.ListImageURL;
                    }
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "服务信息获取成功";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("GetServiceDetail")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetServiceDetail(JObject obj)
        {
            ObjectResult<ServiceDetail_Model> res = new ObjectResult<ServiceDetail_Model>();
            res.Code = "0";
            res.Message = "服务信息获取失败";
            res.Data = null;

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

            GetServiceDetail_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<GetServiceDetail_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.ServiceCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            ServiceDetail_Model result = InfService_BLL.Instance.GetServiceDetail(model.ServiceCode);

            if (result != null)
            {
                if (result.ImaList != null && result.ImaList.Count > 0)
                {
                    foreach (ImaService_Model item in result.ImaList)
                    {
                        item.Path = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.FileName;
                    }
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "服务信息获取成功";
            }

            return toJson(res);
        }

        //评价
        [HttpPost]
        [ActionName("SetComment")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage SetComment(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "服务评价失败";
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

            ServiceComment_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<ServiceComment_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.OrderCode) || string.IsNullOrEmpty(model.DoctorCode) || string.IsNullOrEmpty(model.CustomerCode) 
                || model.UserID == 0 || model.IsSolute == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            OpeServiceOrder_Model serviceOrder = OpeServiceOrder_BLL.Instance.GetCommentStatus(model.OrderCode);

            if (serviceOrder == null)
            {
                return toJson(res);
            }
            else if (serviceOrder.CommentStatus == 2)
            {
                res.Message = "您已对此次服务进行评价";
                return toJson(res);
            }

            int result = OpeComment_BLL.Instance.ServiceComment(model);

            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "服务评价成功";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("BuyServiceInfo")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage BuyServiceInfo(JObject obj)
        {
            ObjectResult<BuyServiceInfo_Model> res = new ObjectResult<BuyServiceInfo_Model>();
            res.Code = "0";
            res.Message = "服务信息获取失败";
            res.Data = null;

            BuyServiceInfo_Model result = new BuyServiceInfo_Model();
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

            BuyService_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<BuyService_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.ServiceCode)|| string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            result.ImaList = ImaService_BLL.Instance.GetServiceImage(model.ServiceCode);
            if (result.ImaList!= null && result.ImaList.Count > 0)
            {
                foreach(ImaService_Model item in result.ImaList)
                {
                    item.Path = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.FileName;
                }
            }

            result.CouponList = InfCoupon_BLL.Instance.UsingCouponService(model.CustomerCode, model.ServiceCode);


            res.Code = "1";
            res.Data = result;
            res.Message = "服务信息获取成功";
            return toJson(res);
        }

        [HttpPost]
        [ActionName("AfterService")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage AfterService(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "售后服务申请失败";
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

            AfterService_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<AfterService_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.OrderCode) || string.IsNullOrEmpty(model.Reason)|| model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = OpeServiceOrder_BLL.Instance.AfterService(model.OrderCode, model.Reason, model.UserID);


            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "售后服务申请成功";
            }
            else if (result == 2)
            {
                res.Message = "服务订单有误";
            }
            else if (result == 3)
            {
                res.Message = "此服务订单无法申请售后服务";
            }

            return toJson(res);
        }
    }
}
