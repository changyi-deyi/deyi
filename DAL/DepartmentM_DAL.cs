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
    public class DepartmentM_DAL
    {
        #region 构造类实例
        public static DepartmentM_DAL Instance
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
            internal static readonly DepartmentM_DAL instance = new DepartmentM_DAL();
        }

        #endregion

        public List<Department_Model> getDepartmentList(string DepartmentName,int UpperID, int StartCount, int EndCount)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT a.*,b.`DepartmentName` AS UpperName FROM `set_department` a LEFT JOIN
                                    `set_department` b ON a.`UpperID` = b.`DepartmentID` WHERE a.`Status`= 1 {0} LIMIT @StartCount,@EndCount ";

                string strWhere = "";

                if (!string.IsNullOrEmpty(DepartmentName))
                {
                    strWhere += " and a.`DepartmentName` like @DepartmentName ";
                }

                if (UpperID > 0) {
                    strWhere += " and a.`UpperID` =@UpperID ";
                }

                strSql = string.Format(strSql, strWhere);

                List<Department_Model> result = db.SetCommand(strSql
                     , db.Parameter("@DepartmentName", "%" + DepartmentName + "%", DbType.String)
                     , db.Parameter("@UpperID", UpperID, DbType.Int32)
                     , db.Parameter("@StartCount", StartCount, DbType.Int32)
                     , db.Parameter("@EndCount", EndCount, DbType.Int32)).ExecuteList<Department_Model>();



                return result;
            }
        }


        public Department_Model getDepartmentDetail(int DepartmentID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `set_department` WHERE `DepartmentID` =@DepartmentID  ";


                Department_Model result = db.SetCommand(strSql
                     , db.Parameter("@DepartmentID", DepartmentID, DbType.Int32)).ExecuteObject<Department_Model>();



                return result;
            }
        }

        public int addDepartment(Department_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT INTO `set_department` (
                                `DepartmentName`,`UpperID`,`Status`,`CreatetTime`,`Creator`) 
                                  VALUES
                                (@DepartmentName,@UpperID,1,@CreatetTime,@Creator)  ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@DepartmentName", model.DepartmentName, DbType.String)
                     , db.Parameter("@UpperID", model.UpperID, DbType.Int32)
                     , db.Parameter("@CreatetTime", model.CreatetTime, DbType.DateTime)
                     , db.Parameter("@Creator", model.Creator, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int updateDepartment(Department_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `set_department` 
                                SET
                                  `DepartmentName` =@DepartmentName,
                                  `UpperID` =@UpperID,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `DepartmentID` =@DepartmentID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@DepartmentName", model.DepartmentName, DbType.String)
                     , db.Parameter("@UpperID", model.UpperID, DbType.Int32)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@DepartmentID", model.DepartmentID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }

        public int deleteDepartment(Department_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" UPDATE 
                                  `set_department` 
                                SET
                                  `Status` =@Status,
                                  `UpdateTime` =@UpdateTime,
                                  `Updater` =@Updater 
                                WHERE `DepartmentID` =@DepartmentID   ";

                int rows = db.SetCommand(strSql
                     , db.Parameter("@Status", model.Status, DbType.String)
                     , db.Parameter("@UpdateTime", model.UpdateTime, DbType.DateTime)
                     , db.Parameter("@Updater", model.Updater, DbType.Int32)
                     , db.Parameter("@DepartmentID", model.DepartmentID, DbType.Int32)).ExecuteNonQuery();

                if (rows != 1)
                {
                    return 0;
                }

                return 1;
            }
        }
    }
}
