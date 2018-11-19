using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    public class Tag_Model
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public string DoctorCode { get; set; }
    }
}
