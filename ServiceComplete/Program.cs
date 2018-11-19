using BLToolkit.Data;
using Common.Log;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceInProgress
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始更新服务状态");
                int result = SerciveCheck();
                if (result != 1)
                {
                    LogUtil.Log("服务状态变更", "服务状态变更（服务完成）更新失败");
                    Console.WriteLine("服务状态变更（服务完成）更新失败");
                    Console.ReadKey();
                }
                Console.WriteLine("服务状态变更成功");
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex, "服务状态变更（服务完成）更新失败");
                Console.WriteLine("服务状态变更（服务完成）更新失败");
                Console.ReadKey();
            }
        }

        public static int SerciveCheck()
        {
            using (DbManager db = new DbManager())
            {
                DateTime now = DateTime.Now;
                string strSqlSel = @" UPDATE `Ope_ServiceOrder`
                                         SET `ServiceStatus` = 5
                                            ,`UpdateTime` = @now
                                            ,`Updater` = 1
                                       WHERE `Status` = 1 
                                         AND `ServiceStatus` = 4
                                         AND `PaymentStatus` = 3
                                         AND TO_DAYS(@now) - TO_DAYS(`ServiceTime`) = 1 ";

                int result = db.SetCommand(strSqlSel
                     , db.Parameter("@now", now, DbType.DateTime)).ExecuteNonQuery();

                if (result < 0)
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}
