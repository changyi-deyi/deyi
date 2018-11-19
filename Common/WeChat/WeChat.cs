using Common.Caching;
using Common.WeChat.Entity;
using Common.WeChat.lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Common.WeChat
{
    public class WeChat : WXBase
    {
        private string APPID = System.Configuration.ConfigurationManager.AppSettings["APPID"];
        private string APPSECRET = System.Configuration.ConfigurationManager.AppSettings["APPSECRET"];

        public bool CheckSignatrue(string signature, string timestamp, string nonce, string token = null)
        {
            return CheckSignature.Check(signature, timestamp, nonce, token);
        }

        public string GetWeChatToken()
        {
            var token = MemcachedNew.Get("WX", "access_token_Finger");
            if (token != null)
            {
                return token.ToString();
            }
            else
            {
                string data = string.Empty;
                HttpStatusCode code = Net.NetUtil.GetResponse("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + APPID + "&secret=" + APPSECRET, null, out data, null, "application/json", "get");


                if (HttpStatusCode.OK == code)
                {
                    if (data.Contains("errcode"))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        WXTokenRS rs = Newtonsoft.Json.JsonConvert.DeserializeObject<WXTokenRS>(data);
                        MemcachedNew.Set("WX", "access_token_Finger", rs.access_token, rs.expires_in - 120);
                        return rs.access_token;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string CodeToToken(string Code)
        {
            string rs = string.Empty;
            Net.NetUtil.GetResponse("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + APPID + "&secret=" + APPSECRET + "&code=" + Code + "&grant_type=authorization_code", null, out rs, null, "appliction/json", "GET");
            return rs;
        }

        public string GetUserDetail(int CompanyID, string openid)
        {
            string rs = string.Empty;
            string access_token = GetWeChatToken();
            Common.Net.NetUtil.GetResponse("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN", null, out rs, null, "appliction/json", "GET");
            return rs;
        }

        public string ShortUrl(string longUrl)
        {
            if (string.IsNullOrEmpty(longUrl))
            {
                return "{\"errcode\":10013,\"errmsg\":\"本地参数错误\"}";
            }

            if (!longUrl.Contains("http") && !longUrl.Contains("https"))
            {
                return "{\"errcode\":10013,\"errmsg\":\"本地参数错误\"}";
            }

            string access_token = GetWeChatToken();
            string strShortUrlJson = string.Empty;

            string prama = "{\"action\":\"long2short\",\"long_url\":\"" + longUrl + "\"}";

            Net.NetUtil.GetResponse("https://api.weixin.qq.com/cgi-bin/shorturl?access_token=" + access_token, prama, out strShortUrlJson, null, "appliction/json", "POST");
            return strShortUrlJson;
        }

        /// <summary>
        /// 获取单个用户详细信息
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="openID"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetWeChatUserInfo(string openID, string language = "zh_CN")
        {
            if (string.IsNullOrEmpty(openID))
            {
                return "{\"errcode\":10013,\"errmsg\":\"本地参数错误\"}";
            }

            if (string.IsNullOrEmpty(language))
            {
                language = "zh_CN";
            }
            else if (language != "zh_CN" || language != "zh_TW" || language != "en")
            {
                language = "zh_CN";
            }

            string access_token = GetWeChatToken();
            string strUserInfoJson = string.Empty;
            Net.NetUtil.GetResponse("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + openID + "&lang=" + language, null, out strUserInfoJson, null, "appliction/json", "GET");

            return strUserInfoJson;
        }

        /// <summary>
        /// 获取带参数的二维码
        /// </summary>
        /// <param name="companyID">公司ID</param>
        /// <param name="strJson">二维码详细信息</param>
        /// <param name="expire_seconds">过期时间</param>
        /// <param name="type">默认:永久的字符串参数值 1:临时 2:永久</param>
        /// <returns></returns>
        public string GetWeChatServiceQRCodeWithParma(string strJson, int expire_seconds = 0, int type = 0)
        {
            string action_name = "QR_LIMIT_STR_SCENE";
            if (type == 1)
            {
                action_name = "QR_SCENE";
            }
            else if (type == 2)
            {
                action_name = "QR_LIMIT_SCENE";
            }
            string access_token = GetWeChatToken();
            string action_info = strJson;

            string parma = parma = "{\"action_name\":\"" + action_name + "\",\"action_info\":{\"scene\":{\"scene_str\":\"" + strJson + "\"}}}";
            if (type == 1 && expire_seconds > 0)
            {
                if (expire_seconds > 0)
                {
                    expire_seconds = 604800;
                }
                parma = "{\"expire_seconds\":" + expire_seconds + ",\"action_name\":\"" + action_name + "\",\"action_info\":{\"scene\":{\"scene_str\":\"" + strJson + "\"}}}";
            }

            string ticket = string.Empty;

            Common.Net.NetUtil.GetResponse("https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + access_token, parma, out ticket, null, "appliction/json");
   


            if (string.IsNullOrEmpty(ticket) || ticket.ToUpper().Contains("ERRCODE"))
            {
                return "";
            }

            WeChatServiceQRCode_Model model = JsonConvert.DeserializeObject<WeChatServiceQRCode_Model>(ticket);

            string imgUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + model.ticket;
            return imgUrl;
        }

        public string GetWeChatJSConfig(string URL)
        {
            var ticket = MemcachedNew.Get("WX", "Ticket_Finger");
            if (ticket != null)
            {
                WXJSConfig wxjsconfig = new WXJSConfig();
                string noncestr = Guid.NewGuid().ToString("N");
                string timestamp = GenerateTimeStamp();
                string signature = GetWeChatJSSignature(noncestr, ticket.ToString(), timestamp, URL);
                wxjsconfig.noncestr = noncestr;
                wxjsconfig.timestamp = timestamp;
                wxjsconfig.appid = APPID;
                wxjsconfig.signature = signature;
                wxjsconfig.ticket = ticket.ToString();
                return Newtonsoft.Json.JsonConvert.SerializeObject(wxjsconfig);
            }
            else
            {
                string Token = GetWeChatToken();
                if (string.IsNullOrWhiteSpace(Token))
                {
                    return string.Empty;
                }
                else
                {
                    string data = string.Empty;
                    HttpStatusCode code = Common.Net.NetUtil.GetResponse("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + Token + "&type=jsapi", null, out data, null, "application/json", "get");
                    if (HttpStatusCode.OK == code)
                    {

                        WXTicketRS rs = Newtonsoft.Json.JsonConvert.DeserializeObject<WXTicketRS>(data);
                        if (rs.errcode == 0 && rs.errmsg.Equals("ok"))
                        {
                            WXJSConfig wxjsconfig = new WXJSConfig();
                            wxjsconfig.noncestr = Guid.NewGuid().ToString("N");
                            wxjsconfig.timestamp = GenerateTimeStamp();
                            wxjsconfig.appid = APPID;
                            wxjsconfig.signature = GetWeChatJSSignature(wxjsconfig.noncestr, rs.ticket, wxjsconfig.timestamp, URL);
                            wxjsconfig.ticket = rs.ticket;
                            MemcachedNew.Set("WX", "Ticket_Finger", rs.ticket, rs.expires_in - 120);
                            return Newtonsoft.Json.JsonConvert.SerializeObject(wxjsconfig);
                        }
                        else
                        {
                            return string.Empty;
                        }


                    }
                    else
                    {
                        return string.Empty;
                    }
                }

            }
        }

        public string GetWeChatJSSignature(string noncestr, string jsapi_ticket, string timestamp, string url)
        {
            SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();
            m_values.Add("noncestr", noncestr);
            m_values.Add("jsapi_ticket", jsapi_ticket);
            m_values.Add("timestamp", timestamp);
            m_values.Add("url", url);
            string buff = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    throw new WxPayException("WXJS内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            byte[] cleanBytes = Encoding.Default.GetBytes(buff);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        public string GetWeChatMediaStream(string MediaID)
        {
            if (string.IsNullOrEmpty(MediaID))
            {
                return null;
            }
            string access_token = GetWeChatToken();
            var stream = string.Empty;

            Net.NetUtil.GetResponse("http://file.api.weixin.qq.com/cgi-bin/media/get?access_token=" + access_token + "&media_id=" + MediaID, null, out stream, null, "appliction/json", "GET");
            return stream;
        }

        public string TemplateMessageSend(MessageTemplate mode)
        {
            string rs = string.Empty;
            string access_token = GetWeChatToken();
            if (!string.IsNullOrEmpty(mode.url))
            {
                mode.url = string.Format(mode.url, APPID);
            }

            string p = JsonConvert.SerializeObject(mode);
            Common.Net.NetUtil.GetResponse("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + access_token, p, out rs);

            return rs;

        }
        public int ActiveCard(WeChatActiveCard_Model model)
        {
            string rs = string.Empty;
            string access_token = GetWeChatToken();
            //string p = JsonConvert.SerializeObject(model);
            string p = "{\"code\":\"" + model.code + "\",\"membership_number\":\"" + model.membership_number + "\",\"card_id\":\"" + model.card_id + "\"}";
            Common.Net.NetUtil.GetResponse("https://api.weixin.qq.com/card/membercard/activate?access_token=" + access_token, p, out rs);

            if (string.IsNullOrEmpty(rs))
            {
                return 0;
            }

            if (rs.Trim().ToLower().Contains("\"errcode\":0"))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        
    }

    //[Serializable]
    //public class WXTokenRS
    //{
    //    public string access_token { get; set; }
    //    public int expires_in { get; set; }
    //}
}
