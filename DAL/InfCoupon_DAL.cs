using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Operate_Model;
using Model.Table_Model;
using BLToolkit.Data;

namespace DAL
{
    public class InfCoupon_DAL
    {
        #region 构造类实例
        public static InfCoupon_DAL Instance
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
            internal static readonly InfCoupon_DAL instance = new InfCoupon_DAL();
        }

        #endregion
        public List<InfCoupon_Model> GetCoupon()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `ID`
                                          ,`Name`
                                          ,`ImageURL`
                                          ,`Detail`
                                          ,`Type`
                                          ,`Rule`
                                          ,`IsDuplicate`
                                          ,`IsReuse`
                                          ,`ExchangeType`
                                          ,`ExchangeAmount`
                                          ,`MaxQty`
                                          ,`Qty`
                                          ,`UsedQty`
                                          ,`ValidType`
                                          ,`ValidRUle`
                                          ,`Weights`
                                          ,`Status`
                                    FROM  `Inf_Coupon`
                                   WHERE  `Status` = 1 
                                     AND  `ExchangeType` = 2
                                ORDER BY  `Weights` DESC";
                List<InfCoupon_Model> list = db.SetCommand(strSql).ExecuteList<InfCoupon_Model>();
                return list;
            }
        }


        public InfCoupon_Model GetCouponInfo(int CouponID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `ID`
                                          ,`Name`
                                          ,`ImageURL`
                                          ,`Detail`
                                          ,`Type`
                                          ,`Rule`
                                          ,`IsDuplicate`
                                          ,`IsReuse`
                                          ,`ExchangeType`
                                          ,`ExchangeAmount`
                                          ,`MaxQty`
                                          ,`Qty`
                                          ,`UsedQty`
                                          ,`ValidType`
                                          ,`ValidRUle`
                                          ,`Weights`
                                          ,`Status`
                                    FROM  `Inf_Coupon`
                                   WHERE  `ID` = @ID
                                     AND  `Status` = 1
                                     AND  `MaxQty` > `Qty` ";
                InfCoupon_Model list = db.SetCommand(strSql, db.Parameter("@ID", CouponID, DbType.Int32)).ExecuteObject<InfCoupon_Model>();
                return list;
            }
        }

        public List<CouponMember_Model> UsingCouponMember(string CustomerCode,int LevelID)
        {
            string now = DateTime.Now.ToString("yyyyMMdd");
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  A.`ID`,
	                                       A.`Name`,
	                                       A.`Type`,
	                                       A.`Rule`,
	                                       B.`CouponCode`
                                     FROM `Inf_Coupon` A, `Ope_CustomerCoupon` B, `Inf_CouponCategory` C, `Set_MemberLevel` D
                                    WHERE  A.`Status` = 1
                                      AND  B.`Status` = 1
                                      AND  C.`Status` = 1
                                      AND  C.`Type` = 2
                                      AND  A.`ID` = B.`ID`
                                      AND  A.`ID` = C.`ID`
                                      AND  C.`Value` = D.`ServiceCode`
                                      AND  ((A.`IsReuse` = 1 and B.`IsUsed` = 1) OR A.`IsReuse` = 2)
                                      AND  B.`CustomerCode` = @CustomerCode
                                      AND  DATE_FORMAT(B.`StartDate`,'%Y%m%d') <= @now
                                      AND  DATE_FORMAT(B.`EndDate`,'%Y%m%d') >= @now
                                      AND  D.`LevelID` = @LevelID ";
                List<CouponMember_Model> list = db.SetCommand(strSql
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@now", now, DbType.String)
                    , db.Parameter("@LevelID", LevelID, DbType.Int32)).ExecuteList<CouponMember_Model>();
                return list;
            }
        }



        public List<CouponMember_Model> UsingCouponService(string CustomerCode, string ServiceCode)
        {
            string now = DateTime.Now.ToString("yyyyMMdd");
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  A.`ID`,
	                                       A.`Name`,
	                                       A.`Type`,
	                                       A.`Rule`,
	                                       B.`CouponCode`
                                     FROM `Inf_Coupon` A, `Ope_CustomerCoupon` B, `Inf_CouponCategory` C
                                    WHERE  A.`Status` = 1
                                      AND  B.`Status` = 1
                                      AND  C.`Status` = 1
                                      AND  C.`Type` = 2
                                      AND  A.`ID` = B.`ID`
                                      AND  A.`ID` = C.`ID`
                                      AND  ((A.`IsReuse` = 1 and B.`IsUsed` = 1) OR A.`IsReuse` = 2)
                                      AND  C.`Value` = @ServiceCode
                                      AND  B.`CustomerCode` = @CustomerCode
                                      AND  DATE_FORMAT(B.`StartDate`,'%Y%m%d') <= @now
                                      AND  DATE_FORMAT(B.`EndDate`,'%Y%m%d') >= @now ";
                List<CouponMember_Model> list = db.SetCommand(strSql
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@now", now, DbType.String)
                    , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteList<CouponMember_Model>();
                return list;
            }
        }
    }
}
