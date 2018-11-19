using Common.Safe;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebManager.Model;
using BLL;
using Common.Util;
using Common.Entity;
using Model.Table_Model;

namespace WebManager.Controllers
{
    public class SettingController : BaseController
    {
        // GET: Setting

        #region hospital
        public ActionResult HospitalList()
        {
            HospitalList_Model result = new HospitalList_Model();
            result.HospitalName = QueryString.SafeQ("hn");
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Hospital_Model> HospitalList = new List<Hospital_Model>();

            HospitalList = HospitalM_BLL.Instance.getHospitalList(result.HospitalName);
            result.TotalCount = HospitalM_BLL.Instance.getHospitalList(result.HospitalName).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Hospital_Model>();
            result.Data = HospitalList;
            return View(result);
        }

        public ActionResult HospitalDetail()
        {
            HospitalDetail_Model result = new HospitalDetail_Model();
            result.HospitalID = QueryString.IntSafeQ("id", 0);

            result.Data = new Hospital_Model();
            if (result.HospitalID > 0)
            {
                result.Data = HospitalM_BLL.Instance.getHospitalDetail(result.HospitalID);
            }
            return View(result);
        }

        public ActionResult OperateHospital(Hospital_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (model.HospitalID == 0)
            {
                model.Creator = this.UserID;
                model.CreatetTime = DateTime.Now.ToLocalTime();
                sqlResult = HospitalM_BLL.Instance.addHospital(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = HospitalM_BLL.Instance.updateHospital(model);
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
        public ActionResult DeleteHospital(Hospital_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = HospitalM_BLL.Instance.deleteHospital(model);
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

        #region Department
        public ActionResult DepartmentList()
        {
            DepartmentList_Model result = new DepartmentList_Model();
            result.DepartmentName = QueryString.SafeQ("dn");
            result.UpperID = QueryString.IntSafeQ("u", 0);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Department_Model> DepartmentList = new List<Department_Model>();

            DepartmentList = DepartmentM_BLL.Instance.getDepartmentList(result.DepartmentName, result.UpperID, StartCount, EndCount);
            result.TotalCount = DepartmentM_BLL.Instance.getDepartmentList(result.DepartmentName, result.UpperID).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Department_Model>();
            result.Data = DepartmentList;
            return View(result);
        }

        public ActionResult DepartmentDetail()
        {
            DepartmentDetail_Model result = new DepartmentDetail_Model();
            result.DepartmentID = QueryString.IntSafeQ("id", 0);


            result.Data = new Department_Model();
            if (result.DepartmentID > 0)
            {
                result.Data = DepartmentM_BLL.Instance.getDepartmentDetail(result.DepartmentID);
            }

            result.UpperList = new List<Department_Model>();
            result.UpperList = DepartmentM_BLL.Instance.getDepartmentList("", 0);


            return View(result);
        }

        public ActionResult OperateDepartment(Department_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (model.DepartmentID == 0)
            {
                model.CreatetTime = DateTime.Now.ToLocalTime();
                model.Creator = this.UserID;
                sqlResult = DepartmentM_BLL.Instance.addDepartment(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = DepartmentM_BLL.Instance.updateDepartment(model);
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
        public ActionResult DeleteDepartment(Department_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = DepartmentM_BLL.Instance.deleteDepartment(model);
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

        #region Title
        public ActionResult TitleList()
        {
            TitleList_Model result = new TitleList_Model();
            result.TitleName = QueryString.SafeQ("tn");
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Title_Model> TitleList = new List<Title_Model>();

            TitleList = TitleM_BLL.Instance.getTitleList(result.TitleName,StartCount,EndCount);
            result.TotalCount = TitleM_BLL.Instance.getTitleList(result.TitleName).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Title_Model>();
            result.Data = TitleList;
            return View(result);
        }

        public ActionResult TitleDetail()
        {
            TitleDetail_Model result = new TitleDetail_Model();
            result.TitleID = QueryString.IntSafeQ("id", 0);

            result.Data = new Title_Model();
            if (result.TitleID > 0)
            {
                result.Data = TitleM_BLL.Instance.getTitleDetail(result.TitleID);
            }
            return View(result);
        }

        public ActionResult OperateTitle(Title_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (model.TitleID == 0)
            {
                model.Creator = this.UserID;
                model.CreatetTime = DateTime.Now.ToLocalTime();
                sqlResult = TitleM_BLL.Instance.addTitle(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = TitleM_BLL.Instance.updateTitle(model);
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
        public ActionResult DeleteTitle(Title_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = TitleM_BLL.Instance.deleteTitle(model);
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


        #region Tag

        public ActionResult TagList()
        {
            TagList_Model result = new TagList_Model();
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Tag_Model> TagList = new List<Tag_Model>();

            TagList = TagM_BLL.Instance.getTagList( StartCount, EndCount);
            result.TotalCount = TagM_BLL.Instance.getTagList().Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Tag_Model>();
            result.Data = TagList;
            return View(result);
        }

        public ActionResult TagDetail()
        {
            TagDetail_Model result = new TagDetail_Model();
            result.TagID = QueryString.IntSafeQ("id", 0);

            result.Data = new Tag_Model();
            if (result.TagID > 0)
            {
                result.Data = TagM_BLL.Instance.getTagDetail(result.TagID);
            }
            return View(result);
        }

        public ActionResult OperateTag(Tag_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (model.TagID == 0)
            {
                model.Creator = this.UserID;
                model.CreatetTime = DateTime.Now.ToLocalTime();
                sqlResult = TagM_BLL.Instance.addTag(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = TagM_BLL.Instance.updateTag(model);
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
        public ActionResult DeleteTag(Tag_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = TagM_BLL.Instance.deleteTag(model);
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


        #region Channel

        public ActionResult ChannelList()
        {
            ChannelList_Model result = new ChannelList_Model();
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Channel_Model> ChannelList = new List<Channel_Model>();

            ChannelList = ChannelM_BLL.Instance.getChannelList(StartCount, EndCount);
            result.TotalCount = ChannelM_BLL.Instance.getChannelList().Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Channel_Model>();
            result.Data = ChannelList;
            return View(result);
        }

        public ActionResult ChannelDetail()
        {
            ChannelDetail_Model result = new ChannelDetail_Model();
            result.ChannelID = QueryString.IntSafeQ("id", 0);

            result.Data = new Channel_Model();
            if (result.ChannelID > 0)
            {
                result.Data = ChannelM_BLL.Instance.getChannelDetail(result.ChannelID);
            }
            return View(result);
        }

        public ActionResult OperateChannel(Channel_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (model.ChannelID == 0)
            {
                model.Creator = this.UserID;
                model.CreatetTime = DateTime.Now.ToLocalTime();
                sqlResult = ChannelM_BLL.Instance.addChannel(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = ChannelM_BLL.Instance.updateChannel(model);
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
        public ActionResult DeleteChannel(Channel_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = ChannelM_BLL.Instance.deleteChannel(model);
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


        #region Banner

        public ActionResult BannerList()
        {
            BannerList_Model result = new BannerList_Model();
            result.Status = QueryString.IntSafeQ("s", 0);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Banner_Model> BannerList = new List<Banner_Model>();

            BannerList = BannerM_BLL.Instance.getBannerList(result.Status,StartCount, EndCount);
            result.TotalCount = BannerM_BLL.Instance.getBannerList(result.Status).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Banner_Model>();
            result.Data = BannerList;
            return View(result);
        }

        public ActionResult BannerDetail()
        {
            BannerDetail_Model result = new BannerDetail_Model();
            result.ID = QueryString.IntSafeQ("id", 0);

            result.Data = new Banner_Model();
            if (result.ID > 0)
            {
                result.Data = BannerM_BLL.Instance.getBannerDetail(result.ID);
            }
            return View(result);
        }

        public ActionResult OperateBanner(Banner_Model model)
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
                sqlResult = BannerM_BLL.Instance.addBanner(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = BannerM_BLL.Instance.updateBanner(model);
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
        public ActionResult DeleteBanner(Banner_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = BannerM_BLL.Instance.deleteBanner(model);
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




        public ActionResult UpdateSort(Banner_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = BannerM_BLL.Instance.UpdateSort(model.ID,model.Sort);
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


        public ActionResult GetPoint()
        {
            SetGetpoint_Model result = new SetGetpoint_Model();

            result = SettingM_BLL.Instance.getSetPoint();

            return View(result);
        }

        public ActionResult OperateGetPoint(OperatePoint_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = SettingM_BLL.Instance.UpdateSetPoint(model.key, model.value);
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



        #region MemberAct

        public ActionResult MemberActList()
        {
            MemberActList_Model result = new MemberActList_Model();
            result.Status = QueryString.IntSafeQ("s", 0);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<MemberAct_Model> MemberActList = new List<MemberAct_Model>();

            MemberActList = MemberActM_BLL.Instance.getMemberActList(result.Status, StartCount, EndCount);
            result.TotalCount = MemberActM_BLL.Instance.getMemberActList(result.Status).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<MemberAct_Model>();
            result.Data = MemberActList;
            return View(result);
        }

        public ActionResult MemberActDetail()
        {
            MemberActDetail_Model result = new MemberActDetail_Model();
            result.ID = QueryString.IntSafeQ("id", 0);

            result.Data = new MemberAct_Model();
            if (result.ID > 0)
            {
                result.Data = MemberActM_BLL.Instance.getMemberActDetail(result.ID);
            }

            result.listLevel = new List<Level_Model>();
            result.listLevel = LevelM_BLL.Instance.getLevelList(1);
            return View(result);
        }

        public ActionResult OperateMemberAct(MemberAct_Model model)
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
                sqlResult = MemberActM_BLL.Instance.addMemberAct(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = MemberActM_BLL.Instance.updateMemberAct(model);
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
        public ActionResult DeleteMemberAct(MemberAct_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            model.UpdateTime = DateTime.Now.ToLocalTime();
            model.Updater = this.UserID;
            int sqlResult = MemberActM_BLL.Instance.deleteMemberAct(model);
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


        public ActionResult MemberActInfo()
        {
            MemberActInfoView_Model result = new MemberActInfoView_Model();
            result.ID = QueryString.IntSafeQ("id", 0);
            result.HandleSts = QueryString.IntSafeQ("s", 0);

            result.Data = new List<MemberActInfo_Model>();
            if (result.ID > 0)
            {
                result.Data = MemberActM_BLL.Instance.getMemberInfoList(result.ID,result.HandleSts);
            }

            return View(result);
        }


        public ActionResult updateHandleSts(MemberActInfo_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            model.UpdateTime = DateTime.Now.ToLocalTime();
            model.Updater = this.UserID;
            int sqlResult = MemberActM_BLL.Instance.updateHandleSts(model);
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


        public ActionResult DownloadMemberActInfo(MemberActInfo_Model model)
        {
            ObjectResult<string> result = new ObjectResult<string>();
            result.Code = "0";
            result.Data = null;
            result.Message = "系统错误";


            MemberAct_Model memberAct = MemberActM_BLL.Instance.getMemberActDetail(model.ID);
           
            if (memberAct != null)
            {
                List<MemberActInfo_Model> listInfo = MemberActM_BLL.Instance.getMemberInfoList(model.ID, model.HandleSts);

                string url = MemberActM_BLL.Instance.ExportMemberActInfo(memberAct,listInfo);
                result.Code = "1";
                result.Data = url;
                result.Message = "操作成功";
            }

            return Json(result);

        }

        #endregion


    }
}