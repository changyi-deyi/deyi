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
    public class SetCardNumPas_DAL
    {
        #region 构造类实例
        public static SetCardNumPas_DAL Instance
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
            internal static readonly SetCardNumPas_DAL instance = new SetCardNumPas_DAL();
        }

        #endregion
        public SetCardNumPas_Model GetCardNum(ExchangeMember_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSqlSel = @" SELECT COUNT(1) FROM `Set_CardNumPas` WHERE `CardNumber` = @CardNumber AND `Passeord` = @Passeord AND `Status` = 1 ";

                int count = db.SetCommand(strSqlSel
                , db.Parameter("@Passeord", model.Passeord, DbType.String)
                , db.Parameter("@CardNumber", model.CardNumber, DbType.String)).ExecuteScalar<int>();
                if (count == 0)
                {
                    return null;
                }

                string strSql = @" SELECT  `LevelID`
                                          ,`IsExchange`
                                          ,`ChannelID`
                                     FROM  `Set_CardNumPas`
                                    WHERE  `CardNumber` = @CardNumber 
                                      AND  `Passeord` = @Passeord
                                      AND  `Status` = 1 ";
                SetCardNumPas_Model result = db.SetCommand(strSql
                , db.Parameter("@Passeord", model.Passeord, DbType.String)
                , db.Parameter("@CardNumber", model.CardNumber, DbType.String)).ExecuteObject<SetCardNumPas_Model>();

                return result;
            }
        }
    }
}
