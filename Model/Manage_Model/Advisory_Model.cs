using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    public class Advisory_Model
    {
        public int ID { get; set; }
        public string OpCode { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int OpType { get; set; }
        public int GroupID { get; set; }
        public int IsDone { get; set; }
        /// <summary>
        /// 1：有效 2：无效
        /// </summary>
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }

        public List<AdvisoryImg_Model> ImgList { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }

    }

    public class AdvisoryImg_Model
    {
        public int ID { get; set; }
        public string path { get; set; }
        public string FileName { get; set; }
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
