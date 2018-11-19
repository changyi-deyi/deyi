using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;
using BLToolkit.Data;
using Model.Operate_Model;

namespace DAL
{
    public class InfFamily_DAL
    {
        #region 构造类实例
        public static InfFamily_DAL Instance
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
            internal static readonly InfFamily_DAL instance = new InfFamily_DAL();
        }

        #endregion
        public List<InfFamily_Model> GetFamilyList(string FamilyCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `ID`
                                          ,`FamilyCode`
                                          ,`IDNumber`
                                          ,`Name`
                                          ,`Relationship`
                                     FROM  `Inf_Family`
                                    WHERE  `FamilyCode` = @FamilyCode 
                                      AND  `Status` = 1 ";
                List<InfFamily_Model> result = db.SetCommand(strSql
                    , db.Parameter("@FamilyCode", FamilyCode, DbType.String)).ExecuteList<InfFamily_Model>();
                return result;
            }
        }

        public int UpdateFamily(int ID, string Name, string IDNumber, string Relationship, int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE  `Inf_Family`
                                      SET  `IDNumber` = @IDNumber
                                          ,`Name` = @Name
                                          ,`Relationship` = @Relationship
                                          ,`Status` = 1
                                          ,`UpdateTime` = @now
                                          ,`Updater` = @UserID
                                    WHERE  `ID` = @ID ";
                int row = db.SetCommand(strSql
                    , db.Parameter("@ID", ID, DbType.Int32)
                    , db.Parameter("@IDNumber", IDNumber, DbType.String)
                    , db.Parameter("@Name", Name, DbType.String)
                    , db.Parameter("@Relationship", Relationship, DbType.String)
                    , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    return 0;
                }
                return 1;
            }
        }

        public int InsertFamily(string FamilyCode, string Name, string IDNumber, string Relationship, int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT `Inf_Family` (`FamilyCode`, `IDNumber`, `Name`, `Relationship`, `Status`, `CreatetTime`, `Creator`)
                                    VALUES (@FamilyCode, @IDNumber, @Name, @Relationship, 1, @now, @UserID)";
                int row = db.SetCommand(strSql
                    , db.Parameter("@FamilyCode", FamilyCode, DbType.String)
                    , db.Parameter("@IDNumber", IDNumber, DbType.String)
                    , db.Parameter("@Name", Name, DbType.String)
                    , db.Parameter("@Relationship", Relationship, DbType.String)
                    , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                    , db.Parameter("@UserID", UserID, DbType.Int32)).ExecuteNonQuery();
                if (row <= 0)
                {
                    return 0;
                }
                return 1;
            }
        }

        public int DeleteFamily(int ID, int UserID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE  `Inf_Family`
                                      SET  `Status` = 2
                                          ,`UpdateTime` = @now
                                          ,`Updater` = @UserID
                                    WHERE  `ID` = @ID ";
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
