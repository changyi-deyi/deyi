using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;
using BLToolkit.Data;

namespace DAL
{
    public class ImaCustomerProfile_DAL
    {
        #region 构造类实例
        public static ImaCustomerProfile_DAL Instance
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
            internal static readonly ImaCustomerProfile_DAL instance = new ImaCustomerProfile_DAL();
        }

        #endregion
        public List<ImaCustomerProfile_Model> GetProfileIma(int ID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `ID`
                                          ,`Path`
                                          ,`FileName`
                                          ,`Status`
                                    FROM  `Ima_CustomerProfile`
                                   WHERE  `Status` = 1 
                                     AND  `ID` = @ID ";
                List<ImaCustomerProfile_Model> list = db.SetCommand(strSql
                    , db.Parameter("@ID", ID, DbType.Int32)).ExecuteList<ImaCustomerProfile_Model>();
                return list;
            }
        }
        public int DeleteProfileIma(int ID, int UserID)
        {
            DateTime nowtime = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE   `Ima_CustomerProfile`
                                      SET   `Status` = 2
                                           ,`UpdateTime` = @UpdateTime
                                           ,`Updater` = @Updater
                                    WHERE   `ID` = @ID ";
                int row = db.SetCommand(strSql
                    , db.Parameter("@UpdateTime", nowtime, DbType.DateTime)
                    , db.Parameter("@Updater", UserID, DbType.Int32)
                    , db.Parameter("@ID", ID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}
