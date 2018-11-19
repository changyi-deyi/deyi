using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class SetDistrict_BLL
    {
        #region 构造类实例
        public static SetDistrict_BLL Instance
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
            internal static readonly SetDistrict_BLL instance = new SetDistrict_BLL();
        }
        #endregion
        public List<SetDistrict_Model> GetDistrict(string REGION_LEVEL, string PARENT_ID)
        {
            return SetDistrict_DAL.Instance.GetDistrict(REGION_LEVEL, PARENT_ID);
        }
    }
}
