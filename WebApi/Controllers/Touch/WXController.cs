using BLL;
using Common.Util;
using Model.Operate_Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WebApi.Controllers.Touch
{
    public class WXController : ApiController
    {
        
        [ActionName("NotifyMember")]
        public HttpResponseMessage NotifyMember()
        {
            string postStr = this.Request.Content.ReadAsStringAsync().Result;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(postStr);
            if (doc == null)
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }

            Common.WeChat.WeChatPay we = new Common.WeChat.WeChatPay();

            if (!we.PayResult(doc))
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }

            XmlNode root = doc.FirstChild;
            if (root == null)
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }

            WeChatReturn_Model weChatModel = new WeChatReturn_Model();
            weChatModel.appid = root["appid"].InnerText;
            weChatModel.bank_type = root["bank_type"].InnerText;
            weChatModel.cash_fee = StringUtils.GetDbInt(root["cash_fee"].InnerText);
            weChatModel.fee_type = root["fee_type"].InnerText;
            weChatModel.is_subscribe = root["is_subscribe"].InnerText;
            weChatModel.mch_id = root["mch_id"].InnerText;
            weChatModel.nonce_str = root["nonce_str"].InnerText;
            weChatModel.openid = root["openid"].InnerText;
            weChatModel.out_trade_no = root["out_trade_no"].InnerText;
            weChatModel.result_code = root["result_code"].InnerText;
            weChatModel.time_end = root["time_end"].InnerText;
            weChatModel.sign = root["sign"].InnerText;
            weChatModel.transaction_id = root["transaction_id"].InnerText;
            weChatModel.total_fee = StringUtils.GetDbInt(root["total_fee"].InnerText);
            weChatModel.trade_type = root["trade_type"].InnerText;
            weChatModel.transaction_id = root["transaction_id"].InnerText;

            if (string.IsNullOrEmpty(weChatModel.out_trade_no))
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }
            int SqlResult = InfMember_BLL.Instance.UpdatePayMemberOrderResult(weChatModel.out_trade_no, postStr,1);

            if (SqlResult == 0)
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }
            else
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[SUCCESS]]></return_code>
                                  <return_msg><![CDATA[OK]]></return_msg>
                                </xml>");
            }
        }


        [ActionName("NotifyService")]
        public HttpResponseMessage NotifyService()
        {
            string postStr = this.Request.Content.ReadAsStringAsync().Result;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(postStr);
            if (doc == null)
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }

            Common.WeChat.WeChatPay we = new Common.WeChat.WeChatPay();

            if (!we.PayResult(doc))
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }

            XmlNode root = doc.FirstChild;
            if (root == null)
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }

            WeChatReturn_Model weChatModel = new WeChatReturn_Model();
            weChatModel.appid = root["appid"].InnerText;
            weChatModel.bank_type = root["bank_type"].InnerText;
            weChatModel.cash_fee = StringUtils.GetDbInt(root["cash_fee"].InnerText);
            weChatModel.fee_type = root["fee_type"].InnerText;
            weChatModel.is_subscribe = root["is_subscribe"].InnerText;
            weChatModel.mch_id = root["mch_id"].InnerText;
            weChatModel.nonce_str = root["nonce_str"].InnerText;
            weChatModel.openid = root["openid"].InnerText;
            weChatModel.out_trade_no = root["out_trade_no"].InnerText;
            weChatModel.result_code = root["result_code"].InnerText;
            weChatModel.time_end = root["time_end"].InnerText;
            weChatModel.sign = root["sign"].InnerText;
            weChatModel.transaction_id = root["transaction_id"].InnerText;
            weChatModel.total_fee = StringUtils.GetDbInt(root["total_fee"].InnerText);
            weChatModel.trade_type = root["trade_type"].InnerText;
            weChatModel.transaction_id = root["transaction_id"].InnerText;

            if (string.IsNullOrEmpty(weChatModel.out_trade_no))
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }
            int SqlResult = OpeServiceOrder_BLL.Instance.UpdatePayServiceOrderResult(weChatModel.out_trade_no, postStr, 1);

            if (SqlResult == 0)
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[FAIL]]></return_code>
                                  <return_msg><![CDATA[参数丢失]]></return_msg>
                                </xml>");
            }
            else
            {
                return toXML(null, @"<xml>
                                  <return_code><![CDATA[SUCCESS]]></return_code>
                                  <return_msg><![CDATA[OK]]></return_msg>
                                </xml>");
            }
        }


        public static HttpResponseMessage toXML(Type type, object obj)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                if (type == null)
                {

                    HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(obj.ToString(), Encoding.GetEncoding("UTF-8"), "application/xml") };

                    return result;

                }
                else
                {
                    XmlSerializer xml = new XmlSerializer(type);
                    try
                    {
                        //序列化对象  
                        xml.Serialize(Stream, obj);
                    }
                    catch (InvalidOperationException)
                    {
                        throw;
                    }
                    Stream.Position = 0;
                }

                using (StreamReader sr = new StreamReader(Stream))
                {
                    string str = sr.ReadToEnd();

                    sr.Dispose();

                    Stream.Dispose();

                    HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/xml") };

                    return result;
                }
            }
        }
    }
}
