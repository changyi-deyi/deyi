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
    public  class TagM_DAL
    {
        #region 构造类实例
        public static TagM_DAL Instance
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
            internal static readonly TagM_DAL instance = new TagM_DAL();
        }

        #endregion

        public List<Tag_Model> getTagList(int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `Set_Tag`  LIMIT @StartCount,@EndCount ";

                List<Tag_Model> result = db.SetCommand(strSql
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Tag_Model>();



                return result;
            }
        }


        public Tag_Model getTagDetail(int TagID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `Set_Tag` WHERE `TagID` =@TagID  ";


                Tag_Model result = db.SetCommand(strSql
                     , db.Parameter("@TagID", TagID, DbType.Int32)).ExecuteObject<Tag_Model>();



                return result;
            }
        }

        public int addTag(Tag_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT INTO `Set_Tag` (
                                `TagName`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                                (@TagName,1,@CreatetTime,@Creator)  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@TagName", model.TagName, DbType.String)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int updateTag(Tag_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Set_Tag` 
                                SET
                                  `TagName` =@TagName,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `TagID` =@TagID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@TagName", model.TagName, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@TagID", model.TagID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteTag(Tag_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Set_Tag` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `TagID` =@TagID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@TagID", model.TagID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }
        public List<Tag_Model> getDoctorTagList(string DoctorCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.`DoctorCode` FROM `set_tag` a 
LEFT JOIN `ope_doctortag` b
ON a.`TagID` = b.`TagID`  AND b.`DoctorCode` =@DoctorCode ";

                List<Tag_Model> result = db.SetCommand(strSql
                     , db.Parameter("@DoctorCode", DoctorCode, DbType.String)).ExecuteList<Tag_Model>();



                return result;
            }
        }
    }
}
