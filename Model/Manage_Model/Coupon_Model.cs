using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    [Serializable]
    public   class Coupon_Model
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Detail { get; set; }
        public int Type { get; set; }
        public string Rule { get; set; }
        public int IsDuplicate { get; set; }
        public int IsReuse { get; set; }
        public int ExchangeType { get; set; }
        public decimal ExchangeAmount { get; set; }
        public int MaxQty { get; set; }
        public int Qty { get; set; }
        public int UsedQty { get; set; }
        public int ValidType { get; set; }
        public string ValidRUle { get; set; }
        public int Weights { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
