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
using System.Xml;

namespace WebApi.Controllers.Touch
{
    public class OrderController : BaseController
    {
        [HttpPost]
        [ActionName("GetServiceOrder")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetServiceOrder(JObject obj)
        {
            ObjectResult<List<ServiceOrder_Model>> res = new ObjectResult<List<ServiceOrder_Model>>();
            res.Code = "0";
            res.Message = "服务订单获取失败";
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

            OrderSelect_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderSelect_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            if (model.flag != 1 && model.flag !=2)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            List<ServiceOrder_Model> list = null;
            //复查画面
            if (model.flag == 1)
            {
                list = OpeServiceOrder_BLL.Instance.GetServiceOrder(model.CustomerCode);
            }
            //我的订单画面
            else
            {
                list = OpeServiceOrder_BLL.Instance.GetOrderList(model.CustomerCode,model.tag);
            }

            if (list != null && list.Count > 0)
            {
                foreach(ServiceOrder_Model item in list)
                {
                    if (!string.IsNullOrEmpty(item.ImageURL))
                    {
                        item.ImageURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.ImageURL;
                    }
                }
                res.Code = "1";
                res.Data = list;
                res.Message = "服务订单获取成功";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("OrderCancel")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage OrderCancel(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "服务订单取消失败";
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

            OrderInfo_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderInfo_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.OrderCode) || model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            int result = OpeServiceOrder_BLL.Instance.OrderCancel(model.OrderCode, model.UserID);


            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "服务订单取消成功";
            }else if (result == 2)
            {
                res.Message = "服务订单有误";
            }
            else if (result == 3)
            {
                res.Message = "此服务订单无法取消";
            }

            return toJson(res);
        }


        [HttpPost]
        [ActionName("GetOrderDetail")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetOrderDetail(JObject obj)
        {
            ObjectResult<ServiceOrder_Model> res = new ObjectResult<ServiceOrder_Model>();
            res.Code = "0";
            res.Message = "服务订单获取失败";
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

            OrderInfo_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderInfo_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.OrderCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            ServiceOrder_Model result = OpeServiceOrder_BLL.Instance.GetOrderDetail(model.OrderCode);


            if (result != null)
            {
                if (!string.IsNullOrEmpty(result.ImageURL))
                {
                    result.ImageURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + result.ImageURL;
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "服务订单获取成功";
            }

            return toJson(res);
        }



        [HttpPost]
        [ActionName("AddServiceOrder")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage AddServiceOrder(JObject obj)
        {
            ObjectResult<ServiceOrderResultApi_Model> res = new ObjectResult<ServiceOrderResultApi_Model>();
            ServiceOrderResultApi_Model mReturn = new ServiceOrderResultApi_Model();
            res.Code = "0";
            res.Message = "服务下单失败";
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

            AddServiceOrder_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<AddServiceOrder_Model>(strSafeJson);


            AddServiceOrderResult_Model order = OpeServiceOrder_BLL.Instance.AddServiceOrder(model);


            if (order != null)
            {
                if (order.Success)
                {
                    model.OrderCode = order.OrderCode;
                    model.PayAmount = order.PayAmount;

                    if (model.PayAmount >= 0)
                    {
                        AddServiceOrderResult_Model payment = OpeServiceOrder_BLL.Instance.PayServiceOrder(model);

                        if (payment != null && payment.Success)
                        {
                            decimal temp = order.PayAmount * 100;
                            int payAmount = (int)temp;
                            Common.WeChat.WeChatPay we = new Common.WeChat.WeChatPay();

                            string notify_url = System.Configuration.ConfigurationManager.AppSettings["NotifyService"];
                            string XMLres = we.UnifiedOrder(payment.NetTradeCode, "购买服务", payAmount, model.WechatOpenID, notify_url);

                            if (string.IsNullOrEmpty(XMLres))
                            {
                                return toJson(res);
                            }

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(XMLres);
                            XmlNode root = doc.FirstChild;
                            string return_code = root["return_code"].InnerText;
                            if (return_code == "FAIL")
                            {
                                res.Message = root["return_msg"].InnerText;
                                return toJson(res);
                            }

                            string result_code = root["result_code"].InnerText;
                            if (result_code == "FAIL")
                            {
                                res.Message = root["err_code_des"].InnerText;
                                return toJson(res);
                            }

                            string prepay_id = root["prepay_id"].InnerText;
                            if (string.IsNullOrWhiteSpace(prepay_id))
                            {
                                return toJson(res);
                            }

                            string js = we.GetJsApiParameters(doc);
                            mReturn.order = payment;
                            mReturn.jsParam = js;
                            res.Data = mReturn;
                            res.Code = "1";
                        }
                    }
                    else
                    {
                        res.Code = "2";
                    }

                }
                else
                {
                    res.Message = order.Message;
                }
            }

            return toJson(res);
        }


        [HttpPost]
        [ActionName("PayServiceOrder")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage PayServiceOrder(JObject obj)
        {
            ObjectResult<ServiceOrderResultApi_Model> res = new ObjectResult<ServiceOrderResultApi_Model>();
            ServiceOrderResultApi_Model mReturn = new ServiceOrderResultApi_Model();
            res.Code = "0";
            res.Message = "服务下单失败";
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

            AddServiceOrder_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<AddServiceOrder_Model>(strSafeJson);

            ServiceOrder_Model order = OpeServiceOrder_BLL.Instance.GetOrderDetail(model.OrderCode);

            model.PayAmount = order.OrderAmount;

            AddServiceOrderResult_Model payment = OpeServiceOrder_BLL.Instance.PayServiceOrder(model);

            if (payment != null && payment.Success)
            {
                decimal temp = model.PayAmount * 100;
                int payAmount = (int)temp;
                Common.WeChat.WeChatPay we = new Common.WeChat.WeChatPay();

                string notify_url = System.Configuration.ConfigurationManager.AppSettings["NotifyService"];
                string XMLres = we.UnifiedOrder(payment.NetTradeCode, "购买服务", payAmount, model.WechatOpenID, notify_url);

                if (string.IsNullOrEmpty(XMLres))
                {
                    return toJson(res);
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(XMLres);
                XmlNode root = doc.FirstChild;
                string return_code = root["return_code"].InnerText;
                if (return_code == "FAIL")
                {
                    res.Message = root["return_msg"].InnerText;
                    return toJson(res);
                }

                string result_code = root["result_code"].InnerText;
                if (result_code == "FAIL")
                {
                    res.Message = root["err_code_des"].InnerText;
                    return toJson(res);
                }

                string prepay_id = root["prepay_id"].InnerText;
                if (string.IsNullOrWhiteSpace(prepay_id))
                {
                    return toJson(res);
                }

                string js = we.GetJsApiParameters(doc);
                mReturn.order = payment;
                mReturn.jsParam = js;
                res.Data = mReturn;
                res.Code = "1";
            }

            return toJson(res);
        }

    }
}
