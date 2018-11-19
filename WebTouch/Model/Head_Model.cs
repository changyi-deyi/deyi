using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTouch.Model
{
    [Serializable]
    public class Head_Model
    {
        public string PrevUrl{get;set;}
        public string CurrentTitle { get; set; }
        public string NextUrl { get; set; }
        public string NextTitle { get; set; }
    }
}