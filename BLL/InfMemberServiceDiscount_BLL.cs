using DAL;
using Model.Table_Model;
using Model.Operate_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class InfMemberServiceDiscount_BLL
    {
        #region 构造类实例
        public static InfMemberServiceDiscount_BLL Instance
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
            internal static readonly InfMemberServiceDiscount_BLL instance = new InfMemberServiceDiscount_BLL();
        }
        #endregion
        public InfMemberServiceDiscount_Model GetDiscount(string ServiceCode, int LevelID)
        {
            return InfMemberServiceDiscount_DAL.Instance.GetDiscount(ServiceCode, LevelID);
        }
    }
}
