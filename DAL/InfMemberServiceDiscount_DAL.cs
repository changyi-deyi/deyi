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
    public class InfMemberServiceDiscount_DAL
    {
        #region 构造类实例
        public static InfMemberServiceDiscount_DAL Instance
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
            internal static readonly InfMemberServiceDiscount_DAL instance = new InfMemberServiceDiscount_DAL();
        }

        #endregion
        public InfMemberServiceDiscount_Model GetDiscount(string ServiceCode,int LevelID)
        {
            using (DbManager db = new DbManager("changyi"))
            {
                string strSqlRow = @" SELECT COUNT(1) FROM `Inf_MemberServiceDiscount` WHERE `LevelID` = @LevelID AND `ServiceCode` = @ServiceCode AND `Status` = 1";

                int row = db.SetCommand(strSqlRow
                , db.Parameter("@LevelID", LevelID, DbType.Int32)
                , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteScalar<int>();

                if (row == 0)
                {
                    return null;
                }

                string strSql = @" SELECT `Discount` FROM `Inf_MemberServiceDiscount` WHERE `LevelID` = @LevelID AND `ServiceCode` = @ServiceCode AND `Status` = 1";

                InfMemberServiceDiscount_Model result = db.SetCommand(strSql
                    , db.Parameter("@LevelID", LevelID, DbType.Int32)
                    , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteObject<InfMemberServiceDiscount_Model>();

                return result;
            }
        }

    }
}
