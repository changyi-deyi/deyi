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
    public class LevelM_DAL
    {
        #region 构造类实例
        public static LevelM_DAL Instance
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
            internal static readonly LevelM_DAL instance = new LevelM_DAL();
        }

        #endregion

        public List<Level_Model> getLevelList(int Status , int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `set_memberlevel` where 1=1 {0} LIMIT @StartCount,@EndCount ";

                string strWhere = "";

                if (Status > 0) {
                    strWhere += " and Status=@Status ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Level_Model> result = db.SetCommand(strSql
                     , db.Parameter("@Status", Status, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Level_Model>();



                return result;
            }
        }


        public Level_Model getLevelDetail(int LevelID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `set_memberlevel` WHERE `LevelID` =@LevelID  ";


                Level_Model result = db.SetCommand(strSql
                     , db.Parameter("@LevelID", LevelID, DbType.Int32)).ExecuteObject<Level_Model>();



                return result;
            }
        }

        public int addLevel(Level_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT INTO `set_memberlevel` (
                                `Name`,`IconURL`,`Summary`,`TermYears`,`OriginPrice`,`PromPrice`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                                (@Name,@IconURL,@Summary,@TermYears,@OriginPrice,@PromPrice,1,@CreatetTime,@Creator)  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Name", model.Name, DbType.String)
                     , db.Parameter("@IconURL", model.IconURL, DbType.String)
                     , db.Parameter("@Summary", model.Summary, DbType.String)
                     , db.Parameter("@TermYears", model.TermYears, DbType.Int32)
                     , db.Parameter("@OriginPrice", model.OriginPrice, DbType.Decimal)
                     , db.Parameter("@PromPrice", model.PromPrice, DbType.Decimal)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int updateLevel(Level_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `set_memberlevel` 
                                SET
                                  `Name` =@Name,
                                  `IconURL` =@IconURL,
                                  `Summary` =@Summary,
                                  `TermYears` =@TermYears,
                                  `OriginPrice` =@OriginPrice,
                                  `PromPrice` =@PromPrice,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `LevelID` =@LevelID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Name", model.Name, DbType.String)
                     , db.Parameter("@IconURL", model.IconURL, DbType.String)
                     , db.Parameter("@Summary", model.Summary, DbType.String)
                     , db.Parameter("@TermYears", model.TermYears, DbType.Int32)
                     , db.Parameter("@OriginPrice", model.OriginPrice, DbType.Decimal)
                     , db.Parameter("@PromPrice", model.PromPrice, DbType.Decimal)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@LevelID", model.LevelID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteLevel(Level_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `set_memberlevel` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `LevelID` =@LevelID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@LevelID", model.LevelID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }
    }
}
