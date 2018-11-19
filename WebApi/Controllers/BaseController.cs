using Common.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;

namespace WebApi.Controllers
{
    public class BaseController : ApiController
    {
        public BaseController()
        {
            UserID = StringUtils.GetDbInt(HttpContext.Current.Request.Headers["US"], 0);
            Method = HttpContext.Current.Request.Headers["ME"];
            Time = StringUtils.GetDbDateTime(HttpContext.Current.Request.Headers["TI"]);
        }
        public int UserID { get; set; }
        public string Method { get; set; }
        public DateTime Time { get; set; }

        public class LimitPropsContractResolver : DefaultContractResolver
        {
            List<string> props = null;

            bool retain;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="props">传入的属性数组</param>
            /// <param name="retain">true:表示props是需要保留的字段  false：表示props是要排除的字段</param>
            public LimitPropsContractResolver(List<string> props, bool retain = true)
            {
                //指定要序列化属性的清单
                this.props = props;

                this.retain = retain;
            }

            protected override IList<JsonProperty> CreateProperties(Type type,

            MemberSerialization memberSerialization)
            {
                IList<JsonProperty> list =
                base.CreateProperties(type, memberSerialization);

                //只保留清单有列出的属性
                return list.Where(p =>
                {
                    if (retain)
                    {
                        return props.Contains(p.PropertyName);
                    }
                    else
                    {
                        return !props.Contains(p.PropertyName);
                    }
                }).ToList();
            }
        }

        public static HttpResponseMessage toJson(Object obj, string dateFormat = "yyyy-MM-dd HH:mm", List<string> outPutProperty = null, bool retain = true)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JsonSerializerSettings s = new JsonSerializerSettings();
                s.DateFormatString = dateFormat;
                s.NullValueHandling = NullValueHandling.Include;
                if (outPutProperty != null)
                    s.ContractResolver = new LimitPropsContractResolver(outPutProperty, retain);
                str = JsonConvert.SerializeObject(obj, Formatting.Indented, s);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            result.Headers.Add("Author", "jimmy.wu");
            return result;
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

        public class BaseHederResult
        {
            public string Code { get; set; }
            public string Message { get; set; }
        }

    }
}
