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

namespace WebApi.Controllers.Touch
{
    public class LevelController : BaseController
    {
        [HttpPost]
        [ActionName("GetLevel")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetLevel(JObject obj)
        {
            ObjectResult<List<SetMemberLevel_Model>> res = new ObjectResult<List<SetMemberLevel_Model>>();
            res.Code = "0";
            res.Message = "会员等级获取失败";
            res.Data = null;

            List<SetMemberLevel_Model> result = SetMemberLevel_BLL.Instance.GetLevel();


            if (result != null && result.Count > 0)
            {
                foreach(SetMemberLevel_Model item in result)
                {
                    if (!string.IsNullOrEmpty(item.IconURL))
                    {
                        item.IconURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.IconURL;
                    }
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "会员等级获取成功";
            }

            return toJson(res);
        }


        [HttpPost]
        [ActionName("GetLevelDetail")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetLevelDetail(JObject obj)
        {
            ObjectResult<SetMemberLevel_Model> result = new ObjectResult<SetMemberLevel_Model>();
            result.Code = "0";
            result.Message = "会员等级获取失败";
            result.Data = null;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }


            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            UtilityOperate_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<UtilityOperate_Model>(strSafeJson);

            if (model.ID < 1) {
                result.Message = "不合法参数";
                return toJson(result);
            }

            SetMemberLevel_Model mReturn = SetMemberLevel_BLL.Instance.GetMemberLevel(model.ID);


            result.Code = "1";
            result.Data = mReturn;
            result.Message = "会员等级获取成功";

            return toJson(result);
        }



    }
}
