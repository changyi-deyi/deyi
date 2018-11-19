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
    public class MemberActM_DAL
    {
        #region 构造类实例
        public static MemberActM_DAL Instance
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
            internal static readonly MemberActM_DAL instance = new MemberActM_DAL();
        }

        #endregion

        public List<MemberAct_Model> getMemberActList(int Status, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @"  SELECT a.*,b.`Name` AS LevelName FROM `inf_memberact` a 
                                    LEFT JOIN `set_memberlevel` b ON a.`LevelID` = b.`LevelID`
                                 WHERE 1=1 {0}  LIMIT @StartCount,@EndCount ";

                string strWhere = "";
                if (Status > 0)
                {
                    strWhere += " and a.Status =@Status";

                }

                strSql = string.Format(strSql, strWhere);

                List<MemberAct_Model> result = db.SetCommand(strSql
                     , db.Parameter("@Status", Status, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<MemberAct_Model>();



                return result;
            }
        }


        public MemberAct_Model getMemberActDetail(int ID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `inf_memberact` WHERE `ID` =@ID  ";


                MemberAct_Model result = db.SetCommand(strSql
                     , db.Parameter("@ID", ID, DbType.Int32)).ExecuteObject<MemberAct_Model>();



                return result;
            }
        }

        public int addMemberAct(MemberAct_Model model)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSql = @" INSERT INTO `inf_memberact` (
                                `Name`,`ImageURL`,`LinkURL`,`LevelID`,`Remark`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                                (@Name,@ImageURL,@LinkURL,@LevelID,@Remark,1,@CreatetTime,@Creator) ;select @@IDENTITY ";

                int id = db.SetCommand(strSql
                     , db.Parameter("@Name", model.Name, DbType.String)
                     , db.Parameter("@ImageURL", model.ImageURL, DbType.String)
                     , db.Parameter("@LinkURL", model.LinkURL, DbType.String)
                     , db.Parameter("@LevelID", model.LevelID, DbType.Int32)
                     , db.Parameter("@Remark", model.Remark, DbType.String)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteScalar<int>();

                if (id == 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }
                

                db.CommitTransaction();
                return 1;
            }
        }

        public int updateMemberAct(MemberAct_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `inf_memberact` 
                                SET
                                  `Name` =@Name,
                                  `ImageURL` =@ImageURL,
                                  `LinkURL` =@LinkURL,
                                  `LevelID` =@LevelID,
                                  `Remark` =@Remark,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ID` =@ID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Name", model.Name, DbType.String)
                     , db.Parameter("@ImageURL", model.ImageURL, DbType.String)
                     , db.Parameter("@LinkURL", model.LinkURL, DbType.String)
                     , db.Parameter("@LevelID", model.LevelID, DbType.Int32)
                     , db.Parameter("@Remark", model.Remark, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteMemberAct(MemberAct_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Inf_MemberAct` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ID` =@ID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public List<MemberActInfo_Model> getMemberInfoList(int ActID,int HandleSts) {
            using (DbManager db = new DbManager())
            {
                string strSql = @"  SELECT a.*,b.`Address` FROM`ope_joinact` a 
LEFT JOIN `inf_address` b
ON a.`AddressID` = b.`ID` WHERE a.`ActID` =@ActID ";

                string strWhere = "";
                if (HandleSts > 0) {
                    strWhere += " and a.HandleSts =@HandleSts ";
                }
                

                List<MemberActInfo_Model> result = db.SetCommand(strSql
                     , db.Parameter("@ActID", ActID, DbType.Int32)
                     , db.Parameter("@HandleSts", HandleSts, DbType.Int32)).ExecuteList<MemberActInfo_Model>();



                return result;
            }
        }


        public int updateHandleSts(MemberActInfo_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `ope_joinact` 
                                SET
                                  `HandleSts` =2,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ID` =@ID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ID", model.ID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }


    }
}
