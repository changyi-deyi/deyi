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
    public class AddressController : BaseController
    {
        //地址管理
        [HttpPost]
        [ActionName("GetAddressList")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetAddressList(JObject obj)
        {
            ObjectResult<List<InfAddress_Model>> res = new ObjectResult<List<InfAddress_Model>>();
            res.Code = "0";
            res.Message = "地址获取失败";
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
            List<InfAddress_Model> result = InfAddress_BLL.Instance.GetAddress(model.CustomerCode);

            if (result != null && result.Count > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "地址获取成功";
            }

            return toJson(res);
        }


        //地址保存
        [HttpPost]
        [ActionName("SaveAddress")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage SaveAddress(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "地址保存失败";
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

            SaveAddress_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveAddress_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode)|| string.IsNullOrEmpty(model.Name)|| string.IsNullOrEmpty(model.Phone) 
                || string.IsNullOrEmpty(model.Address) || model.ProvinceID == 0|| model.CityID == 0 
                || model.DistrictID == 0 || model.IsDefault == 0 || model.IsNew == 0 || model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            if (model.ID ==0 && model.IsNew == 2)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = InfAddress_BLL.Instance.SaveAddress(model);


            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "地址保存成功";
            }

            return toJson(res);
        }

        //地址删除
        [HttpPost]
        [ActionName("DeleteAddress")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage DeleteAddress(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "地址删除失败";
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

            SaveAddress_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveAddress_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode) || model.ID == 0 || model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = InfAddress_BLL.Instance.DeleteAddress(model);


            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "地址删除成功";
            }

            return toJson(res);
        }


        //三级联动
        [HttpPost]
        [ActionName("GetDistrict")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetDistrict(JObject obj)
        {
            ObjectResult<List<SetDistrict_Model>> res = new ObjectResult<List<SetDistrict_Model>>();
            res.Code = "0";
            res.Message = "地区获取失败";
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

            GetDistrict model = Newtonsoft.Json.JsonConvert.DeserializeObject<GetDistrict>(strSafeJson);
            if (!string.IsNullOrEmpty(model.REGION_LEVEL) && model.REGION_LEVEL != "1"
                && model.REGION_LEVEL != "2" && model.REGION_LEVEL != "3" && string.IsNullOrEmpty(model.PARENT_ID))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            List<SetDistrict_Model> result = SetDistrict_BLL.Instance.GetDistrict(model.REGION_LEVEL, model.PARENT_ID);
            if (result != null && result.Count > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "地区获取成功";
            }
            return toJson(res);
        }
    }
}
