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
    public class StaffController : BaseController
    {
        // GET: Staff
        public ActionResult StaffList()
        {
            StaffList_Model result = new StaffList_Model();
            result.StaffName = QueryString.SafeQ("sn");
            result.Role = QueryString.IntSafeQ("r", 0);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Staff_Model> StaffList = new List<Staff_Model>();

            StaffList = UserM_BLL.Instance.getStaffList(result.StaffName, result.Role, StartCount, EndCount);
            result.TotalCount = UserM_BLL.Instance.getStaffList(result.StaffName, result.Role).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Staff_Model>();
            result.Data = StaffList;
            return View(result);
        }


        // GET: Doctor
        public ActionResult StaffDetail()
        {
            StaffDetail_Model result = new StaffDetail_Model();
            result.StaffCode = QueryString.SafeQ("cd");

            result.Data = new Staff_Model();

            if (!string.IsNullOrEmpty(result.StaffCode))
            {
                result.Data = UserM_BLL.Instance.getStaffDetail(result.StaffCode);
            }
            

            return View(result);
        }


        public ActionResult OperateStaff(UserOperate_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (string.IsNullOrEmpty(model.UserCode))
            {
                model.User.Password = CryptMD5.Encrypt(System.Configuration.ConfigurationManager.AppSettings["DefaultPassWord"]);
                model.Staff.CreatetTime = DateTime.Now.ToLocalTime();
                model.Staff.Creator = this.UserID;
                model.User.CreatetTime = DateTime.Now.ToLocalTime();
                model.User.Creator = this.UserID;
                sqlResult = UserM_BLL.Instance.addUser(model);
            }
            else
            {
                model.Staff.UpdateTime = DateTime.Now.ToLocalTime();
                model.Staff.Updater = this.UserID;
                sqlResult = UserM_BLL.Instance.updateStaff(model.Staff);
            }

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



        public ActionResult DeleteStaff(UserDeleteOperate_Model model)
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

    }
}