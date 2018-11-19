using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public  class Doctor_Model
    {
        //客户编号
        public string CustomerCode { get; set; }
        //科室ID（可不填）
        public int DepartmentID { get; set; }
        //是否从服务页面跳转
        public bool service { get; set; }
        //服务编号
        public string ServiceCode { get; set; }
        //是否关注：1是，2不是
        public int followFlg { get; set; }
    }

    [Serializable]
    public class FollowDoctor_Model
    {
        //客户编号
        public string CustomerCode { get; set; }
        //医生编号
        public string DoctorCode { get; set; }
    }

    [Serializable]
    public class DoctorDetailIn_Model
    {
        //医生编号
        public string DoctorCode { get; set; }
        //会员等级ID
        public int LevelID { get; set; }
        //客户编号
        public string CustomerCode { get; set; }
    }

    [Serializable]
    public class DoctorService_Model
    {
        //服务编号
        public string ServiceCode { get; set; }
        //服务名
        public string Name { get; set; }
        //服务简介
        public string Summary { get; set; }
        //服务介绍
        public string Introduct { get; set; }
        //价格类型
        public int PriceType { get; set; }
        //原价
        public decimal OriginPrice { get; set; }
        //优惠价
        public decimal PromPrice { get; set; }
        //兑换价
        public decimal ExchangePrice { get; set; }
        //一览图片URL
        public string ListImageURL { get; set; }
        //医生编号
        public string DoctorCode { get; set; }
        //是否议价
        public int IsBargain { get; set; }
        //价格(医生服务表)
        public string Price { get; set; }
        //价格(画面原价)
        public decimal trueprice { get; set; }
        //是否折扣
        public bool DiscountFlg { get; set; }
        //折扣
        public decimal Discount { get; set; }
        //画面折扣价格
        public decimal DiscountPrice { get; set; }

    }
}
