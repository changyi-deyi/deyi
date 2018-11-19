using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class InfDoctor_Model
    {
        public int UserID { get; set; }
        public string DoctorCode { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public int HospitalID { get; set; }
        public int DepartmentID { get; set; }
        public int TitleID { get; set; }
        public string GoodAt { get; set; }
        public string Introduction { get; set; }
        public string Phone { get; set; }
        public string ImageURL { get; set; }
        public int Type { get; set; }
        public int ServiceTimes { get; set; }
        public decimal Points { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
