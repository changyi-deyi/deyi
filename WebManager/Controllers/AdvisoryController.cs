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
    public class AdvisoryController : BaseController
    {
        // GET: Advisory
        public ActionResult AdvisoryList()
        {
            AdvisoryList_Model result = new AdvisoryList_Model();
            result.CustomerName = QueryString.SafeQ("cn");
            result.CustomerCode = QueryString.SafeQ("cd");
            result.IsDone = QueryString.IntSafeQ("d", 0);
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Advisory_Model> AdvisoryList = new List<Advisory_Model>();

            AdvisoryList = AdvisoryM_BLL.Instance.getAdvisoryList(result.CustomerCode, result.CustomerName, result.IsDone, StartCount, EndCount);
            result.TotalCount = AdvisoryM_BLL.Instance.getAdvisoryList(result.CustomerCode, result.CustomerName, result.IsDone).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Advisory_Model>();
            result.Data = AdvisoryList;


            return View(result);
        }
        public ActionResult AdvisoryDetail()
        {
            AdvisoryDetail_Model result = new AdvisoryDetail_Model();
            result.ID = QueryString.IntSafeQ("id", 0);

            result.Data = new List<Advisory_Model>();
            if (result.ID > 0)
            {
                result.Data = AdvisoryM_BLL.Instance.getGroupAdvisoryList(result.ID);
            }
            return View(result);
        }


        public ActionResult AnswerAdvisory(Advisory_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";


            model.Creator = this.UserID;
            model.CreatetTime = DateTime.Now.ToLocalTime();
            model.OpCode = this.UserCode;
            int sqlResult = AdvisoryM_BLL.Instance.AnswerAdvisory(model);


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