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
    public class CouponController : BaseController
    {

        #region Coupon

        public ActionResult CouponList()
        {
            CouponList_Model result = new CouponList_Model();
            result.Type = QueryString.IntSafeQ("t", 0);
            result.Status = QueryString.IntSafeQ("s", 1);
            result.Name = QueryString.SafeQ("cn");
            result.RowsCount = QueryString.IntSafeQ("rc") == 0 ? 10 : QueryString.IntSafeQ("rc");
            result.PageCount = QueryString.IntSafeQ("pc") == 0 ? 1 : QueryString.IntSafeQ("pc");

            int StartCount = result.RowsCount * (result.PageCount - 1);
            int EndCount = result.RowsCount * result.PageCount;

            List<Coupon_Model> CouponList = new List<Coupon_Model>();

            CouponList = CouponM_BLL.Instance.getCouponList(result.Type, result.Status, result.Name, StartCount, EndCount);
            result.TotalCount = CouponM_BLL.Instance.getCouponList(result.Type, result.Status,result.Name).Count;
            result.TotalPage = StringUtils.GetDbInt(Math.Ceiling(StringUtils.GetDbDouble(result.TotalCount) / StringUtils.GetDbDouble(result.RowsCount)));
            result.Data = new List<Coupon_Model>();
            result.Data = CouponList;
            return View(result);
        }

        public ActionResult CouponDetail()
        {
            CouponDetail_Model result = new CouponDetail_Model();
            result.ID = QueryString.IntSafeQ("id", 0);

            result.Data = new Coupon_Model();
            if (result.ID > 0)
            {
                result.Data = CouponM_BLL.Instance.getCouponDetail(result.ID);
            }
            return View(result);
        }

        public ActionResult OperateCoupon(Coupon_Model model)
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
                sqlResult = CouponM_BLL.Instance.addCoupon(model);
            }
            else
            {
                model.UpdateTime = DateTime.Now.ToLocalTime();
                model.Updater = this.UserID;
                sqlResult = CouponM_BLL.Instance.updateCoupon(model);
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
        public ActionResult DeleteCoupon(Coupon_Model model)
        {
            ObjectResult<bool> result = new ObjectResult<bool>();
            result.Code = "0";
            result.Data = false;
            result.Message = "系统错误";

            int sqlResult = CouponM_BLL.Instance.deleteCoupon(model);
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