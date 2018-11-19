using BLToolkit.Data;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class SettingM_DAL
    {
        #region 构造类实例
        public static SettingM_DAL Instance
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
            internal static readonly SettingM_DAL instance = new SettingM_DAL();
        }

        #endregion

        public SetGetpoint_Model getSetPoint() {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT * FROM `set_getpoint`  ";

                SetGetpoint_Model result = db.SetCommand(strSql).ExecuteObject<SetGetpoint_Model>();

                return result;
            }
        }

        public int UpdateSetPoint(string key ,string value) {
            using (DbManager db = new DbManager())
            {
                string strSql = @" Update `set_getpoint` set "+ key+" =@value   ";

                int rows = db.SetCommand(strSql
                       , db.Parameter("@value", value, DbType.String)).ExecuteNonQuery();

                if (rows != 1) {
                    return 0;
                }
                return 1;
            }
        }



    }
}
