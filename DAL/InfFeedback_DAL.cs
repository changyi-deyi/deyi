using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;
using BLToolkit.Data;
using Model.Operate_Model;

namespace DAL
{
    public class InfFeedback_DAL
    {
        #region 构造类实例
        public static InfFeedback_DAL Instance
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
            internal static readonly InfFeedback_DAL instance = new InfFeedback_DAL();
        }

        #endregion
        public int SubmitFeedback(Feedback_Model model)
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" INSERT `Inf_Feedback` (`UserID`, `Content`, `Image1`, `Image2`, `Image3`, `SoluteStatus`, `Status`, `CreatetTime`, `Creator`)
                                    VALUES (@UserID, @Content, @Image1, @Image2, @Image3, 1, 1, @now, @UserID)";
                int row = db.SetCommand(strSql
                        , db.Parameter("@UserID", model.UserID, DbType.Int32)
                        , db.Parameter("@Content", model.Content, DbType.String)
                        , db.Parameter("@Image1", model.Image1, DbType.String)
                        , db.Parameter("@Image2", model.Image2, DbType.String)
                        , db.Parameter("@Image3", model.Image3, DbType.String)
                        , db.Parameter("@now", DateTime.Now, DbType.DateTime)).ExecuteNonQuery();
                if (row <= 0)
                {
                    return 0;
                }
                return 1;
            }
        }
    }
}
