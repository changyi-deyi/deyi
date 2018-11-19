using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public  class AddMemberOrder_Model
    {
        public string Name { get; set; }
        public string idNumber { get; set; }
        public string CouponCode { get; set; }
        public string WechatOpenID { get; set; }
        public string CustomerCode { get; set; }
        public int MemberLevelID { get; set; }
        public decimal OriginPrice { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal DiscountAmount { get; set; }

        public int UserID { get; set; }


    }

    [Serializable]
    public class AddMemberOrderResult_Model
    {
        public bool Success { get; set; }
        public string OrderCode { get; set; }
        public string NetTradeCode { get; set; }
        public decimal OrderAmount { get; set; }

        public string Message { get; set; }
    }

    [Serializable]
    public class MemberOrderResultApi_Model {
        public string jsParam { get; set; }
        public AddMemberOrderResult_Model order { get; set; }
    }

}
