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
    public class OrderM_DAL
    {
        #region 构造类实例
        public static OrderM_DAL Instance
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
            internal static readonly OrderM_DAL instance = new OrderM_DAL();
        }

        #endregion

        public List<MemberOrder_Model> getMemberOrderList (string CustomerCode, string CustomerName, string StartDate, string EndDate ,int PaymentStatus ,int OrderStatus, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.Name as CustomerName FROM `ope_memberorder` a 
                                LEFT JOIN `inf_customer` b
                                ON a.`CustomerCode` = b.`CustomerCode` 
                                WHERE a.status = 1   {0}  ORDER BY a.`CreatetTime` DESC  LIMIT @StartCount,@EndCount ";

                 string strWhere = "";
                if (!string.IsNullOrEmpty(CustomerCode))
                {
                    strWhere += " AND a.`CustomerCode` = @CustomerCode ";

                }
                if (!string.IsNullOrEmpty(CustomerName))
                {
                    strWhere += " AND b.`Name` LIKE @CustomerName ";

                }
                if (!string.IsNullOrEmpty(StartDate))
                {
                    strWhere += " AND a.`CreatetTime` >= @StartDate ";

                }
                if (!string.IsNullOrEmpty(EndDate))
                {
                    strWhere += " AND a.`CreatetTime` <= @EndDate ";

                }

                if (PaymentStatus > 0) {
                    strWhere += " AND a.`PaymentStatus` = @PaymentStatus ";
                }



                if (OrderStatus > 0)
                {
                    strWhere += " AND a.`OrderStatus` = @OrderStatus ";
                }

                strSql = string.Format(strSql, strWhere);

                List<MemberOrder_Model> result = db.SetCommand(strSql
                                , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                                , db.Parameter("@CustomerName", "%" + CustomerName + "%", DbType.String)
                                , db.Parameter("@StartDate", StartDate, DbType.String)
                                , db.Parameter("@EndDate", EndDate, DbType.String)
                                , db.Parameter("@PaymentStatus", PaymentStatus, DbType.Int32)
                                , db.Parameter("@OrderStatus", OrderStatus, DbType.Int32)
                                , db.Parameter("@StartCount", StartCount, DbType.Int32)
                                , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<MemberOrder_Model>();



                return result;
            }
        }

        public MemberOrder_Model getMemberOrderDetail(string OrderCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.Name AS CustomerName,c.`Name` AS LevelName FROM `ope_memberorder` a 
                                LEFT JOIN `inf_customer` b
                                ON a.`CustomerCode` = b.`CustomerCode` 
				                LEFT JOIN `set_memberlevel` c 
				                ON a.`MemberLevelID` = c.`LevelID`
                                WHERE a.OrderCode =@OrderCode";

                MemberOrder_Model  result = db.SetCommand(strSql
                                , db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteObject<MemberOrder_Model>();


                if (result != null)
                {
                    string strSqlPayment = " SELECT * FROM `ope_payment` WHERE `OrderCode` =@OrderCode";
                    result.listPayment = new List<Payment_Model>();
                    result.listPayment = db.SetCommand(strSqlPayment
                                , db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteList<Payment_Model>();

                    if (result.listPayment != null && result.listPayment.Count > 0)
                    {
                        string strSqlDetail = " SELECT * FROM `ope_paymentdetail` WHERE `PayMentCode`=@PayMentCode";
                        foreach (Payment_Model item in result.listPayment)
                        {
                            item.listPaymentDetail = new List<PaymentDetail_Model>();
                            item.listPaymentDetail = db.SetCommand(strSqlDetail
                                , db.Parameter("@PayMentCode", item.PayMentCode, DbType.String)).ExecuteList<PaymentDetail_Model>();
                        }
                    }

                }



                return result;
            }
        }

        public int CancelMemberOrder(MemberOrder_Model model) {
            using (DbManager db = new DbManager())
            {
                string strSql = @" update Ope_MemberOrder  set 
                                    PaymentStatus=@PaymentStatus,
                                    OrderStatus=@OrderStatus,
                                    Updater=@Updater,
                                    UpdateTime=@UpdateTime 
                                    where OrderCode=@OrderCode ";

                int rows = db.SetCommand(strSql
                                 , db.Parameter("@PaymentStatus", 1, DbType.Int32)
                                 , db.Parameter("@OrderStatus", 3, DbType.Int32)
                                 , db.Parameter("@Updater", model.Updater, DbType.Int32)
                                 , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                                 , db.Parameter("@OrderCode", model.OrderCode, DbType.String)).ExecuteNonQuery();

                if (rows != 1) {
                    return 0;
                }

                return 1;
            }
        }

        public List<ServiceOrder_Model> getServiceOrderList(string CustomerName, string DoctorName, string ServiceName, int PaymentStatus, int ServiceStatus, int OrderStatus, string StartDate, string EndDate, string CustomerCode, int StartCount, int EndCount) {

            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.`Name` AS CustomerName,c.`Name` AS DoctorName,d.`Name` AS ServiceName  FROM `ope_serviceorder` a
                                LEFT JOIN `inf_customer` b 
                                ON a.`CustomerCode` = b.`CustomerCode`
                                LEFT JOIN `inf_doctor` c
                                ON a.`DoctorCode` = c.`DoctorCode` 
                                LEFT JOIN `inf_service` d
                                ON a.`ServiceCode` = d.`ServiceCode`
                                WHERE a.`Status` = 1 {0} ORDER BY a.`CreatetTime` DESC  LIMIT @StartCount,@EndCount ";

                string strWhere = "";
                if (!string.IsNullOrEmpty(CustomerName))
                {
                    strWhere += " AND b.`Name` LIKE @CustomerName ";

                }
                if (!string.IsNullOrEmpty(DoctorName))
                {
                    strWhere += " AND c.`Name` LIKE @DoctorName ";

                }
                if (!string.IsNullOrEmpty(ServiceName))
                {
                    strWhere += " AND c.`Name` LIKE @ServiceName ";

                }


                if (!string.IsNullOrEmpty(StartDate))
                {
                    strWhere += " AND a.`CreatetTime` >= @StartDate ";

                }
                if (!string.IsNullOrEmpty(EndDate))
                {
                    strWhere += " AND a.`CreatetTime` <= @EndDate ";

                }

                if (PaymentStatus > 0)
                {
                    strWhere += " AND a.`PaymentStatus` = @PaymentStatus ";
                }



                if (OrderStatus > 0)
                {
                    strWhere += " AND a.`OrderStatus` = @OrderStatus ";
                }

                if (ServiceStatus > 0)
                {
                    strWhere += " AND a.`ServiceStatus` = @ServiceStatus ";
                }

                strSql = string.Format(strSql, strWhere);

                List<ServiceOrder_Model> result = db.SetCommand(strSql
                                , db.Parameter("@CustomerName", "%" + CustomerName + "%", DbType.String)
                                , db.Parameter("@DoctorName", "%" + DoctorName + "%", DbType.String)
                                , db.Parameter("@ServiceName", "%" + ServiceName + "%", DbType.String)
                                , db.Parameter("@StartDate", StartDate, DbType.String)
                                , db.Parameter("@EndDate", EndDate, DbType.String)
                                , db.Parameter("@PaymentStatus", PaymentStatus, DbType.Int32)
                                , db.Parameter("@OrderStatus", OrderStatus, DbType.Int32)
                                , db.Parameter("@ServiceStatus", ServiceStatus, DbType.Int32)
                                , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                                , db.Parameter("@StartCount", StartCount, DbType.Int32)
                                , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<ServiceOrder_Model>();



                return result;
            }
        }




        public ServiceOrder_Model getServiceOrderDetail(string OrderCode)
        {

            using (DbManager db = new DbManager())
            {
                string strSql = @"SELECT a.*,b.`Name` AS CustomerName,c.`Name` AS DoctorName,d.`Name` AS ServiceName,e.`Name` AS ReceptionistName  FROM `ope_serviceorder` a
                            LEFT JOIN `inf_customer` b 
                            ON a.`CustomerCode` = b.`CustomerCode`
                            LEFT JOIN `inf_doctor` c
                            ON a.`DoctorCode` = c.`DoctorCode` 
                            LEFT JOIN `inf_service` d
                            ON a.`ServiceCode` = d.`ServiceCode`
                            LEFT JOIN `inf_staff` e
                            ON a.`ReceptionistCode` = e.`StaffCode` 
                            WHERE a.`OrderCode` =@OrderCode ";



                ServiceOrder_Model result = db.SetCommand(strSql
                                , db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteObject<ServiceOrder_Model>();


                if (result != null) {
                    string strSqlPayment = " SELECT * FROM `ope_payment` WHERE `OrderCode` =@OrderCode";
                    result.listPayment = new List<Payment_Model>();
                    result.listPayment = db.SetCommand(strSqlPayment
                                , db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteList<Payment_Model>();

                    if (result.listPayment != null && result.listPayment.Count > 0) {
                        string strSqlDetail = " SELECT * FROM `ope_paymentdetail` WHERE `PayMentCode`=@PayMentCode";
                        foreach (Payment_Model item in result.listPayment)
                        {
                            item.listPaymentDetail = new List<PaymentDetail_Model>();
                            item.listPaymentDetail = db.SetCommand(strSqlDetail
                                , db.Parameter("@PayMentCode", item.PayMentCode, DbType.String)).ExecuteList<PaymentDetail_Model>();
                        }
                    }


                    string strSqlcomment = @" SELECT * FROM `ope_comment` WHERE `Status` = 1 AND `OrderCode`=@OrderCode";

                    result.comment = new Model.Table_Model.OpeComment_Model();
                    result.comment = db.SetCommand(strSqlcomment
                                , db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteObject<Model.Table_Model.OpeComment_Model>();

                }



                return result;
            }
        }

        public int ReplyServiceOrder(ServiceOrder_Model model) {
            using (DbManager db = new DbManager())
            {
                string strSql = @"UPDATE 
                              `ope_serviceorder` 
                            SET
                              `SafeCode` =@SafeCode,
                              `ArrangedTime` =@ArrangedTime,
                              `DoctorCode` =@DoctorCode,
                              `Address` =@Address,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater,
                              `ReceptionistCode` =@ReceptionistCode,
                              `ReceptionistPhone` =@ReceptionistPhone
                            WHERE `OrderCode` =@OrderCode ";



                int rows = db.SetCommand(strSql
                                , db.Parameter("@OrderCode", model.OrderCode, DbType.String)
                                , db.Parameter("@SafeCode", model.SafeCode, DbType.String)
                                , db.Parameter("@ArrangedTime", model.ArrangedTime, DbType.String)
                                , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)
                                , db.Parameter("@Address", model.Address, DbType.String)
                                , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                                , db.Parameter("@Updater", model.Updater, DbType.Int32)
                                , db.Parameter("@ReceptionistCode", model.ReceptionistCode, DbType.String)
                                , db.Parameter("@ReceptionistPhone", model.ReceptionistPhone, DbType.String)).ExecuteNonQuery();

                if (rows != 1) {
                    return 0;
                }



                return 1;
            }
        }


        public int UpdateServiceOrderAmount(ServiceOrder_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @"UPDATE 
                              `ope_serviceorder` 
                            SET
                              `OrderAmount` =@OrderAmount,
                              `Unpaid` =@Unpaid,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater
                            WHERE `OrderCode` =@OrderCode ";



                int rows = db.SetCommand(strSql
                                , db.Parameter("@OrderCode", model.OrderCode, DbType.String)
                                , db.Parameter("@OrderAmount", model.OrderAmount, DbType.Decimal)
                                , db.Parameter("@Unpaid", model.OrderAmount, DbType.Decimal)
                                , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                                , db.Parameter("@Updater", model.Updater, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }



                return 1;
            }
        }



        public int UpdateServiceOrderStatus(ServiceOrder_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @"UPDATE 
                              `ope_serviceorder` 
                            SET
                              `ServiceStatus` =@ServiceStatus,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater
                            WHERE `OrderCode` =@OrderCode ";



                int rows = db.SetCommand(strSql
                                , db.Parameter("@OrderCode", model.OrderCode, DbType.String)
                                , db.Parameter("@ServiceStatus", model.ServiceStatus, DbType.Int32)
                                , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                                , db.Parameter("@Updater", model.Updater, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }



                return 1;
            }
        }


        public List<string> getNoResultNetTradeNoByOrder(string OrderCode) {
            using (DbManager db = new DbManager())
            {
                string strSql = @"SELECT `NetTradeCode` FROM `ope_wechatresult` WHERE `OrderCode`=@OrderCode AND STATUS = 1 AND DATA IS  NULL  ";



                List<string> result = db.SetCommand(strSql
                                , db.Parameter("@OrderCode", OrderCode, DbType.String)).ExecuteScalarList<string>();

             

                return result;
            }
        }



    }
}
