using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    public class Banner_Model
    {
        public int ID { get; set; }
        public string ImageURL { get; set; }
        /// <summary>
        /// 0：无 1：外链 2：医生 3：产品 4：服务
        /// </summary>
        public int Type { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public int Sort { get; set; }
    }
}
