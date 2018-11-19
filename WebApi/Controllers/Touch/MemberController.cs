using Common.Caching;
using Common.Entity;
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
using Model.View_Model;
using System.Xml;
using Common.Util;

namespace WebApi.Controllers.Touch
{
    public class MemberController : BaseController
    {
        [HttpPost]
        [ActionName("GetMember")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetMember(JObject obj)
        {
            ObjectResult<InfMember_Model> res = new ObjectResult<InfMember_Model>();
            res.Code = "0";
            res.Message = "会员信息获取失败";
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

            Member_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Member_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            InfMember_Model result = InfMember_BLL.Instance.GetMember(model.CustomerCode);


            if (result != null && result.LevelID > 0)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "会员信息获取成功";
            }

            return toJson(res);
        }


        [HttpPost]
        [ActionName("GetMemberLevel")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetMemberLevel(JObject obj)
        {
            ObjectResult<SetMemberLevel_Model> res = new ObjectResult<SetMemberLevel_Model>();
            res.Code = "0";
            res.Message = "会员信息获取失败";
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

            MemberLevel_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<MemberLevel_Model>(strSafeJson);

            if (model.LevelID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            SetMemberLevel_Model result = SetMemberLevel_BLL.Instance.GetMemberLevel(model.LevelID);
            
            if (result != null)
            {
                res.Code = "1";
                res.Data = result;
                res.Message = "会员信息获取成功";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("GetMemberFamily")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetMemberFamily(JObject obj)
        {
            ObjectResult<List<InfFamily_Model>> res = new ObjectResult<List<InfFamily_Model>>();
            res.Code = "0";
            res.Message = "会员家庭信息获取失败";
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

            Member_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Member_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            List<InfFamily_Model> result = InfMember_BLL.Instance.GetMemberFamily(model.CustomerCode);
            
            res.Code = "1";
            res.Data = result;
            res.Message = "会员家庭信息获取成功";
            return toJson(res);
        }

        //兑换优惠券
        [HttpPost]
        [ActionName("ExchangeMember")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage ExchangeMember(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "会员兑换失败";
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

            ExchangeMember_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<ExchangeMember_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CardNumber) || string.IsNullOrEmpty(model.Passeord) || string.IsNullOrEmpty(model.Name)
                || string.IsNullOrEmpty(model.IDNumber)|| string.IsNullOrEmpty(model.CustomerCode) || model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            SetCardNumPas_Model CardNum = SetCardNumPas_BLL.Instance.GetCardNum(model);
            
            if (CardNum == null)
            {
                res.Message = "您输入的卡号或密码有误，请再次输入";
                return toJson(res);
            }
            else if(CardNum.IsExchange == 2)
            {
                res.Message = "此会员卡已兑换";
                return toJson(res);
            }

            InfCustomer_Model customer = InfCustomer_BLL.Instance.GetCustomerMember(model.CustomerCode);
            if (customer == null)
            {
                return toJson(res);
            }
            else if (customer.IsMember == 2)
            {
                res.Message = "您已是会员";
                return toJson(res);
            }

            int result = InfMember_BLL.Instance.SetMember(model, CardNum.ChannelID, CardNum.LevelID);

            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "会员兑换成功";
            }

            return toJson(res);
        }

        //开通会员信息
        [HttpPost]
        [ActionName("OpenMemberInfo")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage OpenMemberInfo(JObject obj)
        {
            ObjectResult<OpenMemberInfo_Model> res = new ObjectResult<OpenMemberInfo_Model>();
            res.Code = "0";
            res.Message = "购买会员信息获取失败";
            res.Data = null;
            OpenMemberInfo_Model result = new OpenMemberInfo_Model();

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

            OpenMember_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<OpenMember_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode)|| model.LevelID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            SetMemberLevel_Model buyLevel = SetMemberLevel_BLL.Instance.GetMemberLevel(model.LevelID);
            
            if (buyLevel == null)
            {
                res.Message = "会员不存在";
                return toJson(res);
            }
            result.MemberLevel = buyLevel;
            List<InfAddress_Model> addressList = InfAddress_BLL.Instance.GetAddress(model.CustomerCode);
            if (addressList != null&& addressList.Count > 0)
            {
                result.addressInfo = addressList;
            }
            
            List<CouponMember_Model> couponList = InfCoupon_BLL.Instance.UsingCouponMember(model.CustomerCode, model.LevelID);
            if (couponList != null && couponList.Count > 0)
            {
                result.CouponInfo = couponList;
            }

            res.Code = "1";
            res.Data = result;
            res.Message = "会员兑换成功";
            return toJson(res);
        }
        
        [HttpPost]
        [ActionName("IDdecide")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage IDdecide(JObject obj)
        {
            ObjectResult < bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "会员信息获取失败";
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

            Member_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Member_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            InfCustomer_Model result = InfCustomer_BLL.Instance.GetCustomerMember(model.CustomerCode);


            if (result != null && string.IsNullOrEmpty(result.IDNumber))
            {
                res.Code = "1";
                res.Message = "未进行身份验证";
            }
            else if (result != null && !string.IsNullOrEmpty(result.IDNumber))
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "已进行身份验证";
            }

            return toJson(res);
        }
        //实名认证
        [HttpPost]
        [ActionName("NameAuthenticate")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage NameAuthenticate(JObject obj)
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

            Authenticate_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Authenticate_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode)|| string.IsNullOrEmpty(model.Name)|| string.IsNullOrEmpty(model.IDNumber)||model.UserID ==0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int result = InfCustomer_BLL.Instance.NameAuthenticate(model.CustomerCode,model.Name,model.IDNumber,model.UserID);
            
            if (result == 1 )
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "身份验证成功";
            }

            return toJson(res);
        }


        [HttpPost]
        [ActionName("ChangeMobile")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage ChangeMobile(JObject obj)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Message = "手机号变更失败";
            result.Data = false;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            ChangeMobile_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<ChangeMobile_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.Mobile)|| model.UserID == 0)
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

            //查找用户表
            int row = BasUser_BLL.Instance.GetUserCount(model.Mobile);

            CustomerMessage_Model customer = new CustomerMessage_Model();
            if (row > 0)
            {
                result.Message = "该手机号已绑定";
            }
            else
            {
                //更改手机号码
                int res = BasUser_BLL.Instance.ChangeMobile(model.Mobile, model.UserID);
                if (res == 1)
                {
                    result.Code = "1";
                    result.Data = true;
                    result.Message = "手机号变更成功";
                }
            }
            
            return toJson(result);
        }




        [HttpPost]
        [ActionName("addMemberOrder")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage addMemberOrder(JObject obj)
        {
            ObjectResult<MemberOrderResultApi_Model> result = new ObjectResult<MemberOrderResultApi_Model>();
            MemberOrderResultApi_Model mReturn = new MemberOrderResultApi_Model();
            result.Code = "0";
            result.Message = "创建支付失败";
            result.Data = null;

            if (obj == null)
            {
                result.Message = "不合法参数";
                return toJson(result);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            AddMemberOrder_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<AddMemberOrder_Model>(strSafeJson);

            if (model == null) {
                result.Message = "不合法参数";
                return toJson(result);
            }

            AddMemberOrderResult_Model order = InfMember_BLL.Instance.addMemberOrder(model);

            if (order != null)
            {
                if (order.Success)
                {
                    decimal temp = order.OrderAmount * 100;
                    int payAmount = (int)temp;
                    Common.WeChat.WeChatPay we = new Common.WeChat.WeChatPay();

                    string notify_url = System.Configuration.ConfigurationManager.AppSettings["NotifyMember"];
                    string XMLres = we.UnifiedOrder(order.NetTradeCode, "购买会员", payAmount, model.WechatOpenID, notify_url);

                    if (string.IsNullOrEmpty(XMLres))
                    {
                        return toJson(result);
                    }

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(XMLres);
                    XmlNode root = doc.FirstChild;
                    string return_code = root["return_code"].InnerText;
                    if (return_code == "FAIL")
                    {
                        result.Message = root["return_msg"].InnerText;
                        return toJson(result);
                    }

                    string result_code = root["result_code"].InnerText;
                    if (result_code == "FAIL")
                    {
                        result.Message = root["err_code_des"].InnerText;
                        return toJson(result);
                    }

                    string prepay_id = root["prepay_id"].InnerText;
                    if (string.IsNullOrWhiteSpace(prepay_id))
                    {
                        return toJson(result);
                    }

                    string js = we.GetJsApiParameters(doc);
                    mReturn.order = order;
                    mReturn.jsParam = js;
                    result.Data = mReturn;
                    result.Code = "1";

                }
                else {
                    result.Message = order.Message;
                }
            }
            return toJson(result);
        }

        [HttpPost]
        [ActionName("GetCustomerInfo")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetCustomerInfo(JObject obj)
        {
            ObjectResult<CustomerInfo_Model> res = new ObjectResult<CustomerInfo_Model>();
            res.Code = "0";
            res.Message = "用户信息获取失败";
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

            Member_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Member_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.CustomerCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            CustomerInfo_Model result = InfCustomer_BLL.Instance.GetCustomerInfo(model.CustomerCode);


            if (result != null)
            {
                if (string.IsNullOrEmpty(result.Name))
                {
                    result.Name = result.Mobile;
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "用户信息获取成功";
            }

            return toJson(res);
        }



        [HttpPost]
        [ActionName("CancelPayMember")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage CancelPayMember(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "取消订单失败";
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

            OpeMemberOrder_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<OpeMemberOrder_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.OrderCode))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            model.UpdateTime = DateTime.Now.ToLocalTime();
            int result = InfMember_BLL.Instance.CancelPayMember(model);


            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "取消订单成功";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("GetMemberActList")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage GetMemberActList(JObject obj)
        {
            ObjectResult<List<InfMemberAct_Model>> res = new ObjectResult<List<InfMemberAct_Model>>();
            res.Code = "0";
            res.Message = "会员活动获取失败";
            res.Data = null;

            List<InfMemberAct_Model> result = InfMemberAct_BLL.Instance.GetMemberActList();


            if (result != null && result.Count>0)
            {
                foreach(InfMemberAct_Model item in result)
                {
                    if (!string.IsNullOrEmpty(item.ImageURL))
                    {
                        item.ImageURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + item.ImageURL;
                    }
                }
                res.Code = "1";
                res.Data = result;
                res.Message = "会员活动获取成功";
            }

            return toJson(res);
        }

        [HttpPost]
        [ActionName("MemberActCount")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage MemberActCount(JObject obj)
        {
            ObjectResult<int> res = new ObjectResult<int>();
            res.Code = "0";
            res.Message = "操作失败";
            res.Data = 0;

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

            Act_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Act_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.MemberCode) || model.ActID == 0 )
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            int rows = OpeJoinAct_BLL.Instance.MemberActCount(model);
            
            res.Code = "1";
            res.Data = rows;
            res.Message = "操作成功";

            return toJson(res);
        }

        [HttpPost]
        [ActionName("AddAct")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage AddAct(JObject obj)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "活动提交失败";
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

            Act_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<Act_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.Name)|| string.IsNullOrEmpty(model.Phone) || string.IsNullOrEmpty(model.IDNumber)
                || string.IsNullOrEmpty(model.MemberCode) || model.ActID == 0|| model.Gender == 0
                || model.Age == 0|| model.AddressID == 0|| model.UserID == 0)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            
            int result = OpeJoinAct_BLL.Instance.AddAct(model);
            
            if (result == 1)
            {
                res.Code = "1";
                res.Data = true;
                res.Message = "活动提交成功";
            }
            else if(result == 2)
            {
                res.Message = "您已领取此次活动，无法再次领取";
            }

            return toJson(res);
        }
    }
}
