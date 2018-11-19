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
    public class ImaService_DAL
    {
        #region 构造类实例
        public static ImaService_DAL Instance
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
            internal static readonly ImaService_DAL instance = new ImaService_DAL();
        }

        #endregion
        public List<ImaService_Model> GetServiceImage(string ServiceCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `ID` 
                                          ,`ServiceCode`
                                          ,`Path`
                                          ,`FileName`
                                     FROM  `Ima_Service` 
                                    WHERE  `Status` = 1
                                      AND  `ServiceCode` = @ServiceCode";
                List<ImaService_Model> result = db.SetCommand(strSql, db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteList<ImaService_Model>();
                return result;
            }
        }
    }
}
