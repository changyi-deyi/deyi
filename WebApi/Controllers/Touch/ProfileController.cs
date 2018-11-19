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
using System.Web.Http;
using WebApi.Authorize;

namespace WebApi.Controllers.Touch
{
    public class ProfileController : BaseController
    {
        [HttpPost]
        [ActionName("GetProfile")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetProfile(JObject obj)
        {
            ObjectResult<List<ProfileList_Model>> res = new ObjectResult<List<ProfileList_Model>>();
            res.Code = "0";
            res.Message = "健康档案获取失败";
            res.Data = null;
            List<ProfileList_Model> result = new List<ProfileList_Model>();

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

            ProfileMessage_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileMessage_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            List<string> UploadDates = InfCustomerProfile_BLL.Instance.GetUploadDate(model.CustomerCode, model.Type);
            if (UploadDates != null && UploadDates.Count > 0)
            {
                foreach(string UploadDate in UploadDates)
                {
                    ProfileList_Model item = new ProfileList_Model();
                    item.UploadDate = UploadDate;
                    item.UploadData = InfCustomerProfile_BLL.Instance.GetProfileAndImaCount(model.CustomerCode, UploadDate, model.Type);
                    result.Add(item);
                }
            }

            if (result.Count > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "健康档案获取成功";
            }

            return toJson(res);
        }


        [HttpPost]
        [ActionName("GetProfileDetail")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetProfileDetail(JObject obj)
        {
            ObjectResult<List<ImaCustomerProfile_Model>> res = new ObjectResult<List<ImaCustomerProfile_Model>>();
            res.Code = "0";
            res.Message = "健康档案详情获取失败";
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

            CustomerProfile_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerProfile_Model>(strSafeJson);

            if (model.ID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            List<ImaCustomerProfile_Model> list = ImaCustomerProfile_BLL.Instance.GetProfileIma(model.ID);

            if (list != null && list.Count > 0)
            {
                foreach(ImaCustomerProfile_Model item in list)
                {
                    item.Path = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.FileName;
                }
                res.Code = "1";
                res.Data = list;
                res.Message = "健康档案详情获取成功";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("DeleteProfileIma")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage DeleteProfileIma(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "图片删除失败";
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

            DeleteProfileIma_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<DeleteProfileIma_Model>(strSafeJson);

            if (model.ID == 0 || model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = ImaCustomerProfile_BLL.Instance.DeleteProfileIma(model.ID,model.UserID);


            if (result != 0)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "图片删除成功";
            }

            return toJson(res);
        }


        [HttpPost]
        [ActionName("ProfileUpload")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage ProfileUpload(JObject obj)
        {
            ObjectResult<string> res = new ObjectResult<string>();
            res.Code = "0";
            res.Message = "提交失败";
            res.Data = "";

            if (obj == null)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            ProfileUpload_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<ProfileUpload_Model>(strSafeJson);

            int result = InfCustomerProfile_BLL.Instance.ProfileUpload(model, model.UploadImage);
            if (result != 0)
            {
                res.Code = "1";
                res.Message = "提交成功";
                res.Data = "";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("ProfileDelete")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage ProfileDelete(JObject obj)
        {
            ObjectResult<string> res = new ObjectResult<string>();
            res.Code = "0";
            res.Message = "档案删除失败";
            res.Data = "";

            if (obj == null)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            CustomerProfile_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerProfile_Model>(strSafeJson);

            if (model.ID == 0 || model.UserID ==0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = InfCustomerProfile_BLL.Instance.ProfileDelete(model.ID, model.UserID);
            if (result != 0)
            {
                res.Code = "1";
                res.Message = "档案删除成功";
                res.Data = "";
            }

            return toJson(res);
        }

    }
}
