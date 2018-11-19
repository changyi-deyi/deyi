using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Operate_Model;
using Model.Table_Model;
using BLToolkit.Data;

namespace DAL
{
    public class InfService_DAL
    {
        #region 构造类实例
        public static InfService_DAL Instance
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
            internal static readonly InfService_DAL instance = new InfService_DAL();
        }

        #endregion
        public List<InfService_Model> GetService()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  A.`ServiceCode`
                                          ,A.`Name`
                                          ,A.`Summary`
                                          ,A.`Introduct`
                                          ,A.`OriginPrice`
                                          ,A.`PromPrice`
                                          ,A.`ExchangePrice`
                                          ,A.`ListImageURL`
                                     FROM  `Inf_Service` A, `Inf_ServiceSort` B 
                                    WHERE  A.`Status` = 1
                                      AND  A.`IsVisible` = 1
                                      AND  A.`ServiceCode` = B.`ServiceCode`
                                 ORDER BY  B.`Sort`";
                List<InfService_Model> result = db.SetCommand(strSql).ExecuteList<InfService_Model>();
                return result;
            }
        }

        public ServiceDetail_Model GetServiceDetail(string ServiceCode)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlInfo = @" SELECT  `ServiceCode` 
                                              ,`Name`
                                              ,`Summary`
                                              ,`Introduct`
                                              ,`OriginPrice`
                                              ,`PromPrice`
                                              ,`ExchangePrice`
                                              ,`ListImageURL`
                                         FROM  `Inf_Service` 
                                        WHERE  `Status` = 1
                                          AND  `IsVisible` = 1
                                          AND  `ServiceCode` = @ServiceCode ";
                ServiceDetail_Model result = db.SetCommand(strSqlInfo
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteObject<ServiceDetail_Model>();

                if (result != null)
                {
                    string strSqlIma = @" SELECT  `ID`
                                                 ,`ServiceCode`
                                                 ,`Path`
                                                 ,`FileName`
                                            FROM  `Ima_Service`
                                           WHERE  `Status` = 1
                                             AND  `ServiceCode` = @ServiceCode ";
                    result.ImaList = db.SetCommand(strSqlIma
                     , db.Parameter("@ServiceCode", ServiceCode, DbType.String)).ExecuteList<ImaService_Model>();
                }

                return result;
            }
        }
    }
}
