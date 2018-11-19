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
    public class OpeJoinAct_DAL
    {
        #region 构造类实例
        public static OpeJoinAct_DAL Instance
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
            internal static readonly OpeJoinAct_DAL instance = new OpeJoinAct_DAL();
        }

        #endregion
        public int MemberActCount(Act_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT COUNT(1) FROM `Ope_JoinAct` WHERE `ActID` = @ActID AND `MemberCode` = @MemberCode AND `Status` = 1 ";

                int rows = db.SetCommand(strSql
                    , db.Parameter("@ActID", model.ActID, DbType.Int32)
                    , db.Parameter("@MemberCode", model.MemberCode, DbType.String)).ExecuteScalar<int>();

                return rows;
            }
        }
        public int AddAct(Act_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT COUNT(1) FROM `Ope_JoinAct` WHERE `ActID` = @ActID AND `MemberCode` = @MemberCode AND `Status` = 1 ";

                int rows= db.SetCommand(strSql
                    , db.Parameter("@ActID", model.ActID, DbType.Int32)
                    , db.Parameter("@MemberCode", model.MemberCode, DbType.String)).ExecuteScalar<int>();

                if (rows > 0)
                {
                    return 2;
                }
                else
                {
                    db.BeginTransaction();
                    string strSqlIns = @" INSERT INTO `Ope_JoinAct` (`ActID`, `MemberCode`, `Name`, `Gender`, `Age`, `Phone`, `IDNumber`, `AddressID`, `HandleSts`, `Status`, `CreatetTime`, `Creator`)
                                    VALUES (@ActID, @MemberCode, @Name, @Gender, @Age, @Phone, @IDNumber, @AddressID, 1, 1, @now, @UserID) ";

                    rows = db.SetCommand(strSqlIns
                        , db.Parameter("@ActID", model.ActID, DbType.Int32)
                        , db.Parameter("@MemberCode", model.MemberCode, DbType.String)
                        , db.Parameter("@Name", model.Name, DbType.String)
                        , db.Parameter("@Gender", model.Gender, DbType.Int32)
                        , db.Parameter("@Age", model.Age, DbType.Int32)
                        , db.Parameter("@Phone", model.Phone, DbType.String)
                        , db.Parameter("@IDNumber", model.IDNumber, DbType.String)
                        , db.Parameter("@AddressID", model.AddressID, DbType.Int32)
                        , db.Parameter("@now", DateTime.Now, DbType.DateTime)
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

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
}
