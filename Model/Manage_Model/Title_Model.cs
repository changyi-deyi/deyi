using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    [Serializable]
        
    public class Title_Model
    {
        public int TitleID { get; set; }
        public string TitleName { get; set; }
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
