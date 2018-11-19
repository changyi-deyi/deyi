using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage_Model
{
    public class MemberAct_Model
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string LinkURL { get; set; }
        public int LevelID { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
        public string LevelName { get; set; }
    }

    public class MemberActInfo_Model
    {
        public int ID { get; set; }
        public int ActID { get; set; }
        public string MemberCode { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string IDNumber { get; set; }
        public int AddressID { get; set; }
        public int HandleSts { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
        public string Address { get; set; }
    }

}
