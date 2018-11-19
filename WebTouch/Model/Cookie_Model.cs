using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTouch.Model
{
    [Serializable]
    public class Cookie_Model
    {
        public int UserID { get; set; }
        public int Level { get; set; }
        public string UserName { get; set; }
        public string CustomerCode { get; set; }
        public string MemberCode { get; set; }

        public bool IsSigned { get; set; }
    }
}