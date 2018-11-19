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
    public class InfService_BLL
    {
        #region 构造类实例
        public static InfService_BLL Instance
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
            internal static readonly InfService_BLL instance = new InfService_BLL();
        }
        #endregion
        public List<InfService_Model> GetService()
        {
            return InfService_DAL.Instance.GetService();
        }
        public ServiceDetail_Model GetServiceDetail(string ServiceCode)
        {
            return InfService_DAL.Instance.GetServiceDetail(ServiceCode);
        }
    }
}
