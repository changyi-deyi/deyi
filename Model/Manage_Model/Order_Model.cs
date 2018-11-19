using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    [Serializable]
    public class MemberOrder_Model
    {
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public string MemberCode { get; set; }
        public int Type { get; set; }
        public int Qty { get; set; }
        public decimal OriginAmount { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string LevelName { get; set; }
        public string CouponCode { get; set; }
        public int PaymentStatus { get; set; }
        public int OrderStatus { get; set; }
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public List<Payment_Model> listPayment { get; set; }
        public string NetTradeCode { get; set; }
    }


    [Serializable]
    public class ServiceOrder_Model {
        public string OrderCode { get; set; }
        public string MemberCode { get; set; }
        public string CustomerCode { get; set; }
        public string ServiceCode { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string LocatedCity { get; set; }
        public DateTime ExpectTime { get; set; }

        public DateTime? ArrangedTime { get; set; }
        public string DoctorCode { get; set; }
        public string ReceptionistCode { get; set; }
        public string ReceptionistPhone { get; set; }
        public string Address { get; set; }
        public string SafeCode { get; set; }
        public int AmountType { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal Unpaid { get; set; }
        public int PaymentStatus { get; set; }
        public DateTime? PaymentTime { get; set; }
        public int ServiceStatus { get; set; }
        public DateTime ServiceTime { get; set; }
        public int OrderStatus { get; set; }
        public DateTime? FinishTime { get; set; }
        public int CommentStatus { get; set; }
        public DateTime CommentTime { get; set; }
        public int Status { get; set; }
        public int UpperID { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public string CustomerName { get; set; }
        public string DoctorName { get; set; }
        public string ServiceName { get; set; }
        public string HospitalName { get; set; }
        public string DepartmentName { get; set; }
        public string DoctorMobile { get; set; }
        public string ReceptionistName { get; set; }
        

        public List<Payment_Model> listPayment {get;set; }
        public string NetTradeCode { get; set; }

        public OpeComment_Model comment { get; set; }

    }

    [Serializable]
    public class Payment_Model {

        public string PayMentCode { get; set; }
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public int Type { get; set; }
        public string RelatedPaymentCode { get; set; }
        public int Source { get; set; }
        public decimal OrderAmount { get; set; }
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }

        public List<PaymentDetail_Model> listPaymentDetail {get;set;}
    }


    [Serializable]
    public class PaymentDetail_Model
    {

        public string PayMentCode { get; set; }
        public string DetailCode { get; set; }
        public int Mode { get; set; }
        public decimal PaidAmount { get; set; }
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
    }



}
