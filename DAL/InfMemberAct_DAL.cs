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
    public class InfMemberAct_DAL
    {
        #region 构造类实例
        public static InfMemberAct_DAL Instance
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
            internal static readonly InfMemberAct_DAL instance = new InfMemberAct_DAL();
        }

        #endregion
        public List<InfMemberAct_Model> GetMemberActList()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  A.`ID`
                                          ,A.`Name`
                                          ,A.`ImageURL`
                                          ,A.`LinkURL`
                                          ,A.`LevelID`
                                          ,A.`Remark`
                                          ,A.`Status`
                                          ,B.`Name` AS LevelName
                                    FROM  `Inf_MemberAct` A, `Set_MemberLevel` B
                                   WHERE  A.`Status` = 1 
                                     AND  A.`LevelID` = B.`LevelID`
                                ORDER BY  A.`LevelID`, A.`ID` ";
                List<InfMemberAct_Model> model = db.SetCommand(strSql).ExecuteList<InfMemberAct_Model>();
                return model;
            }
        }
    }
}
