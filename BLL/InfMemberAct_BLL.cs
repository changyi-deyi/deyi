using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class InfMemberAct_BLL
    {
        #region 构造类实例
        public static InfMemberAct_BLL Instance
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
            internal static readonly InfMemberAct_BLL instance = new InfMemberAct_BLL();
        }
        #endregion
        public List<InfMemberAct_Model> GetMemberActList()
        {
            return InfMemberAct_DAL.Instance.GetMemberActList();
        }
    }
}
