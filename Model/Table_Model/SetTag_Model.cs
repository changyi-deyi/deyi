using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class SetTag_Model
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
