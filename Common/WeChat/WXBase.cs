using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WeChat
{
    public class WXBase
    {
        public string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        [Serializable]
        public class WXTokenRS
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

        [Serializable]
        public class WXTicketRS
        {
            public int errcode { get; set; }
            public string errmsg { get; set; }
            public string ticket { get; set; }
            public int expires_in { get; set; }
        }

        [Serializable]
        public class WXJSConfig
        {
            public string appid { get; set; }
            public string noncestr { get; set; }
            public string timestamp { get; set; }
            public string signature { get; set; }
            public string ticket { get; set; }
        }

        [Serializable]
        public class WeChatActiveCard_Model
        {
            /// <summary>
            /// 会员卡编号，由开发者填入，作为序列号显示在用户的卡包里。可与Code码保持等值。
            /// </summary>
            public string membership_number { get; set; }
            /// <summary>
            /// 领取会员卡用户获得的code 
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 卡券ID,自定义code卡券必填
            /// </summary>
            public string card_id { get; set; }
            /// <summary>
            /// 商家自定义会员卡背景图
            /// </summary>
            public string background_pic_url { get; set; }
            /// <summary>
            /// 激活后的有效起始时间。若不填写默认以创建时的 data_info 为准。Unix时间戳格式。
            /// </summary>
            public int activate_begin_time { get; set; }
            /// <summary>
            /// 激活后的有效截至时间。若不填写默认以创建时的 data_info 为准。Unix时间戳格式。
            /// </summary>
            public int activate_end_time { get; set; }
            /// <summary>
            /// 初始积分，不填为0。
            /// </summary>
            public int init_bonus { get; set; }
            /// <summary>
            /// 积分同步说明。
            /// </summary>
            public string init_bonus_record { get; set; }
            /// <summary>
            /// 初始余额，不填为0。
            /// </summary>
            public int init_balance { get; set; }
            /// <summary>
            /// 创建时字段custom_field1定义类型的初始值，限制为4个汉字，12字节。
            /// </summary>
            public string init_custom_field_value1 { get; set; }
            /// <summary>
            /// 创建时字段custom_field2定义类型的初始值，限制为4个汉字，12字节。
            /// </summary>
            public string init_custom_field_value2 { get; set; }
            /// <summary>
            /// 创建时字段custom_field3定义类型的初始值，限制为4个汉字，12字节。
            /// </summary>
            public string init_custom_field_value3 { get; set; }

        }
    }
}
