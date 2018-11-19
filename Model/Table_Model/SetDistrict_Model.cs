using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class SetDistrict_Model
    {
        public int REGION_ID { get; set; }
        public string REGION_CODE { get; set; }
        public string REGION_NAME { get; set; }
        public string PARENT_ID { get; set; }
        public string REGION_LEVEL { get; set; }
        public string REGION_ORDER { get; set; }
        public string REGION_NAME_EN { get; set; }
        public string REGION_SHORTNAME_EN { get; set; }
    }
}
