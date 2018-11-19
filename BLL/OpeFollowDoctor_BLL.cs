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
    public class OpeFollowDoctor_BLL
    {
        #region 构造类实例
        public static OpeFollowDoctor_BLL Instance
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
            internal static readonly OpeFollowDoctor_BLL instance = new OpeFollowDoctor_BLL();
        }
        #endregion
        public int FollowOrCancle(FollowDoctor_Model model)
        {
            return OpeFollowDoctor_DAL.Instance.FollowOrCancle(model);
        }
    }
}
