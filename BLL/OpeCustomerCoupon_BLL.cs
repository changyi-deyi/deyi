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
    public class OpeCustomerCoupon_BLL
    {
        #region 构造类实例
        public static OpeCustomerCoupon_BLL Instance
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
            internal static readonly OpeCustomerCoupon_BLL instance = new OpeCustomerCoupon_BLL();
        }
        #endregion
        public List<CustomerCoupon_Model> GetCustomerCoupon(string CustomerCode)
        {
            return OpeCustomerCoupon_DAL.Instance.GetCustomerCoupon(CustomerCode);
        }
        public int CodeExchange(string CustomerCode, int UserID, InfCoupon_Model CouponId)
        {
            return OpeCustomerCoupon_DAL.Instance.CodeExchange(CustomerCode, UserID, CouponId);
        }

        public int BalanceExchange(string CustomerCode, int UserID, decimal Balance, InfCoupon_Model CouponId)
        {
            return OpeCustomerCoupon_DAL.Instance.BalanceExchange(CustomerCode, UserID, Balance, CouponId);
        }
    }
}
