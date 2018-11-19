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
    public class OpeFollowDoctor_DAL
    {
        #region 构造类实例
        public static OpeFollowDoctor_DAL Instance
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
            internal static readonly OpeFollowDoctor_DAL instance = new OpeFollowDoctor_DAL();
        }

        #endregion
        public int FollowOrCancle(FollowDoctor_Model model)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSqlSel = @" SELECT COUNT(1) FROM `Ope_FollowDoctor` WHERE `DoctorCode` = @DoctorCode AND `CustomerCode` = @CustomerCode";

                int count = db.SetCommand(strSqlSel
                , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)
                , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteScalar<int>();

                //关注
                if (count == 0)
                {
                    string strSqlIns = @" INSERT  `Ope_FollowDoctor` (`DoctorCode`, `CustomerCode`)
                                           VALUES  (@DoctorCode, @CustomerCode) ";


                    int rows = db.SetCommand(strSqlIns
                        , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                //取关
                else
                {
                    string strSqlDel = @" DELETE FROM `Ope_FollowDoctor` WHERE `DoctorCode` = @DoctorCode AND `CustomerCode` = @CustomerCode ";


                    int rows = db.SetCommand(strSqlDel
                        , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)).ExecuteNonQuery();

                    if (rows <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                db.CommitTransaction();
                return 1;
            }
        }
    }
}
