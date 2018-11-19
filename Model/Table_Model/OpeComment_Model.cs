using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class OpeComment_Model
    {
        public int ID { get; set; }
        public string OrderCode { get; set; }
        public string CustomerCode { get; set; }
        public string DoctorCode { get; set; }
        public decimal Point { get; set; }
        public string Comment { get; set; }
        public decimal Overall { get; set; }
        public decimal Profession { get; set; }
        public decimal Altitude { get; set; }
        public int IsSolute { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }


        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public string CreatetTimeForView { get; set; }
        public string UpdateTimeForView { get; set; }
    }
}
