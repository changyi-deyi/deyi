using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public  class GetCustomerCoupon_Model
    {
        public string CustomerCode { get; set; }
    }


    [Serializable]
    public class CustomerCoupon_Model
    {
        public string CouponCode { get; set; }
        public string CustomerCode { get; set; }
        public int ID { get; set; }
        public DateTime GotDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Detail { get; set; }
        public int Type { get; set; }
        public string Rule { get; set; }
        public int IsDuplicate { get; set; }
        public int IsReuse { get; set; }
        public int ExchangeType { get; set; }
        public decimal ExchangeAmount { get; set; }
        public int MaxQty { get; set; }
        public int Qty { get; set; }
        public int UsedQty { get; set; }
        public int ValidType { get; set; }
        public string ValidRUle { get; set; }
    }

    [Serializable]
    public class CouponExchange_Model
    {
        //客户编号
        public string CustomerCode { get; set; }
        //串码
        public string ExchangeCode { get; set; }
        //用户ID
        public int UserID { get; set; }
        //优惠券ID
        public int CouponID { get; set; }
        //兑换金额
        public decimal ExchangeAmount { get; set; }
    }

    [Serializable]
    public class CouponMember_Model
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string Rule { get; set; }
        public string CouponCode { get; set; }
    }
}
