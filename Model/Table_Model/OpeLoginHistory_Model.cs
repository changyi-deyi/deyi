using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class OpeLoginHistory_Model
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime LoginTime { get; set; }
        public string BrowserInfo { get; set; }
    }
}
