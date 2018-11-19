using BLToolkit.Data;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AdvisoryM_DAL
    {
        #region 构造类实例
        public static AdvisoryM_DAL Instance
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
            internal static readonly AdvisoryM_DAL instance = new AdvisoryM_DAL();
        }

        #endregion

        public List<Advisory_Model> getAdvisoryList(string CustomerCode, string CustomerName, int IsDone, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.`Name` AS CustomerName,b.`Mobile` as CustomerMobile FROM `ope_advisory` a
                                    LEFT JOIN `inf_customer` b
                                    ON a.`OpCode` = b.`CustomerCode` AND a.`OpType` = 1
                                    LEFT JOIN `inf_staff` C
                                      ON a.`OpCode` = c.`StaffCode` AND a.`OpType` = 2
                                     WHERE a.`Status` = 1 AND a.`GroupID`  = 0  {0}  ORDER BY a.`CreatetTime` DESC  LIMIT @StartCount,@EndCount ";

                string strWhere = "";


                if (!string.IsNullOrEmpty(CustomerCode))
                {
                    strWhere += " AND a.`OpCode` = @CustomerCode ";

                }

                if (!string.IsNullOrEmpty(CustomerName))
                {
                    strWhere += " AND b.`Name` LIKE @CustomerName ";

                }
                if (IsDone > 0)
                {
                    strWhere += " AND a.`IsDone` =@IsDone ";

                }

                strSql = string.Format(strSql, strWhere);

                List<Advisory_Model> result = db.SetCommand(strSql
                     , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                     , db.Parameter("@CustomerName", "%" + CustomerName + "%", DbType.String)
                     , db.Parameter("@IsDone", IsDone, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Advisory_Model>();


                return result;
            }
        }

        public List<Advisory_Model> getGroupAdvisoryList(int ID) {
            using (DbManager db = new DbManager())
            {
                string strSql = @"  SELECT a.*,b.`Name` AS CustomerName FROM `ope_advisory` a
                                    LEFT JOIN `inf_customer` b
                                    ON a.`OpCode` = b.`CustomerCode` AND a.`OpType` = 1
                                    LEFT JOIN `inf_staff` C
                                      ON a.`OpCode` = c.`StaffCode` AND a.`OpType` = 2
                                     WHERE a.`Status` = 1 AND (a.`GroupID`  =@ID  or a.ID =@ID  )  ORDER BY a.`CreatetTime`  ";


                List<Advisory_Model> result = db.SetCommand(strSql, db.Parameter("@ID", ID, DbType.Int32)).ExecuteList<Advisory_Model>();

                if (result != null && result.Count > 0) {
                    string strSqlImg = @" SELECT * FROM `ima_advisory` WHERE STATUS = 1 AND `ID`=@ID ";

                    foreach (Advisory_Model item in result)
                    {
                        item.ImgList = new List<AdvisoryImg_Model>();
                        item.ImgList = db.SetCommand(strSqlImg, db.Parameter("@ID", item.ID, DbType.Int32)).ExecuteList<AdvisoryImg_Model>();
                    }
                }

                return result;

            }
        }


        public int AnswerAdvisory(Advisory_Model model) {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSql = @"INSERT INTO `ope_advisory` 
                            (`OpCode`,`Type`,`Content`,`OpType`,`GroupID`,`IsDone`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                            (@OpCode,1,@Content,2,@GroupID,1,1,@CreatetTime,@Creator) ;  ";


                int rows = db.SetCommand(strSql
                    , db.Parameter("@OpCode", model.OpCode, DbType.String)
                    , db.Parameter("@Content", model.Content, DbType.String)
                    , db.Parameter("@GroupID", model.GroupID, DbType.Int32)
                    , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                    , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1) {
                    db.RollbackTransaction();
                    return 0;
                }

                string strSqlUpdate = @" UPDATE `ope_advisory` SET `IsDone` = 2
                                        ,`UpdateTime`=@UpdateTime
                                        ,`Updater`=@Updater
                                         WHERE `GroupID`=@GroupID OR `ID`=@GroupID ";

                rows = db.SetCommand(strSqlUpdate
                   , db.Parameter("@GroupID", model.GroupID, DbType.Int32)
                   , db.Parameter("@UpdateTime", model.CreatetTime, DbType.DateTime)
                   , db.Parameter("@Updater", model.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows == 0)
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
