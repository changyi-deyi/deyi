using Common.Net;
using Common.WeChat.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common.WeChat
{
    public class WeChatPay
    {
        string appid = System.Configuration.ConfigurationManager.AppSettings["APPID"];
        string mch_id = System.Configuration.ConfigurationManager.AppSettings["mch_id"];
        string ip = System.Configuration.ConfigurationManager.AppSettings["spbill_create_ip"];
        public string UnifiedOrder(string out_trade_no, string body, int total_fee, string openid,string notify_url)
        {
            string Data = string.Empty;
            string url = @"https://api.mch.weixin.qq.com/pay/unifiedorder";

            WxPayData data = new WxPayData();
            data.SetValue("appid", appid);
            data.SetValue("mch_id", mch_id);
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
            data.SetValue("body", body);
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("total_fee", total_fee);
            data.SetValue("spbill_create_ip", ip);
            data.SetValue("notify_url", notify_url);
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("sign_type", "MD5");
            data.SetValue("openid", openid);
            data.SetValue("sign", data.MakeSign());
            string xmlData = data.ToXml();
            HttpStatusCode code = NetUtil.GetResponse(url, xmlData, out Data);
            return Data;
        }



        public string GetJsApiParameters(XmlDocument AddOrderRs)
        {
            if (AddOrderRs != null)
            {
                XmlNode root = null;
                root = AddOrderRs.FirstChild;
                WxPayData payData = new WxPayData();
                payData.SetValue("appId", appid);
                payData.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
                payData.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
                payData.SetValue("package", "prepay_id=" + root["prepay_id"].InnerText);
                payData.SetValue("signType", "MD5");
                payData.SetValue("paySign", payData.MakeSign());
                return payData.ToJson();
            }
            else
            {
                return string.Empty;
            }
        }


        public bool PayResult(XmlDocument xml)
        {
            bool rs = false;
            if (xml != null)
            {
                XmlNode root = null;
                root = xml.FirstChild;
                if (root != null)
                {
                    string appid = root["appid"] == null ? "" : root["appid"].InnerText;
                    string mch_id = root["mch_id"] == null ? "" : root["mch_id"].InnerText;
                    string openid = root["openid"] == null ? "" : root["openid"].InnerText;
                    string is_subscribe = root["is_subscribe"] == null ? "" : root["is_subscribe"].InnerText;
                    string sub_is_subscribe = root["sub_is_subscribe"] == null ? "" : root["sub_is_subscribe"].InnerText;
                    string attach = root["attach"] == null ? "" : root["attach"].InnerText;
                    string bank_type = root["bank_type"] == null ? "" : root["bank_type"].InnerText;
                    string cash_fee = root["cash_fee"] == null ? "" : root["cash_fee"].InnerText;
                    string fee_type = root["fee_type"] == null ? "" : root["fee_type"].InnerText;
                    string nonce_str = root["nonce_str"] == null ? "" : root["nonce_str"].InnerText;
                    string out_trade_no = root["out_trade_no"] == null ? "" : root["out_trade_no"].InnerText;
                    string result_code = root["result_code"] == null ? "" : root["result_code"].InnerText;
                    string return_code = root["return_code"] == null ? "" : root["return_code"].InnerText;
                    string sign = root["sign"] == null ? "" : root["sign"].InnerText;
                    string time_end = root["time_end"] == null ? "" : root["time_end"].InnerText;
                    string total_fee = root["total_fee"] == null ? "" : root["total_fee"].InnerText;
                    string trade_type = root["trade_type"] == null ? "" : root["trade_type"].InnerText;
                    string transaction_id = root["transaction_id"] == null ? "" : root["transaction_id"].InnerText;

                    WxPayData data = new WxPayData();
                    data.SetValue("appid", appid);
                    data.SetValue("mch_id", mch_id);
                    data.SetValue("openid", openid);
                    data.SetValue("attach", attach);
                    data.SetValue("bank_type", bank_type);
                    data.SetValue("cash_fee", cash_fee);
                    data.SetValue("fee_type", fee_type);
                    data.SetValue("is_subscribe", is_subscribe);
                    data.SetValue("sub_is_subscribe", sub_is_subscribe);
                    data.SetValue("nonce_str", nonce_str);
                    data.SetValue("out_trade_no", out_trade_no);
                    data.SetValue("result_code", result_code);
                    data.SetValue("return_code", return_code);
                    data.SetValue("time_end", time_end);
                    data.SetValue("total_fee", total_fee);
                    data.SetValue("trade_type", trade_type);
                    data.SetValue("transaction_id", transaction_id);
                    rs = data.MakeSign().Equals(sign);
                    if (rs)
                    {
                        string temp = QueryPaymentByID(transaction_id, out_trade_no);
                        xml.LoadXml(temp);
                        root = xml.FirstChild;
                        if (root["return_code"].InnerText.ToString() == "SUCCESS"
                            && root["result_code"].InnerText.ToString() == "SUCCESS"
                            && root["trade_state"].InnerText.ToString() == "SUCCESS")
                        {
                            return rs;
                        }
                        else
                        {
                            rs = false;
                            return rs;
                        }
                    }
                    return rs;
                }
                return rs;
            }
            return false;
        }


        public string QueryPaymentByID(string transaction_id, string out_trade_no)
        {
            string Data = string.Empty;

            WxPayData data = new WxPayData();
            data.SetValue("appid", appid);
            data.SetValue("mch_id", mch_id);
            if (!string.IsNullOrWhiteSpace(transaction_id))
            {
                data.SetValue("transaction_id", transaction_id);
            }
            if (!string.IsNullOrWhiteSpace(out_trade_no))
            {
                data.SetValue("out_trade_no", out_trade_no);
            }
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
            data.SetValue("sign", data.MakeSign());
            string xmlData = data.ToXml();
            NetUtil.GetResponse("https://api.mch.weixin.qq.com/pay/orderquery", xmlData, out Data);
            return Data;
        }

        public string QueryPayment(string out_trade_no)
        {
            string Data = string.Empty;
            WxPayData data = new WxPayData();
            data.SetValue("appid", appid);
            data.SetValue("mch_id", mch_id);
            data.SetValue("out_trade_no", out_trade_no);
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
            data.SetValue("sign", data.MakeSign());
            string xmlData = data.ToXml();
            NetUtil.GetResponse("https://api.mch.weixin.qq.com/pay/orderquery", xmlData, out Data);
            return Data;
        }

    }
}
