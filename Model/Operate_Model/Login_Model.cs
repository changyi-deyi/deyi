using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public  class Register_Model
    {
        //手机号
        public string Mobile { get; set; }
        //浏览器信息
        public string BrowserInfo { get; set; }
        //获得OpenID
        public string OpenID { get; set; }
        //微信获得昵称
        public string Name { get; set; }
        //微信获得性别
        public int Gender { get; set; }
        //验证码
        public int Auth { get; set; }
    }

    [Serializable]
    public class WeChatInfo_Model
    {
        //微信获得昵称
        public string nickname { get; set; }
        //微信获得性别
        public int sex { get; set; }
    }


    [Serializable]
    public class LoginStatus_Model
    {
        public int SignStatus { get; set; }
        public string CustomerCode { get; set; }
        public int LevelID { get; set; }
        public string MemberCode { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        //登陆或注册 1：登陆 2：注册
        public int LoginStatue { get; set; }

    }

}
