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
    public class InfAddress_BLL
    {
        #region 构造类实例
        public static InfAddress_BLL Instance
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
            internal static readonly InfAddress_BLL instance = new InfAddress_BLL();
        }
        #endregion
        public List<InfAddress_Model> GetAddress(string CustomerCode)
        {
            return InfAddress_DAL.Instance.GetAddress(CustomerCode);
        }

        public int SaveAddress(SaveAddress_Model model)
        {
            return InfAddress_DAL.Instance.SaveAddress(model);
        }

        public int DeleteAddress(SaveAddress_Model model)
        {
            return InfAddress_DAL.Instance.DeleteAddress(model);
        }
    }
}
