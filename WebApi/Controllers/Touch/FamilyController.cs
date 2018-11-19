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
    public class FamilyController : BaseController
    {

        //找家人
        [HttpPost]
        [ActionName("GetFamilyList")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetFamilyList(JObject obj)
        {
            ObjectResult<List<InfFamily_Model>> result = new ObjectResult<List<InfFamily_Model>>();
            result.Code = "0";
            result.Message = "数据抽取失败";
            result.Data = null;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }
            
            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            GetFamilyInfo_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<GetFamilyInfo_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                result.Message = "不合法参数";
                return toJson(result);
            }
            InfMember_Model member = InfMember_BLL.Instance.GetMember(model.CustomerCode);

            if (member != null && !string.IsNullOrEmpty(member.FamilyCode))
            {
                List<InfFamily_Model> familyList = InfFamily_BLL.Instance.GetFamilyList(member.FamilyCode);
                if (familyList != null && familyList.Count > 0)
                {
                    result.Code = "1";
                    result.Data = familyList;
                    result.Message = "数据抽取成功";
                }
            }
            else
            {
                result.Code = "2";
                result.Message = "非会员用户无法编辑家人";
            }
            return toJson(result);
        }

        //添加/修改成员信息
        [HttpPost]
        [ActionName("ChangeFamily")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage ChangeFamily(JObject obj)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Message = "成员信息编辑失败";
            result.Data = false;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            ChangeFamily_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<ChangeFamily_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode)|| string.IsNullOrEmpty(model.Name)
                || model.UserID == 0 || model.ChangeFlg == 0)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            if (model.ChangeFlg == 2 && model.ID == 0)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }
            if (model.ChangeFlg == 2)
            {
                int res = InfFamily_BLL.Instance.UpdateFamily(model.ID, model.Name, model.IDNumber, model.Relationship, model.UserID);
                if (res == 1)
                {
                    result.Code = "1";
                    result.Data = true;
                    result.Message = "成员信息编辑成功";
                }
            }
            else if (model.ChangeFlg == 1)
            {
                InfMember_Model member = InfMember_BLL.Instance.GetMember(model.CustomerCode);

                if (member != null && !string.IsNullOrEmpty(member.FamilyCode))
                {
                    int res = InfFamily_BLL.Instance.InsertFamily(member.FamilyCode,model.Name,model.IDNumber,model.Relationship,model.UserID);
                    if (res == 1)
                    {
                        result.Code = "1";
                        result.Data = true;
                        result.Message = "成员信息编辑成功";
                    }
                }
                else
                {
                    result.Code = "2";
                    result.Message = "非会员用户无法编辑家人";
                }
            }
            return toJson(result);
        }

        [HttpPost]
        [ActionName("DeleteFamily")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage DeleteFamily(JObject obj)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Message = "成员信息删除失败";
            result.Data = false;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            ChangeFamily_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<ChangeFamily_Model>(strSafeJson);

            if (model.UserID == 0 || model.ID == 0)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }
            int res = InfFamily_BLL.Instance.DeleteFamily(model.ID,model.UserID);
            if (res == 1)
            {
                result.Code = "1";
                result.Data = true;
                result.Message = "成员信息删除成功";
            }
            return toJson(result);
        }
    }
}
