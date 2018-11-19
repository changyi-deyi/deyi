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
    public class SetDistrict_DAL
    {
        #region 构造类实例
        public static SetDistrict_DAL Instance
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
            internal static readonly SetDistrict_DAL instance = new SetDistrict_DAL();
        }

        #endregion
        public List<SetDistrict_Model> GetDistrict(string REGION_LEVEL, string PARENT_ID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `REGION_CODE`
                                          ,`REGION_NAME`
                                     FROM  `Set_District` 
                                    WHERE  1 = 1";
                if (string.IsNullOrEmpty(REGION_LEVEL) || REGION_LEVEL == "1")
                {
                    strSql += " AND `REGION_LEVEL` = '1' ";
                }
                else
                {
                    strSql += " AND `REGION_LEVEL` = @REGION_LEVEL AND `PARENT_ID` = @PARENT_ID ";
                }
                strSql += " ORDER BY `REGION_ID` ";

                List<SetDistrict_Model> result = db.SetCommand(strSql
                    , db.Parameter("@REGION_LEVEL", REGION_LEVEL, DbType.String)
                    , db.Parameter("@PARENT_ID", PARENT_ID, DbType.String)).ExecuteList<SetDistrict_Model>();

                return result;
            }
        }
    }
}
