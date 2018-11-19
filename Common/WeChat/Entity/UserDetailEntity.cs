using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WeChat.Entity
{
    [Serializable]
    public class UserDetailEntity
    {
        public int subscribe { get; set; }
        public string openid { get; set; }
        public string nickname { get; set; }
        public int sex { get; set; }
        public string language { get; set; }
        public string city { get; set; }
        public string province { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public long subscribe_time { get; set; }
        public string unionid { get; set; }
        public string remark { get; set; }
        public int groupid { get; set; }
    }

    [Serializable]
    public class WeChatErrorEntity
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
    }
}
