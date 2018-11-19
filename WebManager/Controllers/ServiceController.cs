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
    public class ServiceController : BaseController
    {

        #region Service

        public ActionResult ServiceList()
        {
            ServiceList_Model result = new ServiceList_Model();
            result.Status = QueryString.IntSafeQ("s", 1);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Service_Model> ServiceList = new List<Service_Model>();

            ServiceList = ServiceM_BLL.Instance.getServiceList(result.Status, StartCount, EndCount);
            result.TotalCount = ServiceM_BLL.Instance.getServiceList(result.Status).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Service_Model>();
            result.Data = ServiceList;
            return View(result);
        }

        public ActionResult ServiceDetail()
        {
            ServiceDetail_Model result = new ServiceDetail_Model();
            result.ServiceCode = QueryString.SafeQ("cd");

            result.Data = new Service_Model();
            if (!string.IsNullOrEmpty(result.ServiceCode))
            {
                result.Data = ServiceM_BLL.Instance.getServiceDetail(result.ServiceCode);
            }
            return View(result);
        }

        public ActionResult OperateService(Service_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (string.IsNullOrEmpty(model.ServiceCode))
            {
                model.Creator = this.UserID;
                model.CreatetTime = DateTime.Now.ToLocalTime();
                sqlResult = ServiceM_BLL.Instance.addService(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = ServiceM_BLL.Instance.updateService(model);
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
        public ActionResult DeleteService(Service_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = ServiceM_BLL.Instance.deleteService(model);
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
        #endregion

        public ActionResult ServiceDoctorList()
        {
            ServiceDoctorList_Model result = new ServiceDoctorList_Model();
            result.ServiceCode = QueryString.SafeQ("cd");
            result.Name = QueryString.SafeQ("sn");

            result.Data = new List<ServiceDoctor_Model>();
            result.Data = ServiceM_BLL.Instance.getServiceDoctorList(result.ServiceCode);
            if (result.Data != null && result.Data.Count > 0) {
                result.IsBargain = result.Data.Max(x => x.IsBargain);
            }

            if (result.IsBargain == 0) {
                result.IsBargain = 2;
            }
            return View(result);
        }



        public ActionResult OperateServiceDoctor(ServiceDoctorList_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            if (model == null || model.Data == null) {
                return Json(result);
            }
            int sqlResult = ServiceM_BLL.Instance.addServiceDoctor(model.ServiceCode, model.Data, this.UserID);

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




        public ActionResult MemberServiceList()
        {
            MemberServiceList_Model result = new MemberServiceList_Model();
            result.ServiceCode = QueryString.SafeQ("cd");
            result.Name = QueryString.SafeQ("sn");

            result.Data = new List<MemberService_Model>();
            result.Data = ServiceM_BLL.Instance.getMemberServiceList(result.ServiceCode);
            return View(result);
        }



        public ActionResult OperateMemberService(MemberService_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            int sqlResult = 0;
            if (model.ID == 0)
            {
                model.Creator = this.UserID;
                model.CreatetTime = DateTime.Now.ToLocalTime();
                sqlResult = ServiceM_BLL.Instance.addMemberService(model);
            }
            else {

                model.Updater = this.UserID;
                model.UpdateTime = DateTime.Now.ToLocalTime();
                sqlResult = ServiceM_BLL.Instance.UpdatedMemberService(model);
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


        public ActionResult ServiceImage()
        {
            ServiceImage_Model result = new ServiceImage_Model();
            result.ServiceCode = QueryString.SafeQ("cd");
            result.Name = QueryString.SafeQ("sn");

            result.Data = new List<ServiceImg_Model>();
            result.Data = ServiceM_BLL.Instance.getServiceImg(result.ServiceCode);
            return View(result);
        }


        public ActionResult addServiceImg(ServiceImg_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            model.Creator = this.UserID;
            model.CreatetTime = DateTime.Now.ToLocalTime();
            int sqlResult = ServiceM_BLL.Instance.addServiceImg(model);

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


        public ActionResult deleteServiceImg(ServiceImg_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            model.Updater = this.UserID;
            model.UpdateTime = DateTime.Now.ToLocalTime();
            int sqlResult = ServiceM_BLL.Instance.DeleteServiceImg(model);

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



        public ActionResult UpdateSort(Service_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = ServiceM_BLL.Instance.UpdateSort(model.ServiceCode,model.Sort);
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