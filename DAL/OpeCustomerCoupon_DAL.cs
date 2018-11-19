using Model.Operate_Model;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BLToolkit.Data;

namespace DAL
{
    public class OpeCustomerCoupon_DAL
    {
        #region 构造类实例
        public static OpeCustomerCoupon_DAL Instance
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
            internal static readonly OpeCustomerCoupon_DAL instance = new OpeCustomerCoupon_DAL();
        }

        #endregion

        public List<CustomerCoupon_Model> GetCustomerCoupon(string CustomerCode)
        {
            string now = DateTime.Now.ToString("yyyyMMdd");
            using (DbManager db = new DbManager("changyi"))
            {
                string strSql = @" SELECT  A.`CouponCode`
                                          ,A.`ID`
                                          ,A.`GotDate`
                                          ,A.`StartDate`
                                          ,A.`EndDate`
                                          ,B.`Name`
                                          ,B.`ImageURL`
                                          ,B.`Detail`
                                          ,B.`Type`
                                          ,B.`Rule`
                                          ,B.`IsDuplicate`
                                          ,B.`IsReuse`
                                          ,B.`ExchangeType`
                                          ,B.`ExchangeAmount`
                                          ,B.`MaxQty`
                                          ,B.`Qty`
                                          ,B.`UsedQty`
                                          ,B.`ValidType`
                                          ,B.`ValidRUle`
                                          ,B.`Weights`
                                     FROM  `Ope_CustomerCoupon` A, `Inf_Coupon` B
                                    WHERE  A.`ID` = B.`ID`
                                      AND  A.`Status` = 1
                                      AND  B.`Status` = 1
                                      AND  A.`IsUsed` = 1
                                      AND  DATE_FORMAT(A.`StartDate`,'%Y%m%d') <= @now
                                      AND  DATE_FORMAT(A.`EndDate`,'%Y%m%d') >= @now
                                      AND  A.`CustomerCode` = @CustomerCode
                                 ORDER BY  A.`CreatetTime` DESC ";
                List<CustomerCoupon_Model> result = db.SetCommand(strSql
                    , db.Parameter("@now", now, DbType.String)
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteList<CustomerCoupon_Model>();
                return result;
            }
        }

        public int CodeExchange(string CustomerCode,int UserID, InfCoupon_Model CouponId)
        {
            DateTime now = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                
                //存储过程获取优惠券编号
                var outParmeter = db.OutputParameter("@Result", DbType.String);
                var sf = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "Ope_CustomerCoupon", DbType.String)
                                  , outParmeter).ExecuteScalar<string>();
                string CouponCode = outParmeter.Value.ToString();
                if (string.IsNullOrEmpty(CouponCode))
                {
                    return 0;
                }

                //计算开始日期与终了日期
                DateTime startData;
                DateTime endData;
                if (CouponId.ValidType == 1)
                {
                    string[] sArray = Regex.Split(CouponId.ValidRUle, "-", RegexOptions.IgnoreCase);
                    startData = DateTime.ParseExact(sArray[0], "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);
                    endData = DateTime.ParseExact(sArray[1], "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);
                }
                else
                {
                    startData = now;
                    endData = now.AddDays(int.Parse(CouponId.ValidRUle));
                }

                //插入客户优惠券表
                string strIns = @" INSERT `Ope_CustomerCoupon` (`CouponCode`, `CustomerCode`, `ID`, `GotDate`, `StartDate`, `EndDate`, `IsUsed`, `Status`, `CreatetTime`, `Creator`)
                                    VALUES (@CouponCode,@CustomerCode,@ID,@now,@startData,@endData,1,1,@now,@UserID)";

                int row = db.SetCommand(strIns
                    , db.Parameter("@CouponCode", CouponCode, DbType.String)
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@ID", CouponId.ID, DbType.Int32)
                    , db.Parameter("@startData", startData, DbType.Date)
                    , db.Parameter("@endData", endData, DbType.Date)
                    , db.Parameter("@now", now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }
                //更新优惠券属性表的已发数量
                string strUpd = @" UPDATE  `Inf_Coupon`
                                      SET  `Qty` = Qty + 1
                                          ,`UpdateTime` = @now
                                          ,`Updater` = @UserID 
                                    WHERE  `ID` = @ID ";

                row = db.SetCommand(strUpd
                    , db.Parameter("@ID", CouponId.ID, DbType.Int32)
                    , db.Parameter("@now", now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                db.CommitTransaction();
                return 1;
            }
        }

        public int BalanceExchange(string CustomerCode, int UserID, decimal Balance, InfCoupon_Model CouponId)
        {
            DateTime now = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                //判断是否重复领取
                if (CouponId.IsDuplicate == 1)
                {
                    string strCheck = @" SELECT COUNT(1) FROM `Ope_CustomerCoupon` WHERE `CustomerCode` = @CustomerCode AND `ID` = @ID ";
                    int count = db.SetCommand(strCheck
                            , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                            , db.Parameter("@ID", CouponId.ID, DbType.Int32)).ExecuteScalar<int>();
                    if (count > 0)
                    {
                        return 2;
                    }
                }
                //存储过程获取优惠券编号
                var outParmeter = db.OutputParameter("@Result", DbType.String);
                var sf = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "Ope_CustomerCoupon", DbType.String)
                                  , outParmeter).ExecuteScalar<string>();
                string CouponCode = outParmeter.Value.ToString();
                if (string.IsNullOrEmpty(CouponCode))
                {
                    return 0;
                }
                //计算开始日期与终了日期
                DateTime startData;
                DateTime endData;
                if (CouponId.ValidType == 1)
                {
                    string[] sArray = Regex.Split(CouponId.ValidRUle, "-", RegexOptions.IgnoreCase);
                    startData = DateTime.ParseExact(sArray[0], "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);
                    endData = DateTime.ParseExact(sArray[1], "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture);
                }
                else
                {
                    startData = now;
                    endData = now.AddDays(int.Parse(CouponId.ValidRUle));
                }

                //插入客户优惠券表
                string strInsCoupon = @" INSERT `Ope_CustomerCoupon` (`CouponCode`, `CustomerCode`, `ID`, `GotDate`, `StartDate`, `EndDate`, `IsUsed`, `Status`, `CreatetTime`, `Creator`)
                                    VALUES (@CouponCode,@CustomerCode,@ID,@now,@startData,@endData,1,1,@now,@UserID)";

                int row = db.SetCommand(strInsCoupon
                    , db.Parameter("@CouponCode", CouponCode, DbType.String)
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@ID", CouponId.ID, DbType.Int32)
                    , db.Parameter("@startData", startData, DbType.Date)
                    , db.Parameter("@endData", endData, DbType.Date)
                    , db.Parameter("@now", now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }
                //更新优惠券属性表的已发数量
                string strUpdCoupon = @" UPDATE  `Inf_Coupon`
                                      SET  `Qty` = Qty + 1
                                          ,`UpdateTime` = @now
                                          ,`Updater` = @UserID 
                                    WHERE  `ID` = @ID ";

                row = db.SetCommand(strUpdCoupon
                    , db.Parameter("@ID", CouponId.ID, DbType.Int32)
                    , db.Parameter("@now", now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                //新增健康币流水表记录
                string strBalanceIns = @" INSERT `Ope_Balance` (`CustomerCode`, `InOutType`, `Type`, `ChangeBalance`, `Balance`, `RelatedType`, `RelatedID`, `Status`, `CreatetTime`, `Creator`)
                                           VALUES (@CustomerCode,2,52,@ChangeBalance,@Balance,4,@RelatedID,1,@now,@UserID)";
                 row = db.SetCommand(strBalanceIns
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                    , db.Parameter("@ChangeBalance", CouponId.ExchangeAmount, DbType.Decimal)
                    , db.Parameter("@Balance", Balance - CouponId.ExchangeAmount, DbType.Decimal)
                    , db.Parameter("@RelatedID", CouponCode, DbType.String)
                    , db.Parameter("@now", now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();

                if (row <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                //更新客户基本信息表
                string strBalanceUpd = @" UPDATE `Inf_Customer`
                                             SET `Balance` = `Balance` - @ChangeBalance
                                                ,`UpdateTime` = @now
                                                ,`Updater` = @UserID
                                           WHERE `CustomerCode` = @CustomerCode ";
                row = db.SetCommand(strBalanceUpd
                        , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                        , db.Parameter("@ChangeBalance", CouponId.ExchangeAmount, DbType.Decimal)
                        , db.Parameter("@now", now, DbType.DateTime)
                        , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();

                if (row <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                db.CommitTransaction();
                return 1;
            }
        }
    }
}
