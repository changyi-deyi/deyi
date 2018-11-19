using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    [Serializable]
  public   class MemberService_Model
    {
        public int ID { get; set; }
        public int LevelID { get; set; }
        public string LevelName { get; set; }
        public string ServiceCode { get; set; }
        public decimal Discount { get; set; }
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
