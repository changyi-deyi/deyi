using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.View_Model
{
    [Serializable]
    public class WXJsConfig_Model
    {
        public string appid { get; set; }
        public string timestamp { get; set; }
        public string noncestr { get; set; }
        public string signature { get; set; }

    }


    [Serializable]
    public class WXShare_Model
    {
        public string url { get; set; }
        public string jsParam { get; set; }
    }
}
