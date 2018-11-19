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
    public class InfBanner_DAL
    {
        #region 构造类实例
        public static InfBanner_DAL Instance
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
            internal static readonly InfBanner_DAL instance = new InfBanner_DAL();
        }

        #endregion
        public List<InfBanner_Model> GetInfoBanner()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT 
                                        A.ID
                                        A.ImageURL
                                        A.Type
                                        A.Value
                                        A.Status
                                        A.CreatetTime
                                        A.Creator
                                        A.UpdateTime
                                        A.Updater
                                    FROM inf_banner
                                    WHERE A.Status = 1 ";
                List<InfBanner_Model> list = db.SetCommand(strSql).ExecuteList<InfBanner_Model>();
                return list;
            }
        }

        public List<InfBanner_Model> GetImage()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT    A.`ImageURL`
                                            ,A.`Type`
                                            ,A.`Value`
                                            ,A.`Status`
                                    FROM     `inf_banner` A
                                    INNER JOIN `inf_bannersort` B ON A.`ID` = B.`ID`
                                    WHERE    A.`Status` = 1
                                    ORDER BY  B.`Sort` ";
                List<InfBanner_Model> result = db.SetCommand(strSql).ExecuteList<InfBanner_Model>();
                return result;
            }
        }
    }
}
