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
    public class OpeCustomerMessage_BLL
    {
        #region 构造类实例
        public static OpeCustomerMessage_BLL Instance
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
            internal static readonly OpeCustomerMessage_BLL instance = new OpeCustomerMessage_BLL();
        }
        #endregion
        public List<OpeCustomerMessage_Model> GetMessage(CustomerMessage_Model model)
        {
            return OpeCustomerMessage_DAL.Instance.GetMessage(model);
        }

        public int DeleteMessage(MessageDelete_Model model)
        {
            return OpeCustomerMessage_DAL.Instance.DeleteMessage(model);
        }

        public int GetReadMessage(CustomerMessage_Model model)
        {
            return OpeCustomerMessage_DAL.Instance.GetReadMessage(model);
        }
    }
}
