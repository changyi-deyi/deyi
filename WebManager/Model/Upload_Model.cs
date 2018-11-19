using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class Upload_Model
    {
        public string FileUrl { set; get; }
        public string Code { set; get; }
        public string Message { set; get; }
        public string FileName { set; get; }
        public string Data { set; get; }
    }
}