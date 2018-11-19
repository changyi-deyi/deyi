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
    public class OpeComment_DAL
    {
        #region 构造类实例
        public static OpeComment_DAL Instance
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
            internal static readonly OpeComment_DAL instance = new OpeComment_DAL();
        }

        #endregion
        public List<OpeComment_Model> GetServiceOrder(string DoctorCode)
        {
            using (DbManager db = new DbManager("changyi"))
            {
                string strSql = @" SELECT  A.`OrderCode`
                                          ,A.`CustomerCode`
                                          ,A.`DoctorCode`
                                          ,A.`Point`
                                          ,A.`Comment`
                                          ,DATE_FORMAT(A.`CreatetTime`,'%Y-%m-%d') AS `CreatetTimeForView`
                                          ,DATE_FORMAT(A.`UpdateTime`,'%Y-%m-%d') AS `UpdateTimeForView`
                                          ,B.`Name` AS `CustomerName`
                                          ,D.`Name` AS `ServiceName`
                                     FROM  `Ope_Comment` A,`Inf_Customer` B, Ope_ServiceOrder C, Inf_Service D
                                    WHERE  A.`CustomerCode` = B.`CustomerCode`
                                      AND  A.`DoctorCode` = @DoctorCode
                                      AND  A.`OrderCode` = C.`OrderCode`
                                      AND  C.`ServiceCode` = D.`ServiceCode`
                                      AND  A.`Status` = 1
                                      AND  B.`Status` = 1
                                      AND  C.`Status` = 1
                                      AND  D.`Status` = 1
                                 ORDER BY  A.`CreatetTime` DESC ";
                List<OpeComment_Model> result = db.SetCommand(strSql, db.Parameter("@DoctorCode", DoctorCode, DbType.String)).ExecuteList<OpeComment_Model>();
                return result;
            }
        }


        public int ServiceComment(ServiceComment_Model model)
        {
            DateTime now = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                decimal Point = decimal.Round((model.Overall+model.Profession +model.Altitude)/3, 1, MidpointRounding.AwayFromZero);

                db.BeginTransaction();
                string strCommentIns = @" INSERT `Ope_Comment` (`OrderCode`, `CustomerCode`, `DoctorCode`, `Point`, `Comment`, `Overall`, `Profession`, `Altitude`, `IsSolute`, `Status`, `CreatetTime`, `Creator`)
                                           VALUES (@OrderCode, @CustomerCode, @DoctorCode, @Point, @Comment, @Overall, @Profession, @Altitude, @IsSolute, 1, @now, @UserID) ";

                int rows = db.SetCommand(strCommentIns
                     , db.Parameter("@OrderCode", model.OrderCode, DbType.String)
                     , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                     , db.Parameter("@DoctorCode", model.DoctorCode, DbType.String)
                     , db.Parameter("@Point", Point, DbType.Decimal)
                     , db.Parameter("@Comment", model.Comment, DbType.String)
                     , db.Parameter("@Overall", model.Overall, DbType.Decimal)
                     , db.Parameter("@Profession", model.Profession, DbType.Decimal)
                     , db.Parameter("@Altitude", model.Altitude, DbType.Decimal)
                     , db.Parameter("@IsSolute", model.IsSolute, DbType.Int32)
                     , db.Parameter("@now", now, DbType.DateTime)
                     , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                if (rows <= 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strOrderUpd = @" UPDATE  `Ope_ServiceOrder`
                                           SET  `CommentStatus` = 2
                                               ,`OrderStatus` = 2
                                               ,`UpdateTime` = @now
                                               ,`Updater` = @UserID
                                         WHERE  `OrderCode` = @OrderCode
                                           AND  `Status` = 1 ";

                rows = db.SetCommand(strOrderUpd
                     , db.Parameter("@OrderCode", model.OrderCode, DbType.String)
                     , db.Parameter("@now", now, DbType.DateTime)
                     , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();

                if (rows <= 0)
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
