using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Operate_Model;
using Model.Table_Model;

namespace BLL
{
    public class SetCardNumPas_BLL
    {
        #region 构造类实例
        public static SetCardNumPas_BLL Instance
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
            internal static readonly SetCardNumPas_BLL instance = new SetCardNumPas_BLL();
        }
        #endregion
        public SetCardNumPas_Model GetCardNum(ExchangeMember_Model model)
        {
            return SetCardNumPas_DAL.Instance.GetCardNum(model);
        }
    }
}
