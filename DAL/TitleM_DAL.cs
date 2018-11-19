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
    public class TitleM_DAL
    {
        #region 构造类实例
        public static TitleM_DAL Instance
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
            internal static readonly TitleM_DAL instance = new TitleM_DAL();
        }

        #endregion

        public List<Title_Model> getTitleList(string TitleName, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `set_title` WHERE 1 = 1 {0} LIMIT @StartCount,@EndCount ";

                string strWhere = "";

                if (!string.IsNullOrEmpty(TitleName))
                {
                    strWhere += " and `TitleName` like @TitleName ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Title_Model> result = db.SetCommand(strSql
                     , db.Parameter("@TitleName", "%" + TitleName + "%", DbType.String)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Title_Model>();



                return result;
            }
        }


        public Title_Model getTitleDetail(int TitleID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `set_title` WHERE `TitleID` =@TitleID  ";


                Title_Model result = db.SetCommand(strSql
                     , db.Parameter("@TitleID", TitleID, DbType.Int32)).ExecuteObject<Title_Model>();



                return result;
            }
        }

        public int addTitle(Title_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT INTO `set_title` (
                                `TitleName`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                                (@TitleName,1,@CreatetTime,@Creator)  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@TitleName", model.TitleName, DbType.String)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int updateTitle(Title_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `set_title` 
                                SET
                                  `TitleName` =@TitleName,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `TitleID` =@TitleID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@TitleName", model.TitleName, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@TitleID", model.TitleID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteTitle(Title_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `set_title` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `TitleID` =@TitleID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@TitleID", model.TitleID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }
    }
}
