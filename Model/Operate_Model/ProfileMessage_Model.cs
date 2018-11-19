using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public  class ProfileMessage_Model
    {
        public string CustomerCode { get; set; }
        public int Type { get; set; }
    }

    [Serializable]
    public class ProfileUpload_Model
    {
        public List<ImageURL_Model> UploadImage { get; set; }
        public string CustomerCode { get; set; }
        public string RelatedCustomerCode { get; set; }
        public int Type { get; set; }
        public int UserID { get; set; }
    }
    
}
