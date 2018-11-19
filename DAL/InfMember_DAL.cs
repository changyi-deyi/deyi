using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Operate_Model;
using Model.Table_Model;
using BLToolkit.Data;
using Model.View_Model;
using Common.Util;

namespace DAL
{
    public class InfMember_DAL
    {
        #region 构造类实例
        public static InfMember_DAL Instance
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
            internal static readonly InfMember_DAL instance = new InfMember_DAL();
        }

        #endregion
        public InfMember_Model GetMember(string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `CustomerCode`
                                          ,`MemberCode`
                                          ,`ChannelID`
                                          ,`LevelID`
                                          ,`Type`
                                          ,`FamilyCode`
                                          ,`MaxQty`
                                          ,`FreeQty`
                                          ,`ExpiredDate`
                                          ,`IDNumber`
                                          ,`Status`
                                    FROM  `Inf_Member`
                                   WHERE  `CustomerCode` = @CustomerCode 
                                     AND  `Status` = 1 ";
                InfMember_Model model = db.SetCommand(strSql
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteObject<InfMember_Model>();
                return model;
            }
        }


        public InfMember_Model GetCustomerLevel(int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  A.`MemberCode`
                                          ,A.`LevelID`
                                    FROM  `Inf_Member` A, Inf_Customer B
                                   WHERE  A.`CustomerCode` = B.`CustomerCode`
                                     AND  B.`UserID` = @UserID
                                     AND  A.`Status` = 1
                                     AND  B.`Status` = 1 ";
                InfMember_Model model = db.SetCommand(strSql
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteObject<InfMember_Model>();
                return model;
            }
        }

        public List<InfFamily_Model> GetMemberFamily(string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `Type`
                                          ,`FamilyCode`
                                    FROM  `Inf_Member`
                                   WHERE  `CustomerCode` = @CustomerCode 
                                     AND  `Status` = 1 ";
                InfMember_Model model = db.SetCommand(strSql
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteObject<InfMember_Model>();

                List<InfFamily_Model> result = null;
                if (model != null && model.Type == 2)
                {
                    string strSqlFamily = @" SELECT  `ID`
                                                    ,`FamilyCode`
                                                    ,`IDNumber`
                                                    ,`Name`
                                                    ,`Relationship`
                                               FROM  `Inf_Family`
                                              WHERE  `FamilyCode` = @FamilyCode";
                    result = db.SetCommand(strSqlFamily
                    , db.Parameter("@FamilyCode", CustomerCode, DbType.String)).ExecuteList<InfFamily_Model>();
                }

                return result;
            }
        }

        public int SetMember(ExchangeMember_Model model, int ChannelID, int LevelID)
        {
            DateTime now = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSqlSel = @" SELECT COUNT(1) FROM `Inf_Member` WHERE `CustomerCode` = @CustomerCode ";

                int count = db.SetCommand(strSqlSel
                     , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteScalar<int>();

                if (count == 0)
                {
                    //存储过程获取订单编号
                    var outParmeter1 = db.OutputParameter("@Result", DbType.String);
                    var sf1 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "Ope_MemberOrder", DbType.String)
                                      , outParmeter1).ExecuteScalar<string>();
                    string OrderCode = outParmeter1.Value.ToString();

                    //存储过程获取会员编号
                    var outParmeter2 = db.OutputParameter("@Result", DbType.String);
                    var sf2 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "Inf_Member", DbType.String)
                                      , outParmeter2).ExecuteScalar<string>();
                    string MemberCode = outParmeter2.Value.ToString();

                    //存储过程获取家庭编号
                    var outParmeter3 = db.OutputParameter("@Result", DbType.String);
                    var sf3 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "Family", DbType.String)
                                      , outParmeter3).ExecuteScalar<string>();
                    string FamilyCode = outParmeter3.Value.ToString();

                    //插入会员订单表
                    //string strOrderIns = @" INSERT `Ope_MemberOrder` (`OrderCode`, `CustomerCode`, `MemberLevelID`, `Type`, `Qty`, `OrderAmount`, `PaymentStatus`, `OrderStatus`, `Status`, `CreatetTime`, `Creator`)
                    //                         VALUES (@OrderCode, @CustomerCode, @MemberLevelID, 1, 1, 0, 3, 2, 1, @now, @UserID) ";

                    //int rows = db.SetCommand(strOrderIns
                    //     , db.Parameter("@OrderCode", OrderCode, DbType.String)
                    //     , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                    //     , db.Parameter("@MemberLevelID", LevelID, DbType.Int32)
                    //     , db.Parameter("@now", now, DbType.DateTime)
                    //     , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    //if (rows <= 0)
                    //{
                    //    db.RollbackTransaction();
                    //    return 0;
                    //}

                    //插入会员信息表
                    string strMemberIns = @" INSERT `Inf_Member` (`CustomerCode`, `MemberCode`, `ChannelID`, `LevelID`, `Type`, `FamilyCode`, `MaxQty`, `FreeQty`, `ExpiredDate`, `IDNumber`, `Status`, `CreatetTime`, `Creator`)
                                         VALUES (@CustomerCode, @MemberCode, @ChannelID, @LevelID, 1, @FamilyCode, 1, 0, @nextyear, @IDNumber, 1, @now, @UserID) ";

                    int rows = db.SetCommand(strMemberIns
                         , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                         , db.Parameter("@MemberCode", MemberCode, DbType.String)
                         , db.Parameter("@ChannelID", ChannelID, DbType.Int32)
                         , db.Parameter("@LevelID", LevelID, DbType.Int32)
                         , db.Parameter("@FamilyCode", FamilyCode, DbType.String)
                         , db.Parameter("@nextyear", now.AddYears(1), DbType.DateTime)
                         , db.Parameter("@IDNumber", model.IDNumber, DbType.String)
                         , db.Parameter("@now", now, DbType.DateTime)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }


                    //更新客户基本信息表
                    string strCustomerUpd = @" UPDATE  `Inf_Customer`
                                              SET  `MemberCode` = @MemberCode
                                                  ,`Name` = @Name
                                                  ,`IsMember` = 2
                                                  ,`IDNumber` = @IDNumber
                                                  ,`UpdateTime` = @now
                                                  ,`Updater` = @UserID
                                            WHERE  `UserID` = @UserID
                                              AND  `Status` = 1 ";
                    rows = db.SetCommand(strCustomerUpd
                         , db.Parameter("@MemberCode", MemberCode, DbType.String)
                         , db.Parameter("@IDNumber", model.IDNumber, DbType.String)
                         , db.Parameter("@Name", model.Name, DbType.String)
                         , db.Parameter("@now", now, DbType.DateTime)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    //更新会员卡卡密表
                    string strCardUpd = @" UPDATE  `Set_CardNumPas`
                                          SET  `IsExchange` = 2
                                              ,`Exchanger` = @UserID
                                              ,`UpdateTime` = @now
                                              ,`Updater` = @UserID
                                        WHERE  `CardNumber` = @CardNumber
                                          AND  `Passeord` = @Passeord ";
                    rows = db.SetCommand(strCardUpd
                         , db.Parameter("@CardNumber", model.CardNumber, DbType.String)
                         , db.Parameter("@Passeord", model.Passeord, DbType.String)
                         , db.Parameter("@now", now, DbType.DateTime)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                else
                { 
                    //更新会员信息表
                    string strMemberIns = @" UPDATE  `Inf_Member`
                                                SET  `LevelID` = @LevelID
                                                    ,`Type` = 1
                                                    ,`MaxQty` = 1
                                                    ,`FreeQty` = 0
                                                    ,`ExpiredDate` = @nextyear
                                                    ,`UpdateTime` = @now
                                                    ,`Updater` = @UserID
                                              WHERE  `CustomerCode` = @CustomerCode";

                    int rows = db.SetCommand(strMemberIns
                         , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                         , db.Parameter("@LevelID", LevelID, DbType.Int32)
                         , db.Parameter("@nextyear", now.AddYears(1), DbType.DateTime)
                         , db.Parameter("@IDNumber", model.IDNumber, DbType.String)
                         , db.Parameter("@now", now, DbType.DateTime)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    //更新客户基本信息表
                    string strCustomerUpd = @" UPDATE  `Inf_Customer`
                                              SET  `IsMember` = 2
                                                  ,`Name` = @Name
                                                  ,`IDNumber` = @IDNumber
                                                  ,`UpdateTime` = @now
                                                  ,`Updater` = @UserID
                                            WHERE  `UserID` = @UserID
                                              AND  `Status` = 1 ";
                    rows = db.SetCommand(strCustomerUpd
                         , db.Parameter("@IDNumber", model.IDNumber, DbType.String)
                         , db.Parameter("@Name", model.Name, DbType.String)
                         , db.Parameter("@now", now, DbType.DateTime)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    //更新会员卡卡密表
                    string strCardUpd = @" UPDATE  `Set_CardNumPas`
                                              SET  `IsExchange` = 2
                                                  ,`Exchanger` = @UserID
                                                  ,`UpdateTime` = @now
                                                  ,`Updater` = @UserID
                                            WHERE  `CardNumber` = @CardNumber
                                              AND  `Passeord` = @Passeord ";
                    rows = db.SetCommand(strCardUpd
                         , db.Parameter("@CardNumber", model.CardNumber, DbType.String)
                         , db.Parameter("@Passeord", model.Passeord, DbType.String)
                         , db.Parameter("@now", now, DbType.DateTime)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                }
                db.CommitTransaction();
                return 1;
            }
        }

        public AddMemberOrderResult_Model addMemberOrder(AddMemberOrder_Model model)
        {
            using (DbManager db = new DbManager())
            {
                DateTime dt = DateTime.Now.ToLocalTime();
                AddMemberOrderResult_Model result = new AddMemberOrderResult_Model();
                result.Success = false;
                result.Message = "系统错误";

                db.BeginTransaction();

                #region 判断优惠券
                if (!string.IsNullOrEmpty(model.CouponCode))
                {
                    string strSqlCheckCoupon = @" SELECT * FROM `ope_customercoupon` WHERE `CouponCode` =@CouponCode ";

                    OpeCustomerCoupon_Model Coupon = db.SetCommand(strSqlCheckCoupon
                         , db.Parameter("@CouponCode", model.CouponCode, DbType.String)).ExecuteObject<OpeCustomerCoupon_Model>();

                    if (Coupon == null)
                    {
                        db.RollbackTransaction();
                        result.Message = "优惠券不存在";
                        return result;
                    }

                    if (Coupon.IsUsed == 2)
                    {
                        db.RollbackTransaction();
                        result.Message = "优惠券已经使用";
                        return result;
                    }

                    if (Coupon.EndDate < dt)
                    {
                        db.RollbackTransaction();
                        result.Message = "优惠券已过期";
                        return result;
                    }
                    if (Coupon.StartDate > dt)
                    {
                        db.RollbackTransaction();
                        result.Message = "优惠券不可用";
                        return result;
                    }
                }
                #endregion

                #region 判断会员

                string strSqlLevel = @" SELECT * FROM `set_memberlevel` WHERE levelID =@levelID ";
                SetMemberLevel_Model member = db.SetCommand(strSqlLevel
                     , db.Parameter("@levelID", model.MemberLevelID, DbType.Int32)).ExecuteObject<SetMemberLevel_Model>();

                if (member == null || member.Status == 2)
                {
                    db.RollbackTransaction();
                    result.Message = "会员等级已无效";
                    return result;
                }

                if (member.OriginPrice != model.OriginPrice)
                {

                    db.RollbackTransaction();
                    result.Message = "价格发生变化，请重新提交";
                    return result;
                }
                #endregion


                var outParmeter1 = db.OutputParameter("@Result", DbType.String);
                var sf1 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "Ope_MemberOrder", DbType.String)
                                  , outParmeter1).ExecuteScalar<string>();
                string OrderCode = outParmeter1.Value.ToString();

                if (string.IsNullOrEmpty(OrderCode))
                {
                    db.RollbackTransaction();
                    return result;
                }

                string strSqlCustomer = @" UPDATE `Inf_Customer`
                                           SET `Name` = @Name
                                              ,`IDNumber` = @IDNumber
                                              ,`UpdateTime` = @UpdateTime
                                              ,`Updater` = @Updater
                                         WHERE `CustomerCode` = @CustomerCode ";

                int rows = db.SetCommand(strSqlCustomer
                                  , db.Parameter("@Name", model.Name, DbType.String)
                                  , db.Parameter("@IDNumber", model.idNumber, DbType.String)
                                  , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                                  , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                                  , db.Parameter("@Updater", model.UserID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return result;
                }


                string strSqlOrder = @" INSERT INTO `ope_memberorder` 
                            (`OrderCode`,`CustomerCode`,`MemberLevelID`,`Type`,`Qty`,`OriginAmount`,`OrderAmount`,`CouponCode`,`DiscountAmount`,`PaymentStatus`,`OrderStatus`,`Status`,`CreatetTime`,`Creator`) 
                            VALUES
                            (@OrderCode,@CustomerCode,@MemberLevelID,1,1,@OriginAmount,@OrderAmount,@CouponCode,@DiscountAmount,1,1,1,@CreatetTime,@Creator) ;
                            ";

                rows = db.SetCommand(strSqlOrder
                                  , db.Parameter("@OrderCode", OrderCode, DbType.String)
                                  , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                                  , db.Parameter("@MemberLevelID", model.MemberLevelID, DbType.Int32)
                                  , db.Parameter("@OriginAmount", model.OriginPrice, DbType.Decimal)
                                  , db.Parameter("@OrderAmount", model.OrderAmount, DbType.Decimal)
                                  , db.Parameter("@CouponCode", model.CouponCode, DbType.String)
                                  , db.Parameter("@DiscountAmount", model.DiscountAmount, DbType.Decimal)
                                  , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                                  , db.Parameter("@Creator", model.UserID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return result;
                }


                var outParmeter2 = db.OutputParameter("@Result", DbType.String);
                var sf2 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "ope_wechatresult", DbType.String)
                                  , outParmeter2).ExecuteScalar<string>();

                string NetTradeCode = outParmeter2.Value.ToString();

                if (string.IsNullOrEmpty(NetTradeCode))
                {
                    db.RollbackTransaction();
                    return result;
                }

                string strSqlWeChat = @" INSERT INTO `ope_wechatresult` 
	                                (`NetTradeCode`,`Type`,`OrderCode`,`Status`,`CreatetTime`,`Creator`,`Amount`) 
                                    VALUES
	                                (@NetTradeCode,1,@OrderCode,1,@CreatetTime,@Creator,@Amount)  ";

                rows = db.SetCommand(strSqlWeChat
                                  , db.Parameter("@NetTradeCode", NetTradeCode, DbType.String)
                                  , db.Parameter("@OrderCode", OrderCode, DbType.String)
                                  , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                                  , db.Parameter("@Creator", model.UserID, DbType.Int32)
                                  , db.Parameter("@Amount", model.OrderAmount, DbType.Decimal)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return result;
                }

                result.OrderCode = OrderCode;
                result.NetTradeCode = NetTradeCode;
                result.OrderAmount = model.OrderAmount;
                result.Success = true;

                db.CommitTransaction();
                return result;
            }
        }


        public int UpdatePayMemberOrderResult(string NetTradeCode, string Data, int mode)
        {
            using (DbManager db = new DbManager())
            {
                DateTime dt = DateTime.Now.ToLocalTime();
                db.BeginTransaction();
                string strSqlOrder = @" SELECT a.`Type`,a.`OrderCode`,b.`CustomerCode`,b.`OrderAmount`,b.`MemberLevelID`,a.`PayMentCode`,a.`Amount`,b.CouponCode,b.DiscountAmount,c.`IDNumber` FROM `ope_wechatresult` a 
                                INNER JOIN `ope_memberorder` b
                                ON a.`OrderCode` = b.`OrderCode`
                                LEFT JOIN `inf_customer` c
                                ON b.`CustomerCode` = c.`CustomerCode`
                                WHERE a.`NetTradeCode` =@NetTradeCode ";

                PaymentResultView_Model PaymentResult = db.SetCommand(strSqlOrder
                               , db.Parameter("@NetTradeCode", NetTradeCode, DbType.String)).ExecuteObject<PaymentResultView_Model>();

                if (PaymentResult == null)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                var outParmeter1 = db.OutputParameter("@Result", DbType.String);
                var sf1 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "ope_payment", DbType.String)
                                  , outParmeter1).ExecuteScalar<string>();
                string PayMentCode = outParmeter1.Value.ToString();

                if (string.IsNullOrEmpty(PayMentCode))
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strSqlInsertPayment = @"   INSERT INTO `ope_payment` (
                                `PayMentCode`,`OrderCode`,`CustomerCode`,`Type`,`Source`,`OrderAmount`,`Status`,`CreatetTime`,`Creator`,`RelatedPaymentCode`) 
                                VALUES
                                (@PayMentCode,@OrderCode,@CustomerCode,1,1,@OrderAmount,1,@CreatetTime,@Creator,@RelatedPaymentCode)   ";



                int rows = db.SetCommand(strSqlInsertPayment
                                  , db.Parameter("@PayMentCode", PayMentCode, DbType.String)
                                  , db.Parameter("@OrderCode", PaymentResult.OrderCode, DbType.String)
                                  , db.Parameter("@CustomerCode", PaymentResult.CustomerCode, DbType.String)
                                  , db.Parameter("@OrderAmount", PaymentResult.OrderAmount, DbType.Decimal)
                                  , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                                  , db.Parameter("@Creator", PaymentResult.Creator, DbType.Int32)
                                  , db.Parameter("@RelatedPaymentCode", PaymentResult.PayMentCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                var outParmeter2 = db.OutputParameter("@Result", DbType.String);
                var sf2 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "Ope_PaymentDetail", DbType.String)
                                  , outParmeter2).ExecuteScalar<string>();
                string DetailCode = outParmeter2.Value.ToString();

                if (string.IsNullOrEmpty(DetailCode))
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strSqlDetail = @" INSERT INTO `ope_paymentdetail` 
                                    (`PayMentCode`,`DetailCode`,`Mode`,`PaidAmount`,`Status`,`CreatetTime`,`Creator`) 
                                    VALUES
                                      (@PayMentCode,@DetailCode,@MODE,@PaidAmount,1,@CreatetTime,@Creator) ";

                rows = db.SetCommand(strSqlDetail
                                  , db.Parameter("@PayMentCode", PayMentCode, DbType.String)
                                  , db.Parameter("@DetailCode", DetailCode, DbType.String)
                                  , db.Parameter("@MODE", mode, DbType.Int32)
                                  , db.Parameter("@PaidAmount", StringUtils.GetDbInt(PaymentResult.Amount / 100), DbType.Decimal)
                                  , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                                  , db.Parameter("@Creator", PaymentResult.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }


                string strSqlUpdateOrder = @" UPDATE 
                          `ope_memberorder` 
                        SET
                          `PaymentStatus` = 3,
                          `PaymentTime` =@PaymentTime,
                          `UpdateTime` =@UpdateTime,
                          `Updater` =@Updater, 
                          `OrderStatus` = 2,
                          `FinishTime` =@FinishTime
                        WHERE `OrderCode` =@OrderCode";

                rows = db.SetCommand(strSqlUpdateOrder
                              , db.Parameter("@PaymentTime", dt, DbType.DateTime)
                              , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                              , db.Parameter("@FinishTime", dt, DbType.DateTime)
                              , db.Parameter("@Updater", PaymentResult.Creator, DbType.Int32)
                              , db.Parameter("@OrderCode", PaymentResult.OrderCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                if (!string.IsNullOrEmpty(PaymentResult.CouponCode))
                {
                    var outParmeter3 = db.OutputParameter("@Result", DbType.String);
                    var sf3 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "Ope_PaymentDetail", DbType.String)
                                      , outParmeter3).ExecuteScalar<string>();
                    string DetailCode2 = outParmeter3.Value.ToString();

                    if (string.IsNullOrEmpty(DetailCode2))
                    {
                        db.RollbackTransaction();
                        return 0;
                    }


                    rows = db.SetCommand(strSqlDetail
                                      , db.Parameter("@PayMentCode", PayMentCode, DbType.String)
                                      , db.Parameter("@DetailCode", DetailCode2, DbType.String)
                                      , db.Parameter("@MODE", 3, DbType.Int32)
                                      , db.Parameter("@PaidAmount", PaymentResult.DiscountAmount, DbType.Decimal)
                                      , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                                      , db.Parameter("@Creator", PaymentResult.Creator, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    string strSqlCoupon = @" UPDATE 
                                      `Ope_CustomerCoupon` 
                                    SET
                                      `IsUsed` =2,
                                      `UpdateTime` =@UpdateTime,
                                      `Updater` =@Updater
                                    WHERE `CouponCode` =@CouponCode";

                    rows = db.SetCommand(strSqlCoupon
                                , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                                , db.Parameter("@Updater", PaymentResult.Creator, DbType.Int32)
                                , db.Parameter("@CouponCode", PaymentResult.CouponCode, DbType.String)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }


                string strSqlMember = @" SELECT * FROM `inf_member` WHERE `CustomerCode`=@CustomerCode";

                InfMember_Model member = db.SetCommand(strSqlMember
                          , db.Parameter("@CustomerCode", PaymentResult.CustomerCode, DbType.String)).ExecuteObject<InfMember_Model>();

                if (member == null)
                {



                    var outParmeter4 = db.OutputParameter("@Result", DbType.String);
                    var sf4 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "Family", DbType.String)
                                      , outParmeter4).ExecuteScalar<string>();
                    string FamilyCode = outParmeter4.Value.ToString();

                    if (string.IsNullOrEmpty(FamilyCode))
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    var outParmeter5 = db.OutputParameter("@Result", DbType.String);
                    var sf5 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "Inf_Member", DbType.String)
                                      , outParmeter5).ExecuteScalar<string>();
                    string MemberCode = outParmeter5.Value.ToString();

                    if (string.IsNullOrEmpty(MemberCode))
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    string strSqlMemberInsert = @"INSERT INTO `inf_member` 
                                    (`CustomerCode`,`MemberCode`,`ChannelID`,`LevelID`,`Type`,`FamilyCode`,`MaxQty`,`FreeQty`,`ExpiredDate`,`IDNumber`,`Status`,`CreatetTime`,`Creator`) 
                                    VALUES
                                    (@CustomerCode,@MemberCode,@ChannelID,@LevelID,@Type,@FamilyCode,@MaxQty,@FreeQty,@ExpiredDate,@IDNumber,1,@CreatetTime,@Creator) ";


                    rows = db.SetCommand(strSqlMemberInsert
                              , db.Parameter("@CustomerCode", PaymentResult.CustomerCode, DbType.String)
                              , db.Parameter("@MemberCode", MemberCode, DbType.String)
                              , db.Parameter("@ChannelID", 1, DbType.Int32)
                              , db.Parameter("@LevelID", PaymentResult.MemberLevelID, DbType.Int32)
                              , db.Parameter("@Type", 1, DbType.Int32)
                              , db.Parameter("@FamilyCode", FamilyCode, DbType.String)
                              , db.Parameter("@MaxQty", 1, DbType.Int32)
                              , db.Parameter("@FreeQty", 0, DbType.Int32)
                              , db.Parameter("@ExpiredDate", dt.AddDays(364), DbType.DateTime)
                              , db.Parameter("@IDNumber", PaymentResult.IDNumber, DbType.String)
                              , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                              , db.Parameter("@Creator", PaymentResult.Creator, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }


                    string strSqlCustomer = @"
                            UPDATE 
                              `Inf_Customer` 
                            SET
                              `MemberCode` =@MemberCode,
                              `IsMember` = 2,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater 
                            WHERE `CustomerCode` =@CustomerCode";

                    rows = db.SetCommand(strSqlCustomer
                                , db.Parameter("@MemberCode", MemberCode, DbType.String)
                                , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                                , db.Parameter("@Updater", PaymentResult.Creator, DbType.Int32)
                                , db.Parameter("CustomerCode", PaymentResult.CustomerCode, DbType.String)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                }
                else {
                    string strSqlUpdateMember = @" UPDATE 
                                          `inf_member` 
                                        set
                                          `LevelID` =@LevelID,
                                          `ExpiredDate` =@ExpiredDate,
                                          `UpdateTime` =@UpdateTime,
                                          `Updater` =@Updater 
                                        where `MemberCode` =@MemberCode";

                    rows = db.SetCommand(strSqlUpdateMember
                            , db.Parameter("@LevelID", PaymentResult.MemberLevelID, DbType.Int32)
                            , db.Parameter("@ExpiredDate", dt.AddDays(364), DbType.DateTime)
                            , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                            , db.Parameter("@Updater", PaymentResult.Creator, DbType.Int32)
                            , db.Parameter("@MemberCode", member.MemberCode, DbType.String)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }



                    string strSqlCustomer = @"
                            UPDATE 
                              `Inf_Customer` 
                            SET
                              `MemberCode` =@MemberCode,
                              `IsMember` = 2,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater 
                            WHERE `CustomerCode` =@CustomerCode";

                    rows = db.SetCommand(strSqlCustomer
                                , db.Parameter("@MemberCode", member.MemberCode, DbType.String)
                                , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                                , db.Parameter("@Updater", PaymentResult.Creator, DbType.Int32)
                                , db.Parameter("CustomerCode", PaymentResult.CustomerCode, DbType.String)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                }
                string strSqlResult = @"
                            UPDATE 
                              `ope_wechatresult` 
                            SET
                              `Data` =@Data,
                              `PayMentCode` =@PayMentCode
                            WHERE `NetTradeCode` =@NetTradeCode";

                rows = db.SetCommand(strSqlResult
                          , db.Parameter("@Data", Data, DbType.String)
                          , db.Parameter("@PayMentCode", PayMentCode, DbType.String)
                          , db.Parameter("@NetTradeCode", NetTradeCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }



                db.CommitTransaction();
                return 1;
            }
        }

        public int CancelPayMember(OpeMemberOrder_Model model) {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSql = @" UPDATE 
                                  `ope_memberorder` 
                                SET
                                  `PaymentStatus` =1,
                                  `OrderStatus` = 3,
                                  `UpdateTime` = @UpdateTime,
                                  `Updater` = @Updater
                                WHERE `OrderCode` = @OrderCode  ";

                int rows = db.SetCommand(strSql
                        , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                        , db.Parameter("@Updater", model.Updater, DbType.Int32)
                        , db.Parameter("@OrderCode", model.OrderCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strSqlPay = @"UPDATE 
                                      `ope_wechatresult` 
                                    SET
                                      `Status` = 2,
                                      `UpdateTime` =@UpdateTime,
                                      `Updater` =@Updater 
                                    WHERE `NetTradeCode` =@NetTradeCode ";

                rows = db.SetCommand(strSqlPay
                       , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                       , db.Parameter("@Updater", model.Updater, DbType.Int32)
                       , db.Parameter("@NetTradeCode", model.NetTradeCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1)
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
