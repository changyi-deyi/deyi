using Common.Caching;
using Common.Entity;
using Common.Util;
using Newtonsoft.Json.Linq;
using System;
using BLL;
using Model.Operate_Model;
using Model.Table_Model;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using WebApi.Authorize;

namespace WebApi.Controllers.Touch
{
    public class AdvisoryController : BaseController
    {
        //获取咨询列表
        [HttpPost]
        [ActionName("GetAdvisoryList")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetAdvisoryList(JObject obj)
        {
            ObjectResult<List<OpeAdvisory_Model>> res = new ObjectResult<List<OpeAdvisory_Model>>();
            res.Code = "0";
            res.Message = "咨询列表获取失败";
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

            GetCustomerCoupon_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<GetCustomerCoupon_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            List<OpeAdvisory_Model> result = OpeAdvisory_BLL.Instance.GetAdvisoryList(model.CustomerCode);
            
            if (result != null && result.Count > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "咨询列表获取成功";
            }

            return toJson(res);
        }
        //获取咨询详情
        [HttpPost]
        [ActionName("GetAdvisoryDetail")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetAdvisoryDetail(JObject obj)
        {
            ObjectResult<List<AdvisoryDetail_Model>> res = new ObjectResult<List<AdvisoryDetail_Model>>();
            res.Code = "0";
            res.Message = "咨询详情获取失败";
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

            GetAdvisoryDetail_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<GetAdvisoryDetail_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode)|| model.GroupID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            List<AdvisoryDetail_Model> result = OpeAdvisory_BLL.Instance.GetAdvisoryDetail(model.CustomerCode, model.GroupID);

            if (result != null && result.Count > 0)
            {
                foreach (AdvisoryDetail_Model temp in result)
                {
                    if (temp.AdvisoryIma != null && temp.AdvisoryIma.Count > 0)
                    {
                        foreach (ImaAdvisory_Model item in temp.AdvisoryIma)
                        {
                            item.Path = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.FileName;
                        }
                    }
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "咨询详情获取成功";
            }

            return toJson(res);
        }
        //获取咨询图片
        [HttpPost]
        [ActionName("GetAdvisoryIma")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetAdvisoryIma(JObject obj)
        {
            ObjectResult<List<ImaAdvisory_Model>> res = new ObjectResult<List<ImaAdvisory_Model>>();
            res.Code = "0";
            res.Message = "咨询图片获取失败";
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

            GetAdvisoryDetail_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<GetAdvisoryDetail_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            List<ImaAdvisory_Model> result = ImaAdvisory_BLL.Instance.GetAdvisoryImage(model.GroupID, model.CustomerCode);

            if (result != null && result.Count > 0)
            {
                foreach(ImaAdvisory_Model item in result)
                {
                    item.Path = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.FileName;
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "咨询图片获取成功";
            }

            return toJson(res);
        }
        //删除咨询图片
        [HttpPost]
        [ActionName("DeleteAdvisoryIma")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage DeleteAdvisoryIma(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "咨询图片删除失败";
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

            DeleteAdvisoryDetail_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<DeleteAdvisoryDetail_Model>(strSafeJson);

            if (model.UserID == 0 || model.ID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = ImaAdvisory_BLL.Instance.DeleteAdvisoryImage(model.ID, model.UserID);

            if (result != 0)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "咨询图片删除成功";
            }

            return toJson(res);
        }


        //提交咨询
        [HttpPost]
        [ActionName("SubmitAdvisory")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage SubmitAdvisory(JObject obj)
        {

            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "咨询提交失败";
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

            SubmitAdvisory_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<SubmitAdvisory_Model>(strSafeJson);

            if (model.UserID == 0 || model.ComtinueFlg == 0 ||string.IsNullOrEmpty(model.CustomerCode)|| string.IsNullOrEmpty(model.Content))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            if (model.ComtinueFlg == 1 && model.GroupID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            int result = OpeAdvisory_BLL.Instance.SubmitAdvisory(model);
            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "咨询提交成功";
            }

            return toJson(res);
        }
    }
}
