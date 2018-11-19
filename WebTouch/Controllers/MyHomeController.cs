using Common.Entity;
using Common.Util;
using Model.Operate_Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTouch.Model;

namespace WebTouch.Controllers
{
    public class MyHomeController : BaseController
    {
        // GET: MyHome
        public ActionResult MyHome()
        {
            return View();
        }
        // 设置
        public ActionResult Setting()
        {
            return View();
        }
        // 我的咨询
        public ActionResult ConsultationList()
        {
            return View();
        }
        // 关注的医生
        public ActionResult MyFavDoctor()
        {
            return View();
        }
        // 我的订单
        public ActionResult MyOrderList_All()
        {
            return View();
        }
        // 我的订单-申请售后
        public ActionResult MyOrderList_PaybackDetail()
        {
            return View();
        }
        // 我的订单-评价
        public ActionResult MyOrderList_CommentDetail()
        {
            return View();
        }
        // 兑换会员
        public ActionResult ExchangeVIP()
        {
            return View();
        }
        // 我的优惠券
        public ActionResult MyCouponList()
        {
            return View();
        }
        // 兑换优惠券
        public ActionResult ExchangeCoupon()
        {
            return View();
        }
        // 我的家人
        public ActionResult MyFamily()
        {
            return View();
        }
        // 添加管理家人
        public ActionResult FamilyDetail()
        {
            return View();
        }
        // 地址管理
        public ActionResult AddressList()
        {
            return View();
        }
        // 地址详情
        public ActionResult AddressDetail()
        {
            return View();
        }
        // 反馈意见
        public ActionResult Submitadvise()
        {
            return View();
        }
        // 咨询详情
        public ActionResult MyConsultation()
        {
            return View();
        }
        // 设置-实名认证
        public ActionResult ConfirmRealName()
        {
            return View();
        }
        // 设置-更改登陆手机号码
        public ActionResult ModifyTel()
        {
            return View();
        }
        // 会员俱乐部
        public ActionResult MyVipHome()
        {
            return View();
        }
        // 会员俱乐部详情
        public ActionResult ExchangeVipHome()
        {
            return View();
        }
        // 取得我的优惠券
        public ActionResult getMyCouponList()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                GetCustomerCoupon_Model model = new GetCustomerCoupon_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Coupon", "GetCustomerCoupon", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 优惠券
        public ActionResult getCoupon()
        {
            string postJson = string.Empty;
            string data = string.Empty;
            GetPostResponseNoRedirect("Coupon", "GetCoupon", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
        //串码兑换优惠券
        public ActionResult codeExchange(string ExchangeCode)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;
            if (string.IsNullOrEmpty(ExchangeCode))
            {
                res.Code = "2";
                res.Message = "串码未输入";
                return Json(res);
            }
            else
            {
                string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
                //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

                Cookie_Model cookieModel = new Cookie_Model();
                if (!string.IsNullOrWhiteSpace(srtCookie))
                {
                    cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                    CouponExchange_Model model = new CouponExchange_Model();

                    model.CustomerCode = cookieModel.CustomerCode;
                    model.ExchangeCode = ExchangeCode;
                    model.UserID = cookieModel.UserID;
                    string postJson = JsonConvert.SerializeObject(model);
                    string data = string.Empty;
                    if(GetPostResponseNoRedirect("Coupon", "CodeExchange", postJson, out data, true, false))
                    {
                        return Content(data, "application/json; charset=utf-8");
                    }
                    else
                    {
                        return Json(res);
                    }
                }
                else
                {
                    return Json(res);
                }
            }
        }
        //优惠券兑换
        public ActionResult balanceExchange(int CouponID, decimal ExchangeAmount)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                CouponExchange_Model model = new CouponExchange_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.UserID = cookieModel.UserID;
                model.CouponID = CouponID;
                model.ExchangeAmount = ExchangeAmount;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Coupon", "BalanceExchange", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        //兑换会员
        public ActionResult exchangeMember(string CardNumber, string Passeord, string IDNumber, string Name)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;
            if (string.IsNullOrEmpty(CardNumber) || string.IsNullOrEmpty(Passeord) || string.IsNullOrEmpty(IDNumber) || string.IsNullOrEmpty(Name))
            {
                res.Code = "2";
                res.Message = "卡号、密码、身份证号、姓名未输入";
                return Json(res);
            }
            else
            {
                string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
                //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

                Cookie_Model cookieModel = new Cookie_Model();
                if (!string.IsNullOrWhiteSpace(srtCookie))
                {
                    cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                    ExchangeMember_Model model = new ExchangeMember_Model();

                    model.CardNumber = CardNumber;
                    model.Passeord = Passeord;
                    model.IDNumber = IDNumber;
                    model.Name = Name;
                    model.CustomerCode = cookieModel.CustomerCode;
                    model.UserID = cookieModel.UserID;
                    string postJson = JsonConvert.SerializeObject(model);
                    string data = string.Empty;
                    if(GetPostResponseNoRedirect("Member", "ExchangeMember", postJson, out data, true, false))
                    {
                        return Content(data, "application/json; charset=utf-8");
                    }
                    else
                    {
                        return Json(res);
                    }
                }
                else
                {
                    return Json(res);
                }
            }
        }
        // 我的订单_评价
        public ActionResult setComment(string OrderCode, string DoctorCode, decimal Overall, decimal Profession, decimal Altitude, int IsSolute, string Comment)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                ServiceComment_Model model = new ServiceComment_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.UserID = cookieModel.UserID;
                model.DoctorCode = DoctorCode;
                model.OrderCode = OrderCode;
                model.Overall = Overall;
                model.Profession = Profession;
                model.Altitude = Altitude;
                model.IsSolute = IsSolute;
                model.Comment = Comment;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Service", "SetComment", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 关注医生
        public ActionResult getDoctorList()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                Doctor_Model model = new Doctor_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                model.followFlg = 1;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Doctor", "GetDoctorList", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 我的咨询
        public ActionResult getAdvisoryList()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                GetCustomerCoupon_Model model = new GetCustomerCoupon_Model();

                model.CustomerCode = cookieModel.CustomerCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Advisory", "GetAdvisoryList", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 咨询详情
        public ActionResult getAdvisoryDetail(int GroupID)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                GetAdvisoryDetail_Model model = new GetAdvisoryDetail_Model();
                model.GroupID = GroupID;
                model.CustomerCode = cookieModel.CustomerCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Advisory", "GetAdvisoryDetail", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 申请售后
        public ActionResult getOrderDetail(string OrderCode)
        {
            OrderInfo_Model model = new OrderInfo_Model();
            model.OrderCode = OrderCode;
            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;
            GetPostResponseNoRedirect("Order", "GetOrderDetail", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
        // 申请售后 提交
        public ActionResult afterService(string OrderCode, string Reason)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            if (string.IsNullOrEmpty(Reason))
            {
                res.Message = "请输入申请售后的理由";
                return Json(res);
            }

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                AfterService_Model model = new AfterService_Model();
                model.OrderCode = OrderCode;
                model.Reason = Reason;
                model.UserID = cookieModel.UserID;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Service", "AfterService", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        //设置-实名认证
        public ActionResult nameAuthenticate(string Name, string IDNumber)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(IDNumber))
            {
                res.Message = "请输入姓名及身份证号";
                return Json(res);
            }

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                Authenticate_Model model = new Authenticate_Model();
                model.CustomerCode = cookieModel.CustomerCode;
                model.UserID = cookieModel.UserID;
                model.Name = Name;
                model.IDNumber = IDNumber;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Member", "NameAuthenticate", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 家庭列表
        public ActionResult getFamilyList()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                GetFamilyInfo_Model model = new GetFamilyInfo_Model();
                model.CustomerCode = cookieModel.CustomerCode;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Family", "GetFamilyList", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 地址管理
        public ActionResult getAddressList()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                GetCustomerCoupon_Model model = new GetCustomerCoupon_Model();
                model.CustomerCode = cookieModel.CustomerCode;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Address", "GetAddressList", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 保存地址
        public ActionResult saveAddress(SaveAddress_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                model.CustomerCode = cookieModel.CustomerCode;
                model.UserID = cookieModel.UserID;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Address", "SaveAddress", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 删除地址
        public ActionResult deleteAddress(SaveAddress_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                model.CustomerCode = cookieModel.CustomerCode;
                model.UserID = cookieModel.UserID;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Address", "DeleteAddress", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 省市区三级联动
        public ActionResult getDistrict(GetDistrict model)
        {
            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;
            GetPostResponseNoRedirect("Address", "GetDistrict", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }
        // 添加编辑家人
        public ActionResult changeFamily(ChangeFamily_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                model.CustomerCode = cookieModel.CustomerCode;
                model.UserID = cookieModel.UserID;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Family", "ChangeFamily", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 添加编辑家人
        public ActionResult deleteFamily(ChangeFamily_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                model.CustomerCode = cookieModel.CustomerCode;
                model.UserID = cookieModel.UserID;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Family", "DeleteFamily", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 更改登陆手机号
        public ActionResult changeMobile(ChangeMobile_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                model.UserID = cookieModel.UserID;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Member", "ChangeMobile", postJson, out data, true, false))
                {
                    if (Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["Name"].ToString() == "1")
                    {
                        CookieUtil.SetCookie("TWXOD", "", 0, true);
                    }
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        // 反馈意见
        public ActionResult submitFeedback(Feedback_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                model.UserID = cookieModel.UserID;

                List<ImageURL_Model> UploadImage = new List<ImageURL_Model>();
                //foreach (string ima in ProfileImage)
                //{
                //    ImageURL_Model imaModel = Newtonsoft.Json.JsonConvert.DeserializeObject<ImageURL_Model>(ima);
                //    UploadImage.Add(imaModel);
                //}
                //model.Image1 = UploadImage[0].ImageURL;
                //model.Image2 = UploadImage[2].ImageURL;
                //model.Image3 = UploadImage[3].ImageURL;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Feedback", "SubmitFeedback", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }
            }
            else
            {
                return Json(res);
            }
        }
        public ActionResult PayServiceOrder(AddServiceOrder_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            model.UserID = this.UserID;
            model.CustomerCode = this.CustomerCode;
            model.WechatOpenID = CookieUtil.GetCookieValue("TWXOD", true);

            string postJson = JsonConvert.SerializeObject(model);
            string data = string.Empty;

            GetPostResponseNoRedirect("Order", "PayServiceOrder", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }

        //用户信息获取
        public ActionResult getUserInfo()
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);
                Member_Model model = new Member_Model();

                model.CustomerCode = cookieModel.CustomerCode;

                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if(GetPostResponseNoRedirect("Member", "GetCustomerInfo", postJson, out data, true, false))
                {
                    cookieModel.MemberCode = Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["MemberCode"].ToString();
                    cookieModel.Level = int.Parse(Newtonsoft.Json.Linq.JObject.Parse(data)["Data"]["LevelID"].ToString());

                    CookieUtil.SetCookie("WebTouch", JsonConvert.SerializeObject(cookieModel), 0, true);
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }

            }
            else
            {
                return Json(res);
            }
        }

        public ActionResult OrderCancel(string OrderCode)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);
                OrderInfo_Model model = new OrderInfo_Model();

                model.UserID = cookieModel.UserID;
                model.OrderCode = OrderCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if (GetPostResponseNoRedirect("Order", "OrderCancel", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }

            }
            else
            {
                return Json(res);
            }
        }

        // 会员活动取得
        public ActionResult getMemberActList()
        {
            string postJson = string.Empty;
            string data = string.Empty;
            GetPostResponseNoRedirect("Member", "GetMemberActList", postJson, out data, true, false);
            return Content(data, "application/json; charset=utf-8");
        }

        public ActionResult AddAct(Act_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);

                model.UserID = cookieModel.UserID;
                model.MemberCode = cookieModel.MemberCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if (GetPostResponseNoRedirect("Member", "AddAct", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }

            }
            else
            {
                return Json(res);
            }
        }
        public ActionResult MemberActCount(Act_Model model)
        {
            ObjectResult<bool> res = new ObjectResult<bool>();
            res.Code = "0";
            res.Message = "操作失败!";
            res.Data = false;

            string srtCookie = CookieUtil.GetCookieValue("WebTouch", true);
            //string srtCookie = "{\"UserID\":2,\"Level\":1,\"UserName\":\"test2\",\"CustomerCode\":\"C201810080000002\",\"MemberCode\":\"M201810100000002\",\"IsSigned\":true}";

            Cookie_Model cookieModel = new Cookie_Model();
            if (!string.IsNullOrWhiteSpace(srtCookie))
            {
                cookieModel = JsonConvert.DeserializeObject<Cookie_Model>(srtCookie);
                
                model.MemberCode = cookieModel.MemberCode;
                string postJson = JsonConvert.SerializeObject(model);
                string data = string.Empty;
                if (GetPostResponseNoRedirect("Member", "MemberActCount", postJson, out data, true, false))
                {
                    return Content(data, "application/json; charset=utf-8");
                }
                else
                {
                    return Json(res);
                }

            }
            else
            {
                return Json(res);
            }
        }
    }
}