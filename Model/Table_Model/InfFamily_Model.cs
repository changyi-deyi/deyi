using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class InfFamily_Model
    {
        public int ID { get; set; }
        public string FamilyCode { get; set; }
        public string IDNumber { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
