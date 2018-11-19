using Model.Operate_Model;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.Data;

namespace DAL
{
    public class OpeCouponCode_DAL
    {
        #region 构造类实例
        public static OpeCouponCode_DAL Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested()
            {
            }
            internal static readonly OpeCouponCode_DAL instance = new OpeCouponCode_DAL();
        }

        #endregion
        public InfCoupon_Model GetCouponCodeID(string Code)
        {
            string now = DateTime.Now.ToString("yyyyMMdd");
            using (DbManager db = new DbManager("changyi"))
            {
                string strSql = @" SELECT  A.`ID`
                                          ,B.`ValidType`
                                          ,B.`ValidRUle`
                                     FROM  `Ope_CouponCode` A,`Inf_Coupon` B
                                    WHERE  A.`ID` = B.`ID`
                                      AND  DATE_FORMAT(A.`StartDate`,'%Y%m%d') <= @now
                                      AND  DATE_FORMAT(A.`EndDate`,'%Y%m%d') >= @now
                                      AND  A.`Status` = 1
                                      AND  B.`Status` = 1
                                      AND (B.`MaxQty` IS NULL OR B.`MaxQty` > B.`Qty`)
                                      AND  A.`Code` = @Code ";
                InfCoupon_Model result = db.SetCommand(strSql
                    , db.Parameter("@Code", Code, DbType.String)
                    , db.Parameter("@now", now, DbType.String)).ExecuteObject<InfCoupon_Model>();
                return result;
            }
        }
    }
}
