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
    public class OpeComment_BLL
    {
        #region 构造类实例
        public static OpeComment_BLL Instance
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
            internal static readonly OpeComment_BLL instance = new OpeComment_BLL();
        }
        #endregion
        public List<OpeComment_Model> GetServiceOrder(string DoctorCode)
        {
            return OpeComment_DAL.Instance.GetServiceOrder(DoctorCode);
        }
        public int ServiceComment(ServiceComment_Model model)
        {
            return OpeComment_DAL.Instance.ServiceComment(model);
        }
    }
}
