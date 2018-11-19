using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class OpeAdvisory_Model
    {
        public int ID { get; set; }
        public string OpCode { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int OpType { get; set; }
        public int GroupID { get; set; }
        public int IsDone { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
