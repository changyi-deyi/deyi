using BLToolkit.Data;
using Common.Log;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace AutoComment
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始自动评价服务订单");
                string days = System.Configuration.ConfigurationManager.AppSettings["Days"];
                int result = autoCommment(days);
                if (result != 1)
                {
                    LogUtil.Log("自动评价", "自动评价服务订单失败");
                    Console.WriteLine("自动评价服务订单失败");
                    Console.ReadKey();
                }
                Console.WriteLine("自动评价服务订单成功");
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex, "自动评价服务订单失败");
                Console.WriteLine("自动评价服务订单失败");
                Console.ReadKey();
            }
        }


        public static int autoCommment(string days)
        {
            DateTime now = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                string strSqlSel = @" SELECT * FROM `Ope_ServiceOrder`
                                           WHERE `PaymentStatus` = 3
                                             AND `ServiceStatus` = 5
                                             AND `CommentStatus` = 1
                                             AND TO_DAYS(@now) - TO_DAYS(`ServiceTime`) > ";
                strSqlSel += days;
                List<OpeServiceOrder_Model> model = db.SetCommand(strSqlSel
                    , db.Parameter("@now", now, DbType.DateTime)).ExecuteList<OpeServiceOrder_Model>();

                if (model != null && model.Count > 0)
                {
                    db.BeginTransaction();
                    string strSqlIns = @" INSERT `ope_comment` (`OrderCode`, `CustomerCode`, `DoctorCode`, `Point`, `Comment`, `Overall`, `Profession`, `Altitude`, `IsSolute`, `Status`, `CreatetTime`, `Creator`)
                                           VALUES (@OrderCode, @CustomerCode, @DoctorCode, 5, '自动评价', 5,5,5,1,1,@now, 1)";

                    string strSqlUpd = @" UPDATE `Ope_ServiceOrder`
                                             SET `OrderStatus` = 2
                                                ,`CommentStatus` = 2
                                                ,`FinishTime` = @now
                                                ,`UpdateTime` = @now
                                                ,`Updater` = 1 
                                           WHERE `OrderCode` = @OrderCode";

                    foreach (OpeServiceOrder_Model item in model)
                    {
                        //插入服务评价表
                        int result = db.SetCommand(strSqlIns
                            , db.Parameter("@OrderCode", item.OrderCode, DbType.String)
                            , db.Parameter("@CustomerCode", item.CustomerCode, DbType.String)
                            , db.Parameter("@DoctorCode", item.DoctorCode, DbType.String)
                            , db.Parameter("@now", now, DbType.DateTime)).ExecuteNonQuery();
                        if (result != 1)
                        {
                            db.RollbackTransaction();
                            return 0;
                        }

                        //更新服务订单表
                        result = db.SetCommand(strSqlUpd
                            , db.Parameter("@OrderCode", item.OrderCode, DbType.String)
                            , db.Parameter("@now", now, DbType.DateTime)).ExecuteNonQuery();

                        if (result != 1)
                        {
                            db.RollbackTransaction();
                            return 0;
                        }
                    }

                    db.CommitTransaction();
                    return 1;
                }
                return 1;
            }
        }
    }
}
