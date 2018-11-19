using BLToolkit.Data;
using Common.Log;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace UnsignIn
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始更新签到状态");
                int result = UnsignIn();
                if (result != 1)
                {
                    LogUtil.Log("签到状态", "签到状态更新失败");
                    Console.WriteLine("签到状态更新失败");
                    Console.ReadKey();
                }
                Console.WriteLine("签到状态更新成功");
            }
            catch(Exception ex)
            {
                LogUtil.Log(ex, "签到状态更新失败");
                Console.WriteLine("签到状态更新失败");
                Console.ReadKey();
            }
        }


        public static int UnsignIn()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE `inf_customer`
                                      SET `SignStatus` = 1
                                         ,`UpdateTime` = @now
                                         ,`Updater` = 1 ";

                int result = db.SetCommand(strSql
                    , db.Parameter("@now", DateTime.Now, DbType.DateTime)).ExecuteNonQuery();

                if (result <= 0)
                {
                    return 0;
                }

                return 1;
            }
        }
    }
}
