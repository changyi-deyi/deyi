using Model.Operate_Model;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.Data;
using Model.View_Model;
using Common.Util;

namespace DAL
{
    public class OpeServiceOrder_DAL
    {
        #region 构造类实例
        public static OpeServiceOrder_DAL Instance
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
            internal static readonly OpeServiceOrder_DAL instance = new OpeServiceOrder_DAL();
        }

        #endregion
        public List<ServiceOrder_Model> GetServiceOrder(string CustomerCode)
        {
            using (DbManager db = new DbManager("changyi"))
            {
                string strSql = @" SELECT  A.`OrderCode` 
                                          ,A.`ServiceCode`
                                          ,A.`CustomerCode`
                                          ,A.`Name`
                                          ,A.`Mobile`
                                          ,A.`Gender`
                                          ,A.`Age`
                                          ,A.`LocatedCity`
                                          ,A.`ExpectTime`
                                          ,A.`ArrangedTime`
                                          ,A.`DoctorCode`
                                          ,A.`ReceptionistCode`
                                          ,A.`ReceptionistPhone`
                                          ,A.`Address`
                                          ,A.`SafeCode`
                                          ,A.`OrderAmount`
                                          ,A.`Unpaid`
                                          ,A.`PaymentStatus`
                                          ,A.`ServiceStatus`
                                          ,DATE_FORMAT(A.`ServiceTime`,'%Y-%m-%d') AS `ServiceTime`
                                          ,A.`OrderStatus`
                                          ,A.`FinishTime`
                                          ,A.`CommentStatus`
                                          ,A.`CommentTime`
                                          ,A.`Status`
                                          ,B.`Name` AS `DoctorName`
                                          ,B.`ImageURL`
                                          ,C.`HospitalName`
                                          ,D.`DepartmentName`
                                     FROM  `Ope_ServiceOrder` A, `Inf_Doctor` B, `Set_Hospital` C, Set_Department D
                                    WHERE  A.`Status` = 1
                                      AND  A.`PaymentStatus` = 3 
                                      AND  A.`ServiceStatus` = 5
                                      AND  A.`OrderStatus` = 3
                                      AND  A.`DoctorCode` = B.`DoctorCode`
                                      AND  B.`HospitalID` = C.`HospitalID`
                                      AND  B.`DepartmentID` = D.`DepartmentID`
                                      AND  A.`CustomerCode` = @CustomerCode
                                 ORDER BY  A.`FinishTime` DESC";
                List<ServiceOrder_Model> result = db.SetCommand(strSql, db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteList<ServiceOrder_Model>();
                return result;
            }
        }

        public List<ServiceOrder_Model> GetOrderList(string CustomerCode, int tag)
        {
            using (DbManager db = new DbManager("changyi"))
            {
                string strSql = @" SELECT  A.`OrderCode` 
                                          ,A.`ServiceCode`
                                          ,A.`CustomerCode`
                                          ,A.`Name`
                                          ,A.`Mobile`
                                          ,A.`Gender`
                                          ,A.`Age`
                                          ,A.`LocatedCity`
                                          ,A.`ExpectTime`
                                          ,A.`ArrangedTime`
                                          ,A.`DoctorCode`
                                          ,A.`ReceptionistCode`
                                          ,A.`ReceptionistPhone`
                                          ,A.`Address`
                                          ,A.`SafeCode`
                                          ,A.`OrderAmount`
                                          ,A.`Unpaid`
                                          ,A.`PaymentStatus`
                                          ,A.`ServiceStatus`
                                          ,A.`ServiceTime`
                                          ,A.`OrderStatus`
                                          ,A.`FinishTime`
                                          ,A.`CommentStatus`
                                          ,A.`CommentTime`
                                          ,A.`CouponCode`
                                          ,A.`CreatetTime`
                                          ,A.`Status`
                                          ,B.`Name` AS `DoctorName`
                                          ,B.`ImageURL`
                                          ,C.`HospitalName`
                                          ,D.`DepartmentName`
                                          ,E.`Name` AS `ServiceName`
                                          ,F.`TitleName`
                                     FROM  `Ope_ServiceOrder` A, `Inf_Doctor` B, `Set_Hospital` C, Set_Department D, Inf_Service E, Set_Title F
                                    WHERE  A.`Status` = 1
                                      AND  A.`DoctorCode` = B.`DoctorCode`
                                      AND  B.`HospitalID` = C.`HospitalID`
                                      AND  B.`DepartmentID` = D.`DepartmentID`
                                      AND  B.`TitleID` = F.`TitleID`
                                      AND  A.`ServiceCode` = E.`ServiceCode`
                                      AND  A.`CustomerCode` = @CustomerCode";
                //全部订单
                if (tag == 0)
                {
                    strSql += " AND (A.`ServiceStatus` <> 9 OR A.`OrderStatus` <> 3 ) ";
                }
                //服务中
                else if (tag == 1)
                {
                    strSql += " AND (A.`ServiceStatus` = 1 OR A.`ServiceStatus` = 2 OR A.`ServiceStatus` = 3 OR A.`ServiceStatus` = 4) AND A.`PaymentStatus` = 3 AND A.`OrderStatus` = 1 AND A.`CommentStatus` = 1 ";
                }
                //待支付
                else if (tag == 2)
                {
                    strSql += " AND A.`PaymentStatus` = 1 AND A.`OrderStatus` = 1 ";
                }
                //待评价
                else if (tag == 3)
                {
                    strSql += " AND A.`PaymentStatus` = 3 AND A.`ServiceStatus` = 4 AND A.`CommentStatus` = 1 ";
                }
                //退款/售后
                else if (tag == 4)
                {
                    strSql += " AND A.`OrderStatus` = 4 ";
                }

                strSql += " ORDER BY  A.`CreatetTime` DESC ";
                List<ServiceOrder_Model> result = db.SetCommand(strSql, db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteList<ServiceOrder_Model>();
                return result;
            }
        }

        public OpeServiceOrder_Model GetCommentStatus(string OrderCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT `CommentStatus` FROM `Ope_ServiceOrder` WHERE `OrderCode` = @OrderCode AND `Status` = 1 ";
                OpeServiceOrder_Model result = db.SetCommand(strSql, db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteObject<OpeServiceOrder_Model>();

                return result;
            }
        }

        public int OrderCancel(string OrderCode, int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `PaymentStatus`
                                          ,`ServiceStatus`
                                          ,`OrderStatus`
                                          ,`CouponCode`
                                     FROM  `Ope_ServiceOrder`
                                    WHERE  `OrderCode` = @OrderCode 
                                      AND  `Status` = 1 ";
                OpeServiceOrder_Model model = db.SetCommand(strSql, db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteObject<OpeServiceOrder_Model>();
                //数据不存在
                if (model == null)
                {
                    return 2;
                }
                //条件不符合取消
                if (model.PaymentStatus != 1 || model.ServiceStatus != 1 || model.OrderStatus != 1)
                {
                    return 3;
                }
                db.BeginTransaction();
                string strUpd = @" UPDATE  `Ope_ServiceOrder`
                                      SET  `ServiceStatus` = 9
                                          ,`OrderStatus` = 3 
                                          ,`UpdateTime` = @now
                                          ,`Updater` = @UserID
                                    WHERE  `OrderCode` = @OrderCode ";

                int row = db.SetCommand(strUpd
                    , db.Parameter("@OrderCode", OrderCode, DbType.String)
                    , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                if (!string.IsNullOrEmpty(model.CouponCode))
                {
                    string strUpdCoupon = @" UPDATE  `ope_customercoupon`
                                                SET  `Status` = 1 
                                                    ,`UpdateTime` = @now
                                                    ,`Updater` = @UserID
                                              WHERE  `CouponCode` = @CouponCode ";
                    row = db.SetCommand(strUpdCoupon
                            , db.Parameter("@OrderCode", OrderCode, DbType.String)
                            , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                            , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                    if (row <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                db.CommitTransaction();
                return 1;
            }
        }

        public ServiceOrder_Model GetOrderDetail(string OrderCode)
        {
            using (DbManager db = new DbManager("changyi"))
            {
                string strSql = @" SELECT  A.`OrderCode` 
                                          ,A.`ServiceCode`
                                          ,A.`CustomerCode`
                                          ,A.`Name`
                                          ,A.`Mobile`
                                          ,A.`Gender`
                                          ,A.`Age`
                                          ,A.`LocatedCity`
                                          ,A.`ExpectTime`
                                          ,A.`ArrangedTime`
                                          ,A.`DoctorCode`
                                          ,A.`ReceptionistCode`
                                          ,A.`ReceptionistPhone`
                                          ,A.`Address`
                                          ,A.`SafeCode`
                                          ,A.`OrderAmount`
                                          ,A.`Unpaid`
                                          ,A.`PaymentStatus`
                                          ,A.`ServiceStatus`
                                          ,A.`ServiceTime`
                                          ,A.`OrderStatus`
                                          ,A.`FinishTime`
                                          ,A.`CommentStatus`
                                          ,A.`CommentTime`
                                          ,A.`CreatetTime`
                                          ,A.`Status`
                                          ,B.`Name` AS `DoctorName`
                                          ,B.`ImageURL`
                                          ,C.`HospitalName`
                                          ,D.`DepartmentName`
                                          ,E.`Name` AS `ServiceName`
                                          ,F.`TitleName`
                                     FROM  `Ope_ServiceOrder` A, `Inf_Doctor` B, `Set_Hospital` C, Set_Department D, Inf_Service E, Set_Title F
                                    WHERE  A.`Status` = 1
                                      AND  A.`DoctorCode` = B.`DoctorCode`
                                      AND  B.`HospitalID` = C.`HospitalID`
                                      AND  B.`DepartmentID` = D.`DepartmentID`
                                      AND  B.`TitleID` = F.`TitleID`
                                      AND  A.`ServiceCode` = E.`ServiceCode`
                                      AND  A.`OrderCode` = @OrderCode";
                ServiceOrder_Model result = db.SetCommand(strSql, db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteObject<ServiceOrder_Model>();
                return result;
            }
        }

        public int AfterService(string OrderCode, string Reason, int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `PaymentStatus`
                                          ,`ServiceStatus`
                                          ,`OrderStatus`
                                     FROM  `Ope_ServiceOrder`
                                    WHERE  `OrderCode` = @OrderCode 
                                      AND  `Status` = 1 ";
                OpeServiceOrder_Model model = db.SetCommand(strSql, db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteObject<OpeServiceOrder_Model>();
                //数据不存在
                if (model == null)
                {
                    return 2;
                }
                //条件不符合取消
                if (model.PaymentStatus == 3 && (model.ServiceStatus == 1 || model.ServiceStatus == 2))
                {
                    string strUpd = @" UPDATE  `Ope_ServiceOrder`
                                          SET  `OrderStatus` = 4
                                              ,`PaymentStatus` = 4 
                                              ,`Reason` = @Reason
                                              ,`UpdateTime` = @now
                                              ,`Updater` = @UserID
                                        WHERE  `OrderCode` = @OrderCode ";

                    int row = db.SetCommand(strUpd
                        , db.Parameter("@OrderCode", OrderCode, DbType.String)
                        , db.Parameter("@Reason", Reason, DbType.String)
                        , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                        , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                    if (row <= 0)
                    {
                        return 0;
                    }
                }
                else
                {
                    return 3;
                }

                return 1;
            }
        }

        public AddServiceOrderResult_Model AddServiceOrder(AddServiceOrder_Model model) {

            using (DbManager db = new DbManager())
            {
                DateTime dt = DateTime.Now.ToLocalTime();
                AddServiceOrderResult_Model result = new AddServiceOrderResult_Model();
                result.Success = false;
                result.Message = "系统错误";

                db.BeginTransaction();
                int rows = 0;
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

                    string strSqlCoupon = @" UPDATE 
                                      `Ope_CustomerCoupon` 
                                    SET
                                      `IsUsed` =2,
                                      `UpdateTime` =@UpdateTime,
                                      `Updater` =@Updater
                                    WHERE `CouponCode` =@CouponCode
                                      AND `CustomerCode` =@CustomerCode";

                    rows = db.SetCommand(strSqlCoupon
                                , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                                , db.Parameter("@Updater", model.UserID, DbType.Int32)
                                , db.Parameter("@CouponCode", model.CouponCode, DbType.String)
                                , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return result;
                    }
                }
                #endregion

                #region 判断服务

                string strSqlService = @" SELECT * FROM `inf_service` WHERE `ServiceCode` =@ServiceCode";
                InfService_Model Service = db.SetCommand(strSqlService
                     , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)).ExecuteObject<InfService_Model>();

                if (Service == null || Service.Status == 2)
                {
                    db.RollbackTransaction();
                    result.Message = "服务已无效";
                    return result;
                }


                string strSqlDoctorService = @" SELECT * FROM `inf_doctorservice` WHERE `ServiceCode` =@ServiceCode AND `DoctorCode` =@DoctorCode";
                InfDoctorService_Model DoctorService = db.SetCommand(strSqlDoctorService
                     , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)
                     , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)).ExecuteObject<InfDoctorService_Model>();

                if (DoctorService == null || DoctorService.Status == 2)
                {
                    db.RollbackTransaction();
                    result.Message = "服务已无效";
                    return result;
                }
                //非会员价
                if (!model.DiscountFlg)
                {
                    if (DoctorService.Price != model.OriginAmount && model.AmountType == 1)
                    {
                        db.RollbackTransaction();
                        result.Message = "价格发生变化，请重新提交";
                        return result;
                    }
                }
                //会员价
                else
                {
                    string strSqlRow = @" SELECT COUNT(1) FROM `Inf_MemberServiceDiscount` WHERE `LevelID` = @LevelID AND `ServiceCode` = @ServiceCode AND `Status` = 1";

                    int row = db.SetCommand(strSqlRow
                    , db.Parameter("@LevelID", model.LevelID, DbType.Int32)
                    , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)).ExecuteScalar<int>();

                    if (row <= 0)
                    {
                        db.RollbackTransaction();
                        result.Message = "价格发生变化，请重新提交";
                        return result;
                    }
                    string strSql = @" SELECT `Discount` FROM `Inf_MemberServiceDiscount` WHERE `LevelID` = @LevelID AND `ServiceCode` = @ServiceCode AND `Status` = 1";

                    InfMemberServiceDiscount_Model discount = db.SetCommand(strSql
                        , db.Parameter("@LevelID", model.LevelID, DbType.Int32)
                        , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)).ExecuteObject<InfMemberServiceDiscount_Model>();
                    
                    if ((DoctorService.Price* discount.Discount) != model.OriginAmount && model.AmountType == 1)
                    {
                        db.RollbackTransaction();
                        result.Message = "价格发生变化，请重新提交";
                        return result;
                    }

                }

                #endregion


                var outParmeter1 = db.OutputParameter("@Result", DbType.String);
                var sf1 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                  , db.Parameter("@TN", "ope_serviceorder", DbType.String)
                                  , outParmeter1).ExecuteScalar<string>();
                string OrderCode = outParmeter1.Value.ToString();

                if (string.IsNullOrEmpty(OrderCode))
                {
                    db.RollbackTransaction();
                    return result;
                }

                string strSqlOrder = @" INSERT INTO `ope_serviceorder` 
                            (`OrderCode`,`ServiceCode`,DoctorCode,`CustomerCode`,`Name`,`Mobile`,`Gender`,`Age`,`LocatedCity`,`Description`,`ExpectTime`,`OrderAmount`,`Unpaid`,`PaymentStatus`,`ServiceStatus`,`OrderStatus`,`CommentStatus`,`Status`,`CreatetTime`,`Creator`,`AmountType`,`DepositAmount`,`CouponCode`,`OriginAmount`,`DiscountAmount`) 
                            VALUES
                            (@OrderCode,@ServiceCode,@DoctorCode,@CustomerCode,@Name,@Mobile,@Gender,@Age,@LocatedCity,@Description,@ExpectTime,@OrderAmount,@Unpaid,1,1,1,1,1,@CreatetTime,@Creator,@AmountType,@DepositAmount,@CouponCode,@OriginAmount,@DiscountAmount) ;";

                 rows = db.SetCommand(strSqlOrder
                                  , db.Parameter("@OrderCode", OrderCode, DbType.String)
                                  , db.Parameter("@ServiceCode", model.ServiceCode, DbType.String)
                                  , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)
                                  , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                                  , db.Parameter("@Name", model.Name, DbType.String)
                                  , db.Parameter("@Mobile", model.Mobile, DbType.String)
                                  , db.Parameter("@Gender", model.Gender, DbType.Int32)
                                  , db.Parameter("@Age", model.Age, DbType.Int32)
                                  , db.Parameter("@LocatedCity", model.LocatedCity, DbType.String)
                                  , db.Parameter("@Description", model.Description, DbType.String)
                                  , db.Parameter("@ExpectTime", model.ExpectTime, DbType.DateTime)
                                  , db.Parameter("@OrderAmount", DoctorService.IsBargain == 1 ? -1 : model.OrderAmount, DbType.Decimal)
                                  , db.Parameter("@Unpaid", DoctorService.IsBargain == 1 ? -1 : model.OrderAmount, DbType.Decimal)
                                  , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                                  , db.Parameter("@Creator", model.UserID, DbType.Int32)
                                  , db.Parameter("@AmountType", DoctorService.IsBargain, DbType.Int32)
                                  , db.Parameter("@DepositAmount", model.DepositAmount, DbType.Decimal)
                                  , db.Parameter("@CouponCode", model.CouponCode, DbType.String)
                                  , db.Parameter("@OriginAmount", model.OriginAmount, DbType.Decimal)
                                  , db.Parameter("@DiscountAmount", model.DiscountAmount, DbType.Decimal)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return result;
                }
                
                result.OrderCode = OrderCode;
                result.PayAmount = DoctorService.IsBargain == 1 ? -1 : model.OrderAmount;
                result.Success = true;

                db.CommitTransaction();
                return result;
            }
        }


        public AddServiceOrderResult_Model PayServiceOrder(AddServiceOrder_Model model)
        {

            using (DbManager db = new DbManager())
            {
                DateTime dt = DateTime.Now.ToLocalTime();
                AddServiceOrderResult_Model result = new AddServiceOrderResult_Model();
                result.Success = false;
                result.Message = "系统错误";

                db.BeginTransaction();

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

                int rows = db.SetCommand(strSqlWeChat
                                  , db.Parameter("@NetTradeCode", NetTradeCode, DbType.String)
                                  , db.Parameter("@OrderCode", model.OrderCode, DbType.String)
                                  , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                                  , db.Parameter("@Creator", model.UserID, DbType.Int32)
                                  , db.Parameter("@Amount", model.OrderAmount, DbType.Decimal)).ExecuteNonQuery();

                if (rows != 1)
                {
                    db.RollbackTransaction();
                    return result;
                }

                result.OrderCode = model.OrderCode;
                result.NetTradeCode = NetTradeCode;
                result.PayAmount = model.PayAmount;
                result.Success = true;


                db.CommitTransaction();
                return result;
            }
        }


        public int UpdatePayServiceOrderResult(string NetTradeCode, string Data, int mode)
        {
            using (DbManager db = new DbManager())
            {
                DateTime dt = DateTime.Now.ToLocalTime();
                db.BeginTransaction();
                string strSqlOrder = @" SELECT a.`Type`,a.`OrderCode`,b.`CustomerCode`,b.`OrderAmount`,a.`PayMentCode`,a.`Amount`,b.CouponCode,b.DiscountAmount,c.`IDNumber` FROM `ope_wechatresult` a 
                                INNER JOIN `ope_serviceorder` b
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
                          `ope_serviceorder` 
                        SET
                          `PaymentStatus` = 3,
                          `PaymentTime` =@PaymentTime,
                          `UpdateTime` =@UpdateTime,
                          `Updater` =@Updater,
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
                                      , db.Parameter("@OrderCode", DetailCode2, DbType.String)
                                      , db.Parameter("@MODE", 3, DbType.Int32)
                                      , db.Parameter("@PaidAmount", PaymentResult.DiscountAmount, DbType.Decimal)
                                      , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                                      , db.Parameter("@Creator", PaymentResult.Creator, DbType.Int32)).ExecuteNonQuery();

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

    }
}
