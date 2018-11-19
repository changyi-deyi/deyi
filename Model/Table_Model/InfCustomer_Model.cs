using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class InfCustomer_Model
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
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
