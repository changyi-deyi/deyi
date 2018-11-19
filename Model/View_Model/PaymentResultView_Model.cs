using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.View_Model
{
    [Serializable]
    public  class PaymentResultView_Model
    {
        public int Type { get; set; }
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public int MemberLevelID { get; set; }
        public decimal OrderAmount { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public string PayMentCode { get; set; }
        public int Amount { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public string IDNumber { get; set; }
    }
}
