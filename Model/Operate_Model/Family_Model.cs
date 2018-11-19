using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    public class FamilyInfo_Model
    {
        //家庭编号
        public string FamilyCode { get; set; }
        //证件号码
        public string IDNumber { get; set; }
        //姓名
        public string Name { get; set; }
        //关系
        public string Relationship { get; set; }
        //空余人数
        public int FreeQty { get; set; }
    }

    public class GetFamilyInfo_Model
    {
        //客户编号
        public string CustomerCode { get; set; }
    }
    
    public class ChangeFamily_Model
    {
        //客户编号
        public string CustomerCode { get; set; }
        //客户ID
        public int UserID { get; set; }
        //家庭信息ID
        public int ID { get; set; }
        //证件号码
        public string IDNumber { get; set; }
        //姓名
        public string Name { get; set; }
        //关系
        public string Relationship { get; set; }
        //添加/修改：1 添加，2 修改
        public int ChangeFlg { get; set; }
    }

}
