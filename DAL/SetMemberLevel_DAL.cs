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
    public class SetMemberLevel_DAL
    {
        #region 构造类实例
        public static SetMemberLevel_DAL Instance
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
            internal static readonly SetMemberLevel_DAL instance = new SetMemberLevel_DAL();
        }

        #endregion
        public List<SetMemberLevel_Model> GetLevel()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `LevelID` 
                                          ,`Name`
                                          ,`IconURL`
                                          ,`Summary`
                                          ,`TermYears`
                                          ,`OriginPrice`
                                          ,`PromPrice`
                                          ,`LastLevelID`
                                          ,`NextLevelID`
                                          ,`Status`
                                     FROM  `Set_MemberLevel` 
                                    WHERE  `Status` = 1
                                 ORDER BY  `LevelID` ";
                List<SetMemberLevel_Model> result = db.SetCommand(strSql).ExecuteList<SetMemberLevel_Model>();
                return result;
            }
        }

        public SetMemberLevel_Model GetMemberLevel(int LevelID)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `LevelID` 
                                          ,`Name`
                                          ,`IconURL`
                                          ,`Summary`
                                          ,`TermYears`
                                          ,`OriginPrice`
                                          ,`PromPrice`
                                          ,`LastLevelID`
                                          ,`NextLevelID`
                                          ,`Status`
                                     FROM  `Set_MemberLevel` 
                                    WHERE  `Status` = 1
                                      AND  `LevelID` = @LevelID ";
                SetMemberLevel_Model result = db.SetCommand(strSql
                    , db.Parameter("@LevelID", LevelID, DbType.Int32)).ExecuteObject<SetMemberLevel_Model>();
                return result;
            }
        }
    }
}
