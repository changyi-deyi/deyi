using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;
using BLToolkit.Data;

namespace DAL
{
    public class SetDepartment_DAL
    {
        #region 构造类实例
        public static SetDepartment_DAL Instance
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
            internal static readonly SetDepartment_DAL instance = new SetDepartment_DAL();
        }

        #endregion
        public List<SetDepartment_Model> GetDepartment()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `DepartmentID` 
                                          ,`DepartmentName`
                                          ,`UpperID`
                                          ,`Status`
                                     FROM  `Set_Department` 
                                    WHERE  `Status` = 1
                                 ORDER BY  `UpperID`, `DepartmentID` ";
                List<SetDepartment_Model> result = db.SetCommand(strSql).ExecuteList<SetDepartment_Model>();
                return result;
            }
        }
    }
}
