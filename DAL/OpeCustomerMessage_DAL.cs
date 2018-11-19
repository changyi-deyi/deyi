using Model.Operate_Model;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLToolkit.Data;

namespace DAL
{
    public class OpeCustomerMessage_DAL
    {
        #region 构造类实例
        public static OpeCustomerMessage_DAL Instance
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
            internal static readonly OpeCustomerMessage_DAL instance = new OpeCustomerMessage_DAL();
        }

        #endregion
        public List<OpeCustomerMessage_Model> GetMessage(CustomerMessage_Model model)
        {
            using (DbManager db = new DbManager("changyi"))
            {
                db.BeginTransaction();
                string strSqlUpd = @" UPDATE `Ope_CustomerMessage`
                                         SET `IsRead` = 2
                                       WHERE `CustomerCode` = @CustomerCode
                                         AND `Status` = 1 ";
                int row = db.SetCommand(strSqlUpd
                    , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteNonQuery();

                if (row < 0)
                {
                    db.RollbackTransaction();
                    return null;
                }

                string strSql = @" SELECT    A.`ID`
                                            ,A.`CustomerCode`
                                            ,A.`Type`
                                            ,A.`Message`
                                            ,A.`URL`
                                            ,A.`SendTime`
                                            ,A.`IsRead`
                                            ,A.`ReadTime`
                                            ,A.`Status`
                                    FROM     `Ope_CustomerMessage` A
                                    WHERE    A.`Status` = 1 
                                      AND    A.`CustomerCode` = @CustomerCode
                                 ORDER BY    A.`SendTime` DESC ";
                List<OpeCustomerMessage_Model> result = db.SetCommand(strSql, db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteList<OpeCustomerMessage_Model>();

                db.CommitTransaction();
                return result;
            }
        }

        public int DeleteMessage(MessageDelete_Model model)
        {
            DateTime nowtime = DateTime.Now;
            using (DbManager db = new DbManager("changyi"))
            {
                string strSql = @" UPDATE   `Ope_CustomerMessage`
                                      SET   `Status` = 2
                                           ,`UpdateTime` = @UpdateTime
                                           ,`Updater` = @Updater
                                    WHERE   `ID` = @ID ";
                int row = db.SetCommand(strSql
                    , db.Parameter("@UpdateTime", nowtime, DbType.DateTime)
                    , db.Parameter("@Updater", model.UserID, DbType.Int32)
                    , db.Parameter("@ID", model.MessageID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    return 0;
                }
                return 1;
            }
        }

        public int GetReadMessage(CustomerMessage_Model model)
        {
            using (DbManager db = new DbManager("changyi"))
            {
                string strSql = @" SELECT    COUNT(1)
                                    FROM     `Ope_CustomerMessage`
                                    WHERE    `Status` = 1 
                                      AND    `CustomerCode` = @CustomerCode
                                      AND    `IsRead` = 1";
                int rows = db.SetCommand(strSql, db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteScalar<int>();
                return rows;
            }
        }

    }
}
