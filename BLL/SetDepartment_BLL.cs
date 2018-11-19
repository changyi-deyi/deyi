using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class SetDepartment_BLL
    {
        #region 构造类实例
        public static SetDepartment_BLL Instance
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
            internal static readonly SetDepartment_BLL instance = new SetDepartment_BLL();
        }
        #endregion
        public List<SetDepartment_Model> GetDepartment()
        {
            return SetDepartment_DAL.Instance.GetDepartment();
        }
    }
}
