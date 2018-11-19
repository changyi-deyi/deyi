using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class ImaService_BLL
    {
        #region 构造类实例
        public static ImaService_BLL Instance
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
            internal static readonly ImaService_BLL instance = new ImaService_BLL();
        }
        #endregion
        public List<ImaService_Model> GetServiceImage(string ServiceCode)
        {
            return ImaService_DAL.Instance.GetServiceImage(ServiceCode);
        }
    }
}
