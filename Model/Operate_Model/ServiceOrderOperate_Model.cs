using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public class AddServiceOrder_Model
    {
        public string OrderCode { get; set; }
        public string DoctorCode { get; set; }
        public string ServiceCode { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string LocatedCity { get; set; }
        public string Description { get; set; }
        public DateTime ExpectTime { get; set; }
        public string CouponCode { get; set; }
        public decimal OriginAmount { get; set; }
        public decimal OrderAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public int AmountType { get; set; }
        public decimal DepositAmount { get; set; }
        public bool DiscountFlg { get; set; }
        public List<string> ImgList { get; set; }

        public int UserID { get; set; }
        public int LevelID { get; set; }
        public string WechatOpenID { get; set; }
        public decimal PayAmount { get; set; }


    }

    [Serializable]
    public class AddServiceOrderResult_Model
    {
        public bool Success { get; set; }
        public string OrderCode { get; set; }
        public string NetTradeCode { get; set; }
        public decimal PayAmount { get; set; }

        public string Message { get; set; }
    }

    [Serializable]
    public class ServiceOrderResultApi_Model
    {
        public string jsParam { get; set; }
        public AddServiceOrderResult_Model order { get; set; }
    }
}
