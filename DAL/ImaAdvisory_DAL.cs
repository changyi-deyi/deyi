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
    public class ImaAdvisory_DAL
    {
        #region 构造类实例
        public static ImaAdvisory_DAL Instance
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
            internal static readonly ImaAdvisory_DAL instance = new ImaAdvisory_DAL();
        }

        #endregion
        public List<ImaAdvisory_Model> GetAdvisoryImage(int GroupID,string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlGroup = @" SELECT `ID`
                                          FROM `Ope_Advisory`
                                         WHERE `GroupID` = @GroupID 
                                           AND `CustomerCode` = @CustomerCode
                                           AND `Status` = 1 ";
                List<int> ListID = db.SetCommand(strSqlGroup
                    , db.Parameter("@GroupID", GroupID, DbType.Int32)
                    , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteScalarList<int>();

                if (ListID != null && ListID.Count > 0)
                {
                    string strSql = @" SELECT  `ID` 
                                              ,`Path`
                                              ,`FileName`
                                         FROM  `Ima_Advisory` 
                                        WHERE  `Status` = 1 ";
                    int index = 1;
                    foreach (int id in ListID)
                    {
                        if (index == 1)
                        {
                            strSql += " AND `ID` IN (" + id;
                            index++;
                        }
                        else
                        {
                            strSql += ", " + id;
                        }
                    }
                    strSql += ")";
                    List<ImaAdvisory_Model> result = db.SetCommand(strSql).ExecuteList<ImaAdvisory_Model>();
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public int DeleteAdvisoryImage(int ID,int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE `Ima_Advisory`
                                      SET `Status` = 2
                                         ,`UpdateTime` = @now
                                         ,`Updater` = @UserID
                                    WHERE `ID` = @ID ";
                int row = db.SetCommand(strSql
                     , db.Parameter("@ID", ID, DbType.Int32)
                     , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                     , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}
