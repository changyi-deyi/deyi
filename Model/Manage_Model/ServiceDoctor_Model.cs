using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
   
    [Serializable]
    public class ServiceDoctor_Model
    {
        public string DoctorCode { get; set; }
        public string DoctorName { get; set; }
        public string ServiceCode { get; set; }
        public int IsBargain { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public int Creator { get; set; }

        public DateTime CreatetTime { get; set; }

        public int? Updater { get; set; }

        public DateTime? UpdateTime { get; set; }
        public string UpperName { get; set; }
    }
}
