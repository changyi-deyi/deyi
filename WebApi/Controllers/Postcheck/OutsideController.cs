using BLL;
using Common.Util;
using Common.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

namespace WebApi.Controllers.Postcheck
{
    public class OutsideController : ApiController
    {
        //身份验证
        [HttpPost]
        [ActionName("IDNumberCheck")]
        public HttpResponseMessage IDNumberCheck(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "身份验证失败";
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

            IDcheck_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<IDcheck_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.IDNumber)|| model.LevelID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            List<string> ChannelList = System.Configuration.ConfigurationManager.AppSettings["OutsideChannel"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();

            int result = InfCustomer_BLL.Instance.CheckCustomer(model.IDNumber,model.LevelID, ChannelList);

            if (result == 1)
            {
                res.Code = "1";
                res.Message = "身份验证成功";
                res.Data = true;
            }
            else if(result == 2)
            {
                res.Message = "身份验证异常,存在复数个相同条件会员";
            }
            else
            {
                res.Code = "1";
                res.Message = "身份验证不通过";
            }
            
            return toJson(res);

        }


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
            result.Headers.Add("Author", "Aaron.Hon");
            return result;
        }

        [Serializable]
        public class IDcheck_Model
        {
            //会员等级
            public int LevelID { get; set; }
            //证件号码
            public string IDNumber { get; set; }

        }
    }
}
