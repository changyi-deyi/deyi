using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class OpeMemberOrder_Model
    {
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public int MemberLevelID { get; set; }
        public int Type { get; set; }
        public int Qty { get; set; }
        public decimal OriginAmount { get; set; }
        public decimal OrderAmount { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public int OrderStatus { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
        public string NetTradeCode { get; set; }
    }
}
