using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;

namespace Model.Operate_Model
{
    [Serializable]
    public  class Member_Model
    {
        public string CustomerCode { get; set; }
    }

    [Serializable]
    public class MemberLevel_Model
    {
        public int LevelID { get; set; }
    }

    [Serializable]
    public class ExchangeMember_Model
    {
        //卡号
        public string CardNumber { get; set; }
        //密码
        public string Passeord { get; set; }
        //身份证号
        public string IDNumber { get; set; }
        //姓名
        public string Name { get; set; }
        //客户编号
        public string CustomerCode { get; set; }
        //用户ID
        public int UserID { get; set; }
    }

    [Serializable]
    public class OpenMember_Model
    {
        //开通会员等级
        public int LevelID { get; set; }
        //客户会员等级
        public int CustomerLevelID { get; set; }
        //客户编号
        public string CustomerCode { get; set; }
    }

    [Serializable]
    public class OpenMemberInfo_Model
    {
        //会员等级
        public SetMemberLevel_Model MemberLevel { get; set; }
        //收获地址
        public List<InfAddress_Model>  addressInfo { get; set; }
        //可用优惠券列表
        public List<CouponMember_Model> CouponInfo { get; set; }
    }


    [Serializable]
    public class Authenticate_Model
    {
        //客户编号
        public string CustomerCode { get; set; }
        //用户ID
        public int UserID { get; set; }
        //姓名
        public string Name { get; set; }
        //身份证号
        public string IDNumber { get; set; }
    }

    [Serializable]
    public class ChangeMobile_Model
    {
        //手机号
        public string Mobile { get; set; }
        //验证码
        public int Auth { get; set; }
        //用户ID
        public int UserID { get; set; }
    }
}
