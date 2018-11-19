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
    public class ChannelM_DAL
    {
        #region 构造类实例
        public static ChannelM_DAL Instance
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
            internal static readonly ChannelM_DAL instance = new ChannelM_DAL();
        }

        #endregion

        public List<Channel_Model> getChannelList(int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `Set_Channel`  LIMIT @StartCount,@EndCount ";

                List<Channel_Model> result = db.SetCommand(strSql
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Channel_Model>();



                return result;
            }
        }


        public Channel_Model getChannelDetail(int ChannelID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `Set_Channel` WHERE `ChannelID` =@ChannelID  ";


                Channel_Model result = db.SetCommand(strSql
                     , db.Parameter("@ChannelID", ChannelID, DbType.Int32)).ExecuteObject<Channel_Model>();



                return result;
            }
        }

        public int addChannel(Channel_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT INTO `Set_Channel` (
                                `ChannelName`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                                (@ChannelName,1,@CreatetTime,@Creator)  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@ChannelName", model.ChannelName, DbType.String)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int updateChannel(Channel_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Set_Channel` 
                                SET
                                  `ChannelName` =@ChannelName,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ChannelID` =@ChannelID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@ChannelName", model.ChannelName, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ChannelID", model.ChannelID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteChannel(Channel_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `Set_Channel` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `ChannelID` =@ChannelID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@ChannelID", model.ChannelID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }
    }
}
