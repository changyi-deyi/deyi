using BLL;
using Common.Entity;
using Common.Safe;
using Common.Util;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebManager.Model;

namespace WebManager.Controllers
{
    public class CustomerController : BaseController
    {
        // GET: Customer
        public ActionResult CustomerList()
        {
            CustomerList_Model result = new CustomerList_Model();
            result.CustomerName = QueryString.SafeQ("cn");
            result.LevelID = QueryString.IntSafeQ("l", 0);
            result.ChannelID = QueryString.IntSafeQ("c", 0);
            result.Status = QueryString.IntSafeQ("s", 1);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Customer_Model> CustomerList = new List<Customer_Model>();

            CustomerList = UserM_BLL.Instance.getCustomerList(result.CustomerName, result.LevelID, result.ChannelID, result.Status, StartCount, EndCount);
            result.TotalCount = UserM_BLL.Instance.getCustomerList(result.CustomerName, result.LevelID, result.ChannelID, result.Status).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Customer_Model>();
            result.Data = CustomerList;

            result.listLevel = new List<Level_Model>();
            result.listLevel = LevelM_BLL.Instance.getLevelList(0);

            result.listChannel = new List<Channel_Model>();
            result.listChannel = ChannelM_BLL.Instance.getChannelList();

            return View(result);
        }

        public ActionResult CustomerDetail()
        {

            CustomerDetail_Model result = new CustomerDetail_Model();
            result.CustomerCode = QueryString.SafeQ("cd");
            if (!string.IsNullOrEmpty(result.CustomerCode))
            {
                result.Customer = new Customer_Model();
                result.Customer = UserM_BLL.Instance.getCustomerDetail(result.CustomerCode);

                if (result.Customer != null && !string.IsNullOrEmpty(result.Customer.MemberCode))
                {
                    result.Member = new Member_Model();
                    result.Member = UserM_BLL.Instance.getMemberDetail(result.Customer.MemberCode);
                }
            }
            return View(result);
        }


        public ActionResult BalanceList()
        {
            BalanceList_Model result = new BalanceList_Model();
            result.CustomerCode = QueryString.SafeQ("cd");
            result.StartDate = QueryString.SafeQ("st");
            result.EndDate = QueryString.SafeQ("et");
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;
            if (!string.IsNullOrEmpty(result.CustomerCode))
            {
                List<Balance_Model> balance = new List<Balance_Model>();
                balance = UserM_BLL.Instance.getBalanceList(result.CustomerCode, result.StartDate, result.EndDate, StartCount, EndCount);

                result.TotalCount = UserM_BLL.Instance.getBalanceList(result.CustomerCode, result.StartDate, result.EndDate).Count;
                result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
                result.Data = new List<Balance_Model>();
                result.Data = balance;
            }
            return View(result);
        }

        public ActionResult DeleteCustomer(UserDeleteOperate_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            model.Updater = this.UserID;
            model.UpdateTime = DateTime.Now.ToLocalTime();
            int sqlResult = UserM_BLL.Instance.deleteUser(model);


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


        public ActionResult Document()
        {
            CustomerProfileList_Model result = new CustomerProfileList_Model();
            result.CustomerCode = QueryString.SafeQ("cd");
            result.VerifyStatus = QueryString.IntSafeQ("s");
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 99999999 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;
            if (!string.IsNullOrEmpty(result.CustomerCode))
            {
                List<CustomerProfile_Model> customer = new List<CustomerProfile_Model>();
                customer = UserM_BLL.Instance.getCustomerProfile(result.CustomerCode, result.VerifyStatus, StartCount, EndCount);

                result.TotalCount = UserM_BLL.Instance.getCustomerProfile(result.CustomerCode, result.VerifyStatus).Count;
                result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
                result.Data = new List<CustomerProfile_Model>();
                result.Data = customer;

                if (customer != null) {
                    result.listImg = new List<CustomerProfileImg_Model>();
                    result.listImg = UserM_BLL.Instance.getCustomerProfileImg(result.CustomerCode, result.VerifyStatus);
                }
            }
            return View(result);

        }


        public ActionResult changeVerifyStatus(CustomerProfile_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            model.Updater = this.UserID;
            model.UpdateTime = DateTime.Now.ToLocalTime();
            int sqlResult = UserM_BLL.Instance.changeVerifyStatus(model);


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

    }
}