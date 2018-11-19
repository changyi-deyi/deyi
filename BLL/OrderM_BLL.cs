using DAL;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class OrderM_BLL
    {
        #region 构造类实例
        public static OrderM_BLL Instance
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
            internal static readonly OrderM_BLL instance = new OrderM_BLL();
        }

        #endregion

        public List<MemberOrder_Model> getMemberOrderList(string CustomerCode, string CustomerName, string StartDate, string EndDate, int PaymentStatus, int OrderStatus, int StartCount = 0, int EndCount = 999999999)
        {
            return OrderM_DAL.Instance.getMemberOrderList(CustomerCode, CustomerName, StartDate, EndDate, PaymentStatus, OrderStatus, StartCount, EndCount);
        }


        public MemberOrder_Model getMemberOrderDetail(string OrderCode)
        {
            return OrderM_DAL.Instance.getMemberOrderDetail(OrderCode);
        }

        public int CancelMemberOrder(MemberOrder_Model model)
        {
            return OrderM_DAL.Instance.CancelMemberOrder(model);
        }

        public List<ServiceOrder_Model> getServiceOrderList(string CustomerName, string DoctorName, string ServiceName, int PaymentStatus, int ServiceStatus, int OrderStatus, string StartDate, string EndDate, string CustomerCode = null, int StartCount = 0, int EndCount = 999999999)
        {
            return OrderM_DAL.Instance.getServiceOrderList(CustomerName, DoctorName, ServiceName, PaymentStatus, ServiceStatus, OrderStatus, StartDate, EndDate, CustomerCode, StartCount, EndCount);
        }


        public ServiceOrder_Model getServiceOrderDetail(string OrderCode)
        {
            return OrderM_DAL.Instance.getServiceOrderDetail(OrderCode);
        }


        public int ReplyServiceOrder(ServiceOrder_Model model)
        {
            return OrderM_DAL.Instance.ReplyServiceOrder(model);

        }


        public int UpdateServiceOrderAmount(ServiceOrder_Model model) {
            return OrderM_DAL.Instance.UpdateServiceOrderAmount(model);
        }


        public int UpdateServiceOrderStatus(ServiceOrder_Model model) {
            return OrderM_DAL.Instance.UpdateServiceOrderStatus(model);
        }


        public List<string> getNoResultNetTradeNoByOrder(string OrderCode)
        {
            return OrderM_DAL.Instance.getNoResultNetTradeNoByOrder(OrderCode);
        }
        }
}
