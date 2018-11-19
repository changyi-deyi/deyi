using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public class Act_Model
    {
        //活动ID
        public int ActID { get; set; }
        //姓名
        public string Name { get; set; }
        //性别
        public int Gender { get; set; }
        //年龄
        public int Age { get; set; }
        //电话
        public string Phone { get; set; }
        //身份证号
        public string IDNumber { get; set; }
        //地址ID
        public int AddressID { get; set; }
        //会员编号
        public string MemberCode { get; set; }
        //客户ID
        public int UserID { get; set; }

    }
}
