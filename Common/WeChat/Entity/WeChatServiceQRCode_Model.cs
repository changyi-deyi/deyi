using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.WeChat.Entity
{
    [Serializable]
    public class WeChatServiceQRCode_Model
    {
        public string ticket { get; set; }
        public int expire_seconds { get; set; }
        public string url { get; set; }
    }

    [Serializable]
    public class WeChatServiceCardId_Model
    {
        public string card_id { get; set; }
    }

}
