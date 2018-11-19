using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class OpeCustomerMessage_Model
    {
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
        public string URL { get; set; }
        public DateTime SendTime { get; set; }
        public int IsRead { get; set; }
        public DateTime ReadTime { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
