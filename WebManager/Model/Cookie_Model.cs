using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class Cookie_Model
    {
        public int UserID { get; set; }
        public int Type { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int Role { get; set; }

    }
}