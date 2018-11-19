using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{

    [Serializable]
    public class SaveAddress_Model
    {
        //地址ID(更新删除必填)
        public int ID { get; set; }
        //姓名
        public string Name { get; set; }
        //电话
        public string Phone { get; set; }
        //省ID
        public int ProvinceID { get; set; }
        //市ID
        public int CityID { get; set; }
        //区ID
        public int DistrictID { get; set; }
        //详细地址
        public string Address { get; set; }
        //是否默认
        public int IsDefault { get; set; }
        //是否新增:1 新建，2 更新，3删除
        public int IsNew { get; set; }
        //客户编号
        public string CustomerCode { get; set; }
        //客户ID
        public int UserID { get; set; }

    }

    [Serializable]
    public class GetDistrict
    {
        //地区级别
        public string REGION_LEVEL { get; set; }
        //上级Code
        public string PARENT_ID { get; set; }

    }
}
