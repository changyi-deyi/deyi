using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Operate_Model;
using Model.Table_Model;

namespace Model.View_Model
{
    [Serializable]
    public class DoctorList_Model
    {
        //医生姓名
        public string Name { get; set; }
        //医生编号
        public string DoctorCode { get; set; }
        //医生性别
        public int Gender { get; set; }
        //医生职称
        public string TitleName { get; set; }
        //医生照片地址
        public string ImageURL { get; set; }
        //咨询量
        public int ServiceTimes { get; set; }
        //好评率
        public decimal Points { get; set; }
        //医院名称
        public string HospitalName { get; set; }
        //科室名称
        public string DepartmentName { get; set; }
        //是否关注(0：未关注，1：关注)
        public int followFlg { get; set; }
        //标签
        public List<string> tag { get; set; }
    }

    [Serializable]
    public class DoctorDetail_Model
    {
        //医生信息
        public DoctorList_Model DoctorInfo { get; set; }
        //服务信息
        public List<DoctorService_Model> DoctorService { get; set; }
        //医生评价
        public List<OpeComment_Model> DoctorComment { get; set; }
    }
}
