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
    public class LevelController : BaseController
    {

        #region Level

        public ActionResult LevelList()
        {
            LevelList_Model result = new LevelList_Model();
            result.Status = QueryString.IntSafeQ("s", 0);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Level_Model> LevelList = new List<Level_Model>();

            LevelList = LevelM_BLL.Instance.getLevelList(result.Status, StartCount, EndCount);
            result.TotalCount = LevelM_BLL.Instance.getLevelList(result.Status).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Level_Model>();
            result.Data = LevelList;
            return View(result);
        }

        public ActionResult LevelDetail()
        {
            LevelDetail_Model result = new LevelDetail_Model();
            result.LevelID = QueryString.IntSafeQ("id", 0);

            result.Data = new Level_Model();
            if (result.LevelID > 0)
            {
                result.Data = LevelM_BLL.Instance.getLevelDetail(result.LevelID);
            }
            return View(result);
        }

        public ActionResult OperateLevel(Level_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = 0;
            if (model.LevelID == 0)
            {
                model.Creator = this.UserID;
                model.CreatetTime = DateTime.Now.ToLocalTime();
                sqlResult = LevelM_BLL.Instance.addLevel(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = LevelM_BLL.Instance.updateLevel(model);
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
        public ActionResult DeleteLevel(Level_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = LevelM_BLL.Instance.deleteLevel(model);
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
    }
}