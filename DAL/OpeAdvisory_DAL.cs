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
    public class OpeAdvisory_DAL
    {
        #region 构造类实例
        public static OpeAdvisory_DAL Instance
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
            internal static readonly OpeAdvisory_DAL instance = new OpeAdvisory_DAL();
        }

        #endregion
        public List<OpeAdvisory_Model> GetAdvisoryList(string CustomerCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  A.`ID`,
                                           A.`Content`,
	                                       A.`CreatetTime`,
	                                       A.`GroupID`,
	                                       A.`OpType`
                                    FROM   `Ope_Advisory` A
                                   WHERE A.`GroupID` = 0
                                     AND A.`OpCode` = @CustomerCode
                                     AND A.`Status` = 1 
                                ORDER BY A.`CreatetTime` DESC";

                List<OpeAdvisory_Model> result = db.SetCommand(strSql
                            , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteList<OpeAdvisory_Model>();

                return result;
            }
        }

        public List<AdvisoryDetail_Model> GetAdvisoryDetail(string CustomerCode, int GroupID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `ID`
                                          ,`OpCode`
                                          ,`Type`
                                          ,`Content`
                                          ,`OpType`
                                          ,`GroupID`
                                          ,`CreatetTime`
                                     FROM  `Ope_Advisory`
                                    WHERE  (`GroupID` = @GroupID OR `ID` = @GroupID)
                                      AND  `Status` = 1 
                                 ORDER BY  `ID` ";
                List<AdvisoryDetail_Model> result = db.SetCommand(strSql
                            , db.Parameter("@GroupID", GroupID, DbType.Int32)).ExecuteList<AdvisoryDetail_Model>();

                if (result != null && result.Count > 0)
                {
                    foreach (AdvisoryDetail_Model item in result)
                    {
                        string strSqlIma = @" SELECT  `ID`
                                                     ,`Path`
                                                     ,`FileName`
                                                FROM  `Ima_Advisory`
                                               WHERE  `ID` = @ID
                                                 AND  `Status` = 1 ";
                        item.AdvisoryIma = db.SetCommand(strSqlIma
                            , db.Parameter("@ID", item.ID, DbType.Int32)).ExecuteList<ImaAdvisory_Model>();
                    }
                }

                return result;
            }
        }

        public int SubmitAdvisory(SubmitAdvisory_Model model)
        {
            DateTime now = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                int ImaID = 0;
                //继续咨询
                if (model.ComtinueFlg == 1)
                {
                    string strOpeIns = @" INSERT `Ope_Advisory` (`OpCode`, `Type`, `Content`, `OpType`, `GroupID`, `IsDone`, `Status`, `CreatetTime`, `Creator`)
                                       VALUES (@CustomerCode, 1, @Content, 1, @GroupID, 1, 1, @now, @UserID);select @@IDENTITY";
                    ImaID = db.SetCommand(strOpeIns
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                        , db.Parameter("@Content", model.Content, DbType.String)
                        , db.Parameter("@GroupID", model.GroupID, DbType.Int32)
                        , db.Parameter("@now", now, DbType.DateTime)
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteScalar<int>();
                    if (ImaID <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    string strOpeUpd = @" UPDATE  `Ope_Advisory`
                                             SET  `IsDone` = 1
                                                 ,`UpdateTime` = @now
                                                 ,`Updater` = @UserID
                                           WHERE  `GroupID` = @GroupID
                                              OR  `ID` = @GroupID ";
                    int row = db.SetCommand(strOpeUpd
                         , db.Parameter("@GroupID", model.GroupID, DbType.Int32)
                         , db.Parameter("@now", now, DbType.DateTime)
                         , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();
                    if (row <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                //新增咨询
                else
                {
                    string strOpeIns = @" INSERT `Ope_Advisory` (`OpCode`, `Type`, `Content`, `OpType`, `GroupID`, `IsDone`, `Status`, `CreatetTime`, `Creator`)
                                       VALUES (@CustomerCode, 1, @Content, 1, 0, 1, 1, @now, @UserID);select @@IDENTITY";
                    ImaID = db.SetCommand(strOpeIns
                        , db.Parameter("@CustomerCode", model.CustomerCode, DbType.String)
                        , db.Parameter("@Content", model.Content, DbType.String)
                        , db.Parameter("@now", now, DbType.DateTime)
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteScalar<int>();
                    if (ImaID <= 0)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }
                }
                
                //如有图片则插入图片
                if (model.AdvisoryIma!= null && model.AdvisoryIma.Count > 0)
                {
                    string strImaIns = @" INSERT `Ima_Advisory` (`ID`, `Path`, `FileName`, `Status`, `CreatetTime`, `Creator`)
                                           VALUES (@ID, @Path, @FileName, 1, @now, @UserID) ";
                    foreach (ImageURL_Model item in model.AdvisoryIma)
                    {
                        int row = db.SetCommand(strImaIns
                             , db.Parameter("@ID", ImaID, DbType.Int32)
                             , db.Parameter("@Path", item.ImageURL, DbType.String)
                             , db.Parameter("@FileName", item.FileName, DbType.String)
                             , db.Parameter("@now", now, DbType.DateTime)
                             , db.Parameter("@UserID", model.UserID, DbType.Int32)).ExecuteNonQuery();
                        if (row <= 0)
                        {
                            db.RollbackTransaction();
                            return 0;
                        }
                    }
                }
                db.CommitTransaction();
                return 1;
            }
        }
    }
}
