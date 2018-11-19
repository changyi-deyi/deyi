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
    public class DoctorController : BaseController
    {
        // GET: Doctor
        public ActionResult DoctorList()
        {
            DoctorList_Model result = new DoctorList_Model();
            result.DoctorName = QueryString.SafeQ("dn");
            result.HostpitalID = QueryString.IntSafeQ("hId",0);
            result.DepartmentID = QueryString.IntSafeQ("dId",0);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Doctor_Model> DoctorList = new List<Doctor_Model>();

            DoctorList = UserM_BLL.Instance.getDoctorList(result.DoctorName,result.HostpitalID,result.DepartmentID,StartCount,EndCount);
            result.TotalCount = UserM_BLL.Instance.getDoctorList(result.DoctorName, result.HostpitalID, result.DepartmentID).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Doctor_Model>();
            result.Data = DoctorList;

            result.listHospital = new List<Hospital_Model>();
            result.listHospital = HospitalM_BLL.Instance.getHospitalList("");

            result.listDepartment = new List<Department_Model>();
            result.listDepartment = DepartmentM_BLL.Instance.getDepartmentList(null, 0);
            

            return View(result);
        }

        // GET: Doctor
        public ActionResult DoctorDetail()
        {
            DoctorDetail_Model result = new DoctorDetail_Model();
            result.DoctorCode = QueryString.SafeQ("cd");

            result.Data = new Doctor_Model();

            if (!string.IsNullOrEmpty(result.DoctorCode)) {
                result.Data = UserM_BLL.Instance.getDoctorDetail(result.DoctorCode);
            }

            result.listHospital = new List<Hospital_Model>();
            result.listHospital = HospitalM_BLL.Instance.getHospitalList("");

            result.listDepartment = new List<Department_Model>();
            result.listDepartment = DepartmentM_BLL.Instance.getDepartmentList(null, 0);

            result.listTitle = new List<Title_Model>();
            result.listTitle = TitleM_BLL.Instance.getTitleList("");

            result.listTag = new List<Tag_Model>();
            result.listTag = TagM_BLL.Instance.getDoctorTagList(result.DoctorCode);

            

            return View(result);
        }


        public ActionResult OperateDoctor(UserOperate_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (string.IsNullOrEmpty(model.UserCode))
            {
                model.User.Password = CryptMD5.Encrypt(System.Configuration.ConfigurationManager.AppSettings["DefaultPassWord"]);
                model.Doctor.CreatetTime = DateTime.Now.ToLocalTime();
                model.Doctor.Creator = this.UserID;
                model.User.CreatetTime = DateTime.Now.ToLocalTime();
                model.User.Creator = this.UserID;
                sqlResult = UserM_BLL.Instance.addUser(model);
            }
            else
            {
                model.Doctor.UpdateTime = DateTime.Now.ToLocalTime();
                model.Doctor.Updater = this.UserID;
                sqlResult = UserM_BLL.Instance.updateDoctor(model.Doctor);
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


        public ActionResult DeleteDoctor(UserDeleteOperate_Model model)
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