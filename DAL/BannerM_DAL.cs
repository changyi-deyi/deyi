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
    public class BannerM_DAL
    {
        #region 构造类实例
        public static BannerM_DAL Instance
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
            internal static readonly BannerM_DAL instance = new BannerM_DAL();
        }

        #endregion

        public List<Banner_Model> getBannerList(int Status, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.`Sort` FROM `Inf_Banner` a 
LEFT JOIN `inf_bannersort` b 
ON a.`ID`=b.`ID` 
 WHERE 1=1 {0}  LIMIT @StartCount,@EndCount ";

                string strWhere = "";
                if (Status > 0) {
                    strWhere += " and a.Status =@Status";

                }

                strSql = string.Format(strSql, strWhere);

                List<Banner_Model> result = db.SetCommand(strSql
                     , db.Parameter("@Status", Status, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Banner_Model>();



                return result;
            }
        }


        public Banner_Model getBannerDetail(int ID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `Inf_Banner` WHERE `ID` =@ID  ";


                Banner_Model result = db.SetCommand(strSql
                     , db.Parameter("@ID", ID, DbType.Int32)).ExecuteObject<Banner_Model>();



                return result;
            }
        }

        public int addBanner(Banner_Model model)
        {
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSql = @" INSERT INTO `Inf_Banner` (
                                `ImageURL`,`Type`,`Value`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                                (@ImageURL,@Type,@Value,1,@CreatetTime,@Creator) ;select @@IDENTITY ";

                int id = db.SetCommand(strSql
                     , db.Parameter("@ImageURL", model.ImageURL, DbType.String)
                     , db.Parameter("@Type", model.Type, DbType.Int32)
                     , db.Parameter("@Value", model.Value, DbType.String)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteScalar<int>();

                if (id == 0)
                {
                    db.RollbackTransaction();
                    return 0;
                }

                string strSqlSort = @" INSERT INTO `inf_bannersort` (`ID`, `Sort`) VALUES (@ID, @Sort) ;";

                int rows = db.SetCommand(strSqlSort
                     , db.Parameter("@ID", id, DbType.Int32)
                     , db.Parameter("@Sort", id, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1) {
                    db.RollbackTransaction();
                    return 0;
                }

                db.CommitTransaction();
                return 1;
            }
        }

        public int updateBanner(Banner_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Inf_Banner` 
                                SET
                                  `ImageURL` =@ImageURL,
                                  `Type` =@Type,
                                  `Value` =@Value,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ID` =@ID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@ImageURL", model.ImageURL, DbType.String)
                     , db.Parameter("@Type", model.Type, DbType.Int32)
                     , db.Parameter("@Value", model.Value, DbType.String)
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

        public int deleteBanner(Banner_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Inf_Banner` 
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

        public int UpdateSort(int ID, int Sort) {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `inf_bannersort` 
                                SET
                                  `Sort` =@Sort
                                WHERE `ID` =@ID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Sort", Sort, DbType.String)
                     , db.Parameter("@ID", ID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

    }
}
