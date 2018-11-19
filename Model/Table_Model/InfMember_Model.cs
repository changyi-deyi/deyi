using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class InfMember_Model
    {
        public string CustomerCode { get; set; }
        public string MemberCode { get; set; }
        public int ChannelID { get; set; }
        public int LevelID { get; set; }
        public int Type { get; set; }
        public string FamilyCode { get; set; }
        public int MaxQty { get; set; }
        public int FreeQty { get; set; }
        public DateTime ExpiredDate { get; set; }
        public string IDNumber { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
