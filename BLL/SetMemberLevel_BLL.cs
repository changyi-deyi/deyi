using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class SetMemberLevel_BLL
    {
        #region 构造类实例
        public static SetMemberLevel_BLL Instance
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
            internal static readonly SetMemberLevel_BLL instance = new SetMemberLevel_BLL();
        }
        #endregion
        public List<SetMemberLevel_Model> GetLevel()
        {
            return SetMemberLevel_DAL.Instance.GetLevel();
        }
        public SetMemberLevel_Model GetMemberLevel(int LevelID)
        {
            return SetMemberLevel_DAL.Instance.GetMemberLevel(LevelID);
        }
    }
}
