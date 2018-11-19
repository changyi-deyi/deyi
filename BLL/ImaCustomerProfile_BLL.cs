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
    public class ImaCustomerProfile_BLL
    {
        #region 构造类实例
        public static ImaCustomerProfile_BLL Instance
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
            internal static readonly ImaCustomerProfile_BLL instance = new ImaCustomerProfile_BLL();
        }
        #endregion
        public List<ImaCustomerProfile_Model> GetProfileIma(int ID)
        {
            return ImaCustomerProfile_DAL.Instance.GetProfileIma(ID);
        }
        public int DeleteProfileIma(int ID, int UserID)
        {
            return ImaCustomerProfile_DAL.Instance.DeleteProfileIma(ID, UserID);
        }
    }
}
