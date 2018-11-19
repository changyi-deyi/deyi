using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class BasUser_Model
    {
        public int UserID { get; set; }
        public int Type { get; set; }
        public string LoginUserName { get; set; }
        public string Password { get; set; }
        public string WechatOpenID { get; set; }
        public string WecharUnionID { get; set; }
        public DateTime LastLogin { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
