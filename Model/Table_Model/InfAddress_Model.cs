using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class InfAddress_Model
    {
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public int ProvinceID { get; set; }
        public int CityID { get; set; }
        public int DistrictID { get; set; }
        public string Address { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public int IsDefault { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }

        //省名
        public string ProvinceName { get; set; }
        //市名
        public string CityName { get; set; }
        //区名
        public string DistrictName { get; set; }
    }
}
