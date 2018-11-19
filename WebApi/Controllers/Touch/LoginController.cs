using BLL;
using Common.Caching;
using Common.Entity;
using Common.Util;
using Model.Operate_Model;
using Model.Table_Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Authorize;

namespace WebApi.Controllers.Touch
{
    public class LoginController : BaseController
    {
        [HttpPost]
        [ActionName("GetAuthCode")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetAuthCode(JObject obj)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code ="0";
            result.Message = "发送失败";
            result.Data = false;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }



            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            UtilityOperate_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<UtilityOperate_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.mobile) )
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            Random random = new Random(Guid.NewGuid().GetHashCode());
            string randomNumber = "";

            for (int i = 0; i < 6; i++)
            {
                randomNumber += random.Next(0, 9).ToString();
            }

            MemcachedNew.Set("authCode", model.mobile, randomNumber, 180);

            //randomNumber = "1234";
            if (!SendMessage.SendAuthCode(model.mobile, randomNumber))
            {
                return toJson(result);
            }

            
            result.Code = "1";
            result.Data = true;
            result.Message = "发送成功";
            return toJson(result);
        }


        [HttpPost]
        [ActionName("LoginRegister")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage LoginRegister(JObject obj)
        {
            ObjectResult<LoginStatus_Model> result = new ObjectResult<LoginStatus_Model>();
            result.Code = "0";
            result.Message = "登陆失败";
            result.Data = null;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            Register_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Register_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.Mobile))
            {
                result.Message = "不合法参数";
                return toJson(result);
            }
            
            string memAuth = MemcachedNew.Get<string>("authCode", model.Mobile);
            if (model.Auth.ToString() != memAuth)
            {
                result.Message = "验证码无效!";
                return toJson(result);
            }

            LoginStatus_Model res = new LoginStatus_Model();

            //查找用户表
            BasUser_Model user = BasUser_BLL.Instance.GetUser(model.Mobile);
            int status = 0;
            CustomerMessage_Model customer = new CustomerMessage_Model();
            if (user!= null && user.UserID > 0)
            {
                //登陆
                status = BasUser_BLL.Instance.Login(user,model);
                if (status == 0)
                {
                    result.Message = "登陆失败";
                    return toJson(result);
                }
                res.LoginStatue = 1;
                customer.UserID = user.UserID;
            }
            else
            {
                //注册
                status = BasUser_BLL.Instance.Register(user, model);
                if (status == 0)
                {
                    result.Message = "登陆失败";
                    return toJson(result);
                }
                res.LoginStatue = 2;
                customer.UserID = status;
            }

            InfCustomer_Model info = InfCustomer_BLL.Instance.GetMark(customer);
            InfMember_Model level = InfMember_BLL.Instance.GetCustomerLevel(customer.UserID);

            res.UserID = info.UserID;
            res.Name = info.Name;
            res.CustomerCode = info.CustomerCode;
            res.SignStatus = info.SignStatus;
            if (level != null)
            {
                res.MemberCode = level.MemberCode;
                res.LevelID = level.LevelID;
            }

            result.Code = "1";
            result.Data = res;
            result.Message = "登陆成功";

            return toJson(result);
        }

        [HttpPost]
        [ActionName("LoginRegisterWithOpenID")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage LoginRegisterWithOpenID(JObject obj)
        {
            ObjectResult<LoginStatus_Model> result = new ObjectResult<LoginStatus_Model>();
            result.Code = "0";
            result.Message = "登陆失败";
            result.Data = null;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            Register_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Register_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.OpenID))
            {
                result.Message = "不合法参数";
                return toJson(result);
            }
            
            //查找用户表
            BasUser_Model user = BasUser_BLL.Instance.GetUserWithOpenID(model.OpenID);
            int status = 0;
            CustomerMessage_Model customer = new CustomerMessage_Model();
            if (user != null && user.UserID > 0)
            {
                //登陆
                status = BasUser_BLL.Instance.Login(user, model);
                if (status == 0)
                {
                    result.Message = "登陆失败";
                    return toJson(result);
                }

                customer.UserID = user.UserID;
            }
            else
            {
                return toJson(result);
            }

            InfCustomer_Model info = InfCustomer_BLL.Instance.GetMark(customer);
            InfMember_Model level = InfMember_BLL.Instance.GetCustomerLevel(customer.UserID);

            LoginStatus_Model res = new LoginStatus_Model();
            res.UserID = info.UserID;
            res.Name = info.Name;
            res.CustomerCode = info.CustomerCode;
            res.SignStatus = info.SignStatus;
            if (level != null)
            {
                res.MemberCode = level.MemberCode;
                res.LevelID = level.LevelID;
            }

            result.Code = "1";
            result.Data = res;
            result.Message = "登陆成功";

            return toJson(result);
        }
    }
}
