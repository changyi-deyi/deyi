using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class ImaAdvisory_BLL
    {
        #region 构造类实例
        public static ImaAdvisory_BLL Instance
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
            internal static readonly ImaAdvisory_BLL instance = new ImaAdvisory_BLL();
        }
        #endregion
        public List<ImaAdvisory_Model> GetAdvisoryImage(int GroupID, string CustomerCode)
        {
            return ImaAdvisory_DAL.Instance.GetAdvisoryImage(GroupID, CustomerCode);
        }

        public int DeleteAdvisoryImage(int ID, int UserID)
        {
            return ImaAdvisory_DAL.Instance.DeleteAdvisoryImage(ID, UserID);
        }
    }
}
