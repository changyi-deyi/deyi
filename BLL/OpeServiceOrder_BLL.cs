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
    public class OpeServiceOrder_BLL
    {
        #region 构造类实例
        public static OpeServiceOrder_BLL Instance
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
            internal static readonly OpeServiceOrder_BLL instance = new OpeServiceOrder_BLL();
        }
        #endregion
        public List<ServiceOrder_Model> GetServiceOrder(string CustomerCode)
        {
            return OpeServiceOrder_DAL.Instance.GetServiceOrder(CustomerCode);
        }
        public List<ServiceOrder_Model> GetOrderList(string CustomerCode, int tag)
        {
            return OpeServiceOrder_DAL.Instance.GetOrderList(CustomerCode, tag);
        }
        public OpeServiceOrder_Model GetCommentStatus(string OrderCode)
        {
            return OpeServiceOrder_DAL.Instance.GetCommentStatus(OrderCode);
        }
        public int OrderCancel(string OrderCode, int UserID)
        {
            return OpeServiceOrder_DAL.Instance.OrderCancel(OrderCode, UserID);
        }
        public ServiceOrder_Model GetOrderDetail(string OrderCode)
        {
            return OpeServiceOrder_DAL.Instance.GetOrderDetail(OrderCode);
        }
        public int AfterService(string OrderCode, string Reason, int UserID)
        {
            return OpeServiceOrder_DAL.Instance.AfterService(OrderCode, Reason, UserID);
        }

        public AddServiceOrderResult_Model AddServiceOrder(AddServiceOrder_Model model)
        {
            return OpeServiceOrder_DAL.Instance.AddServiceOrder(model);
        }


        public AddServiceOrderResult_Model PayServiceOrder(AddServiceOrder_Model model)
        {
            return OpeServiceOrder_DAL.Instance.PayServiceOrder(model);
        }


        public int UpdatePayServiceOrderResult(string NetTradeCode, string Data, int mode) {
            return OpeServiceOrder_DAL.Instance.UpdatePayServiceOrderResult(NetTradeCode,  Data,  mode);
        }
        
    }
}
