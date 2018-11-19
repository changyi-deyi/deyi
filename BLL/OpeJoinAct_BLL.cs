using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;
using Model.Operate_Model;

namespace BLL
{
    public class OpeJoinAct_BLL
    {
        #region 构造类实例
        public static OpeJoinAct_BLL Instance
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
            internal static readonly OpeJoinAct_BLL instance = new OpeJoinAct_BLL();
        }
        #endregion
        public int MemberActCount(Act_Model model)
        {
            return OpeJoinAct_DAL.Instance.MemberActCount(model);
        }
        public int AddAct(Act_Model model)
        {
            return OpeJoinAct_DAL.Instance.AddAct(model);
        }
    }
}
