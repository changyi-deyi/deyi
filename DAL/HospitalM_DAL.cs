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
    public  class HospitalM_DAL
    {
        #region 构造类实例
        public static HospitalM_DAL Instance
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
            internal static readonly HospitalM_DAL instance = new HospitalM_DAL();
        }

        #endregion

        public List<Hospital_Model> getHospitalList(string HospitalName, int StartCount, int EndCount) {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `set_hospital` WHERE status = 1 {0} LIMIT @StartCount,@EndCount ";

                string strWhere = "";

                if (!string.IsNullOrEmpty(HospitalName)) {
                    strWhere += " and `HospitalName` like @HospitalName ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Hospital_Model> result = db.SetCommand(strSql
                     , db.Parameter("@HospitalName", "%" + HospitalName+"%", DbType.String)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Hospital_Model>();



                return result;
            }
        }


        public Hospital_Model getHospitalDetail(int HospitalID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `set_hospital` WHERE `HospitalID` =@HospitalID  ";


                Hospital_Model result = db.SetCommand(strSql
                     , db.Parameter("@HospitalID", HospitalID, DbType.Int32)).ExecuteObject<Hospital_Model>();



                return result;
            }
        }

        public int addHospital(Hospital_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT INTO `set_hospital` (
                                `HospitalName`,`Introduction`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                                (@HospitalName,@Introduction,1,@CreatetTime,@Creator)  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@HospitalName", model.HospitalName, DbType.String)
                     , db.Parameter("@Introduction", model.Introduction, DbType.String)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1) {
                    return 0;
                }

                return 1;
            }
        }

        public int updateHospital(Hospital_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `set_hospital` 
                                SET
                                  `HospitalName` =@HospitalName,
                                  `Introduction` =@Introduction,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `HospitalID` =@HospitalID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@HospitalName", model.HospitalName, DbType.String)
                     , db.Parameter("@Introduction", model.Introduction, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@HospitalID", model.HospitalID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteHospital(Hospital_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `set_hospital` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `HospitalID` =@HospitalID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@HospitalID", model.HospitalID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

    }
}
