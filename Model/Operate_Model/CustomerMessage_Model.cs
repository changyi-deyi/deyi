using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public  class CustomerMessage_Model
    {
        public int UserID { get; set; }
        public string CustomerCode { get; set; }
    }

    [Serializable]
    public class MessageDelete_Model
    {
        public int UserID { get; set; }
        public int MessageID { get; set; }
    }

    [Serializable]
    public class CustomerInfo_Model
    {
        public int UserID { get; set; }
        public string MemberCode { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string HeadImgURL { get; set; }
        public string Mobile { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public decimal Balance { get; set; }
        public int IsMember { get; set; }
        public string IDNumber { get; set; }
        public DateTime LastSignInDate { get; set; }
        public int CycleDate { get; set; }
        public int ContinuousDate { get; set; }
        public int SignStatus { get; set; }
        //会员等级
        public int LevelID { get; set; }
        //会员名称
        public string LevelName { get; set; }

    }
}
