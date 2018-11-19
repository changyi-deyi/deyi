using BLToolkit.Data;
using Common.Log;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;

namespace MemberCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始更新会员状态");
                int result = MemberCheck();
                if (result != 1)
                {
                    LogUtil.Log("会员状态", "会员状态更新失败");
                    Console.Write("会员状态更新失败");
                    Console.ReadKey();
                }
                Console.WriteLine("会员状态更新成功");
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex, "会员状态更新失败");
                Console.WriteLine("会员状态更新失败");
                Console.ReadKey();
            }
        }
        
        public static int MemberCheck()
        {
            using (DbManager db = new DbManager())
            {
                DateTime now = DateTime.Now;
                string strSqlSel = @" SELECT `CustomerCode`
                                     FROM `inf_member` 
                                    WHERE DATE_FORMAT(`ExpiredDate`,'%Y%m%d') < @now
                                      AND `Status` = 1 ";

                List<InfMember_Model> model = db.SetCommand(strSqlSel
                    , db.Parameter("@now", now.ToString("yyyyMMdd"), DbType.String)).ExecuteList<InfMember_Model>();

                if (model != null && model.Count > 0)
                {
                    db.BeginTransaction();
                    string strUpd = @" UPDATE `inf_customer`
                                          SET `IsMember` = 1
                                             ,`UpdateTime` = @now
                                             ,`Updater` = 1
                                        WHERE `Status` = 1 ";
                    int index = 1;
                    foreach (InfMember_Model item in model)
                    {
                        if (index == 1)
                        {
                            strUpd += " AND `CustomerCode` IN ('" + item.CustomerCode + "'";
                            index++;
                        }
                        else
                        {
                            strUpd += ",'" + item.CustomerCode + "'";
                        }
                    }
                    strUpd += ") ";

                    int result = db.SetCommand(strUpd
                        , db.Parameter("@now", DateTime.Now, DbType.DateTime)).ExecuteNonQuery();
                    if (result < 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                    db.CommitTransaction();
                    return 1;
                }

                return 1;
            }
        }
    }
}
