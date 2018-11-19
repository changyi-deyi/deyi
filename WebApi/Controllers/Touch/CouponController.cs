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
    public class CouponController : BaseController
    {
        [HttpPost]
        [ActionName("GetCoupon")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetCoupon(JObject obj)
        {
            ObjectResult<List<InfCoupon_Model>> res = new ObjectResult<List<InfCoupon_Model>>();
            res.Code = "0";
            res.Message = "优惠券属性获取失败";
            res.Data = null;
            
            List<InfCoupon_Model> result = InfCoupon_BLL.Instance.GetCoupon();


            if (result != null && result.Count > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "优惠券属性获取成功";
            }

            return toJson(res);
        }

        //我的优惠券
        [HttpPost]
        [ActionName("GetCustomerCoupon")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetCustomerCoupon(JObject obj)
        {
            ObjectResult<List<CustomerCoupon_Model>> res = new ObjectResult<List<CustomerCoupon_Model>>();
            res.Code = "0";
            res.Message = "优惠券属性获取失败";
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
            List<CustomerCoupon_Model> result = OpeCustomerCoupon_BLL.Instance.GetCustomerCoupon(model.CustomerCode);

            if (result != null && result.Count > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "优惠券属性获取成功";
            }

            return toJson(res);
        }

        //兑换码兑换
        [HttpPost]
        [ActionName("CodeExchange")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage CodeExchange(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "优惠券兑换失败";
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

            CouponExchange_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<CouponExchange_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode) || string.IsNullOrEmpty(model.ExchangeCode) || model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            //取得优惠券ID
            InfCoupon_Model CodeID = OpeCouponCode_BLL.Instance.GetCouponCodeID(model.ExchangeCode);

            if (CodeID != null && CodeID.ID != 0)
            {
                int result = OpeCustomerCoupon_BLL.Instance.CodeExchange(model.CustomerCode, model.UserID, CodeID);
                if (result == 1)
                {
                    res.Code = "1";
                    res.Data = true;
                    res.Message = "优惠券兑换成功";
                }
            }

            return toJson(res);
        }

        //优惠券兑换
        [HttpPost]
        [ActionName("BalanceExchange")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage BalanceExchange(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "优惠券兑换失败";
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

            CouponExchange_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<CouponExchange_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode) || model.UserID == 0 || model.CouponID == 0 || model.ExchangeAmount == decimal.Zero)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            //取得健康币余额
            InfCustomer_Model customer = InfCustomer_BLL.Instance.GetBalance(model.UserID, model.ExchangeAmount);
            if(customer == null)
            {
                res.Message = "您的健康币余额不足";
                return toJson(res);
            }
            
            //取得优惠券信息
            InfCoupon_Model CouponInfo = InfCoupon_BLL.Instance.GetCouponInfo(model.CouponID);
            if (CouponInfo == null)
            {
                res.Message = "优惠券已下架";
                return toJson(res);
            }
            DateTime today = DateTime.Now;
            DateTime startData;
            DateTime endData;
            if (CouponInfo.ValidType == 1)
            {
                string[] sArray = Regex.Split(CouponInfo.ValidRUle, "-", RegexOptions.IgnoreCase);
                startData = DateTime.ParseExact(sArray[0], "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);
                endData = DateTime.ParseExact(sArray[1], "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);

                if(DateTime.Compare(today, startData) <0 || DateTime.Compare(today, endData) > 0)
                {
                    res.Message = "优惠券已下架";
                    return toJson(res);
                }
            }
            //优惠券兑换
            int result = OpeCustomerCoupon_BLL.Instance.BalanceExchange(model.CustomerCode, model.UserID, customer.Balance, CouponInfo);
            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "优惠券兑换成功";
            }
            else if (result == 2)
            {
                res.Message = "此优惠券不可重复领取";
            }

            return toJson(res);
        }
    }
}
