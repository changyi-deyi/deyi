using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;

namespace Model.Operate_Model
{
    [Serializable]
    public  class ServiceOrder_Model
    {
        public string OrderCode { get; set; }
        public string ServiceCode { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string LocatedCity { get; set; }
        public string Description { get; set; }
        public DateTime ExpectTime { get; set; }
        public DateTime ArrangedTime { get; set; }
        public string DoctorCode { get; set; }
        public string ReceptionistCode { get; set; }
        public string ReceptionistPhone { get; set; }
        public string Address { get; set; }
        public string SafeCode { get; set; }
        public int AmountType { get; set; }
        public decimal OriginAmount { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal Unpaid { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public int PaymentStatus { get; set; }
        public DateTime PaymentTime { get; set; }
        public int ServiceStatus { get; set; }
        public DateTime ServiceTime { get; set; }
        public int OrderStatus { get; set; }
        public DateTime FinishTime { get; set; }
        public int CommentStatus { get; set; }
        public DateTime CommentTime { get; set; }
        public string Reason { get; set; }
        public DateTime CreatetTime { get; set; }
        public string DoctorName { get; set; }
        public string ImageURL { get; set; }
        public string DepartmentName { get; set; }
        public string HospitalName { get; set; }
        public string ServiceName { get; set; }
        public string TitleName { get; set; }

    }


    [Serializable]
    public class OrderSelect_Model
    {
        //客户编号
        public string CustomerCode { get; set; }
        //是否用于复查画面 1：复查，2：我的订单
        public int flag { get; set; }
        //我的订单服务状态 0：无条件，1：服务中，2：待支付，3：待评价，4：退款/售后
        public int tag { get; set; }

    }

    [Serializable]
    public class ServiceComment_Model
    {
        //总体评分
        public decimal Overall { get; set; }
        //专业质量评分
        public decimal Profession { get; set; }
        //态度
        public decimal Altitude { get; set; }
        //问题是否解决
        public int IsSolute { get; set; }
        //评价
        public string Comment { get; set; }
        //订单编号
        public string OrderCode { get; set; }
        //医生编号
        public string DoctorCode { get; set; }
        //客户编号
        public string CustomerCode { get; set; }
        //用户ID
        public int UserID { get; set; }

    }

    [Serializable]
    public class BuyService_Model
    {
        //服务编号
        public string ServiceCode { get; set; }
        //客户编号
        public string CustomerCode { get; set; }

    }

    [Serializable]
    public class GetServiceDetail_Model
    {
        //服务编号
        public string ServiceCode { get; set; }

    }

    [Serializable]
    public class BuyServiceInfo_Model
    {
        //服务编号
        public List<ImaService_Model> ImaList { get; set; }
        //客户编号
        public List<CouponMember_Model> CouponList { get; set; }

    }

    [Serializable]
    public class OrderInfo_Model
    {
        //订单编号
        public string OrderCode { get; set; }
        //用户ID
        public int UserID { get; set; }

    }

    [Serializable]
    public class AfterService_Model
    {
        //订单编号
        public string OrderCode { get; set; }
        //售后理由
        public string Reason { get; set; }
        //用户ID
        public int UserID { get; set; }

    }

    [Serializable]
    public class ServiceDetail_Model
    {
        public string ServiceCode { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Introduct { get; set; }
        public decimal OriginPrice { get; set; }
        public decimal PromPrice { get; set; }
        public decimal ExchangePrice { get; set; }
        public string ListImageURL { get; set; }
        //图片列表
        public List<ImaService_Model> ImaList { get; set; }
    }
}
