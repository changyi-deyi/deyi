using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class InfMemberServiceDiscount_Model
    {
        public int ID { get; set; }
        public int LevelID { get; set; }
        public string ServiceCode { get; set; }
        public decimal Discount { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
