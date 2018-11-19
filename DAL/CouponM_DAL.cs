using BLToolkit.Data;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CouponM_DAL
    {
        #region 构造类实例
        public static CouponM_DAL Instance
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
            internal static readonly CouponM_DAL instance = new CouponM_DAL();
        }

        #endregion

        public List<Coupon_Model> getCouponList(int Type,int Status,string Name, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `Inf_Coupon` where 1=1 {0} order by id desc LIMIT @StartCount,@EndCount ";

                string strWhere = "";

                if (Status > 0)
                {
                    strWhere += " and Status=@Status ";
                }

                if (Type > 0)
                {
                    strWhere += " and Type=@Type ";
                }

                if (!string.IsNullOrEmpty(Name)) {
                    strWhere += " and Name like @Name ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Coupon_Model> result = db.SetCommand(strSql
                     , db.Parameter("@Status", Status, DbType.Int32)
                     , db.Parameter("@Type", Type, DbType.Int32)
                     , db.Parameter("@Name", "%" + Name+ "%", DbType.String)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Coupon_Model>();



                return result;
            }
        }


        public Coupon_Model getCouponDetail(int ID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `Inf_Coupon` WHERE `ID` =@ID  ";


                Coupon_Model result = db.SetCommand(strSql
                     , db.Parameter("@ID", ID, DbType.Int32)).ExecuteObject<Coupon_Model>();



                return result;
            }
        }

        public int addCoupon(Coupon_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT INTO `inf_coupon` 
(`Name`,`ImageURL`,`Detail`,`Type`,`Rule`,`IsDuplicate`,`IsReuse`,`ExchangeType`,`MaxQty`,`Qty`,`UsedQty`,`ValidType`,`ValidRUle`,`Status`,`CreatetTime`,`Creator`,`ExchangeAmount`,`Weights`) 
VALUES
(@Name,@ImageURL,@Detail,@Type,@Rule,@IsDuplicate,@IsReuse,@ExchangeType,@MaxQty,0,0,@ValidType,@ValidRUle,1,@CreatetTime,@Creator,@ExchangeAmount,@Weights )  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Name", model.Name, DbType.String)
                     , db.Parameter("@ImageURL", model.ImageURL, DbType.String)
                     , db.Parameter("@Detail", model.Detail, DbType.String)
                     , db.Parameter("@Type", model.Type, DbType.Int32)
                     , db.Parameter("@Rule", model.Rule, DbType.String)
                     , db.Parameter("@IsDuplicate", model.IsDuplicate, DbType.Int32)
                     , db.Parameter("@IsReuse", model.IsReuse, DbType.Int32)
                     , db.Parameter("@ExchangeType", model.ExchangeType, DbType.Int32)
                     , db.Parameter("@MaxQty", model.MaxQty, DbType.Int32)
                     , db.Parameter("@ValidType", model.ValidType, DbType.Int32)
                     , db.Parameter("@ValidRUle", model.ValidRUle, DbType.String)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)
                     , db.Parameter("@ExchangeAmount", model.ExchangeAmount, DbType.Decimal)
                     , db.Parameter("@Weights", model.Weights, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int updateCoupon(Coupon_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                      `inf_coupon` 
                                    SET
                                      `ID` =@ID,
                                      `Name` =@Name,
                                      `ImageURL` =@ImageURL,
                                      `Detail` =@Detail,
                                      `Type` =@Type,
                                      `Rule` =@Rule,
                                      `IsDuplicate` =@IsDuplicate,
                                      `IsReuse` =@IsReuse,
                                      `ExchangeType` =@ExchangeType,
                                      `MaxQty` =@MaxQty,
                                      `ValidType` =@ValidType,
                                      `ValidRUle` =@ValidRUle,
                                      `UpdateTime` =@UpdateTime,
                                      `Updater` =@Updater,
                                      `ExchangeAmount` =@ExchangeAmount,
                                      `Weights` =@Weights 
                                    WHERE `ID` =@ID    ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Name", model.Name, DbType.String)
                     , db.Parameter("@ImageURL", model.ImageURL, DbType.String)
                     , db.Parameter("@Detail", model.Detail, DbType.String)
                     , db.Parameter("@Type", model.Type, DbType.Int32)
                     , db.Parameter("@Rule", model.Rule, DbType.String)
                     , db.Parameter("@IsDuplicate", model.IsDuplicate, DbType.Int32)
                     , db.Parameter("@IsReuse", model.IsReuse, DbType.Int32)
                     , db.Parameter("@ExchangeType", model.ExchangeType, DbType.Int32)
                     , db.Parameter("@MaxQty", model.MaxQty, DbType.Int32)
                     , db.Parameter("@ValidType", model.ValidType, DbType.Int32)
                     , db.Parameter("@ValidRUle", model.ValidRUle, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ExchangeAmount", model.ExchangeAmount, DbType.Decimal)
                     , db.Parameter("@Weights", model.Weights, DbType.Int32)
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteCoupon(Coupon_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Inf_Coupon` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ID` =@ID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }
    }
}
