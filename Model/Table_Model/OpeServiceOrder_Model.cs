using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class OpeServiceOrder_Model
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
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
