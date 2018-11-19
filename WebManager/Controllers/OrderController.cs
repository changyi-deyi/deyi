using BLL;
using Common.Entity;
using Common.Safe;
using Common.Util;
using Common.WeChat;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebManager.Model;

namespace WebManager.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Order
        public ActionResult MemberOrderList()
        {
            MemberOrderList_Model result = new MemberOrderList_Model();
            result.CustomerName = QueryString.SafeQ("cn");
            result.CustomerCode = QueryString.SafeQ("cd");
            result.StartDate = QueryString.SafeQ("st");
            result.EndDate = QueryString.SafeQ("et");
            result.PaymentStatus = QueryString.IntSafeQ("ps",0);
            result.OrderStatus = QueryString.IntSafeQ("os", 0);

            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;


            List<MemberOrder_Model> MemberOrderList = new List<MemberOrder_Model>();

            MemberOrderList = OrderM_BLL.Instance.getMemberOrderList(result.CustomerCode, result.CustomerName, result.StartDate, result.EndDate,result.PaymentStatus ,result.OrderStatus,StartCount,EndCount);

            result.TotalCount = OrderM_BLL.Instance.getMemberOrderList("", result.CustomerName, result.StartDate, result.EndDate, result.PaymentStatus, result.OrderStatus).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<MemberOrder_Model>();
            result.Data = MemberOrderList;
            return View(result);
        }
        public ActionResult MemberOrderDetail()
        {
            MemberOrderDetail_Model result = new MemberOrderDetail_Model();
            result.OrderCode = QueryString.SafeQ("cd");

            result.Data = new MemberOrder_Model();

            result.Data = OrderM_BLL.Instance.getMemberOrderDetail(result.OrderCode);
            return View(result);
        }



        public ActionResult CancelMemberOrder(MemberOrder_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            model.Updater = this.UserID;
            model.UpdateTime = DateTime.Now.ToLocalTime();
            int sqlResult = OrderM_BLL.Instance.CancelMemberOrder(model);


            if (sqlResult == 1)
            {
                result.Code = "1";
                result.Data = true;
                result.Message = "操作成功";
            }
            else if (sqlResult == 0)
            {
                result.Message = "操作失败";
            }

            return Json(result);

        }


        public ActionResult ServiceOrderList()
        {
            ServiceOrderList_Model result = new ServiceOrderList_Model();
            result.CustomerName = QueryString.SafeQ("cn");
            result.DoctorName = QueryString.SafeQ("dn");
            result.ServiceName = QueryString.SafeQ("sn");
            result.StartDate = QueryString.SafeQ("st");
            result.EndDate = QueryString.SafeQ("et");
            result.PaymentStatus = QueryString.IntSafeQ("ps", 0);
            result.OrderStatus = QueryString.IntSafeQ("os", 0);
            result.ServiceStatus = QueryString.IntSafeQ("ss", 0);

            result.CustomerCode = QueryString.SafeQ("cd");


            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            
            List<ServiceOrder_Model> ServiceOrderList = new List<ServiceOrder_Model>();
            ServiceOrderList = OrderM_BLL.Instance.getServiceOrderList(result.CustomerName,result.DoctorName,result.ServiceName,result.PaymentStatus,result.ServiceStatus,result.OrderStatus,result.StartDate,result.EndDate, result.CustomerCode, StartCount, EndCount);
            result.TotalCount = OrderM_BLL.Instance.getServiceOrderList(result.CustomerName, result.DoctorName, result.ServiceName, result.PaymentStatus, result.ServiceStatus, result.OrderStatus, result.StartDate, result.EndDate, result.CustomerCode).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<ServiceOrder_Model>();
            result.Data = ServiceOrderList;
            return View(result);
        }


        public ActionResult ServiceOrderDetail()
        {
            ServiceOrderDetail_Model result = new ServiceOrderDetail_Model();
            result.OrderCode = QueryString.SafeQ("cd");
            result.Data = OrderM_BLL.Instance.getServiceOrderDetail(result.OrderCode);
            result.DoctorList = UserM_BLL.Instance.getDoctorList(null, 0, 0);
            result.StaffList = UserM_BLL.Instance.getStaffList(null, -1);

            return View(result);
        }


        public ActionResult ReplyServiceOrder(ServiceOrder_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            Random random = new Random(Guid.NewGuid().GetHashCode());
            string randomNumber = "";

            for (int i = 0; i < 6; i++)
            {
                randomNumber += random.Next(0, 9).ToString();
            }

            model.SafeCode = randomNumber;

            model.Updater = this.UserID;
            model.UpdateTime = DateTime.Now.ToLocalTime();
            int sqlResult = OrderM_BLL.Instance.ReplyServiceOrder(model);


            if (sqlResult == 1)
            {
                SendCustomer_Model customer = new SendCustomer_Model();
                customer.customer = model.Name;
                customer.hospital = model.HospitalName;
                customer.department = model.DepartmentName;
                customer.doctor = model.DoctorName;
                customer.datetime =((DateTime) model.ArrangedTime).ToString();
                customer.address = model.Address;
                customer.code = model.SafeCode;

                if (!SendMessage.SendCustomer(model.Mobile,customer))
                {
                }

                SendDoctor_Model doctor = new SendDoctor_Model();
                doctor.doctor = model.DoctorName;
                doctor.customer = model.Name;
                doctor.datetime = ((DateTime)model.ArrangedTime).ToString();
                doctor.address = model.Address;
                doctor.code = model.SafeCode;


                if (!SendMessage.SendDoctor(model.DoctorMobile, doctor))
                {
                }
                result.Code = "1";
                result.Data = true;
                result.Message = "操作成功";
            }
            else if (sqlResult == 0)
            {
                result.Message = "操作失败";
            }

            return Json(result);

        }


        public ActionResult UpdateServiceOrderAmount(ServiceOrder_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            model.Updater = this.UserID;
            model.UpdateTime = DateTime.Now.ToLocalTime();
            int sqlResult = OrderM_BLL.Instance.UpdateServiceOrderAmount(model);


            if (sqlResult == 1)
            {
                result.Code = "1";
                result.Data = true;
                result.Message = "操作成功";
            }
            else if (sqlResult == 0)
            {
                result.Message = "操作失败";
            }

            return Json(result);

        }


        public ActionResult UpdateServiceOrderStatus(ServiceOrder_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            model.Updater = this.UserID;
            model.UpdateTime = DateTime.Now.ToLocalTime();
            int sqlResult = OrderM_BLL.Instance.UpdateServiceOrderStatus(model);


            if (sqlResult == 1)
            {
                result.Code = "1";
                result.Data = true;
                result.Message = "操作成功";
            }
            else if (sqlResult == 0)
            {
                result.Message = "操作失败";
            }

            return Json(result);

        }


        public ActionResult QueryPayResult(MemberOrder_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = true;
            result.Message = "刷新成功";


            List<string> listNetTradeCode = OrderM_BLL.Instance.getNoResultNetTradeNoByOrder(model.OrderCode);

            if (listNetTradeCode != null && listNetTradeCode.Count > 0) {
                WeChatPay we = new WeChatPay();
                foreach (string item in listNetTradeCode)
                {
                    string data = we.QueryPaymentByID("", item);

                    if (string.IsNullOrEmpty(data))
                    {
                        continue;
                    }


                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(data);
                    XmlNode root = doc.FirstChild;

                    if (root == null)
                    {
                        continue;
                    }


                    if (root["return_code"].InnerText.ToString() != "SUCCESS"
                                   || root["result_code"].InnerText.ToString() != "SUCCESS"
                                   || root["trade_state"].InnerText.ToString() != "SUCCESS")
                    {
                        continue;
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
                        continue;
                    }
                    int SqlResult = InfMember_BLL.Instance.UpdatePayMemberOrderResult(weChatModel.out_trade_no, data, 1);
                }
               
            }

           

            return Json(result);

        }


        public ActionResult QueryServicePayResult(ServiceOrder_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = true;
            result.Message = "刷新成功";

            List<string> listNetTradeCode = OrderM_BLL.Instance.getNoResultNetTradeNoByOrder(model.OrderCode);

            if (listNetTradeCode != null && listNetTradeCode.Count > 0)
            {
                WeChatPay we = new WeChatPay();
                foreach (string item in listNetTradeCode)
                {
                    string data = we.QueryPaymentByID("", item);

                    if (string.IsNullOrEmpty(data))
                    {
                        continue;
                    }


                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(data);
                    XmlNode root = doc.FirstChild;

                    if (root == null)
                    {
                        continue;
                    }


                    if (root["return_code"].InnerText.ToString() != "SUCCESS"
                                   || root["result_code"].InnerText.ToString() != "SUCCESS"
                                   || root["trade_state"].InnerText.ToString() != "SUCCESS")
                    {
                        continue;
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
                        continue;
                    }
                    int SqlResult = OpeServiceOrder_BLL.Instance.UpdatePayServiceOrderResult(weChatModel.out_trade_no, data, 1);

                }
            }

            return Json(result);
        }

    }
}