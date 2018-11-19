using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public  class CustomerProfile_Model
    {
        public int ID { get; set; }
        public string CustomerCode { get; set; }
        public string RelatedCustomerCode { get; set; }
        public int Type { get; set; }
        public DateTime UploadTime { get; set; }
        public int Status { get; set; }
        //图片数量
        public int ImaCount { get; set; }
        public int UserID { get; set; }
    }

    [Serializable]
    public class DeleteProfileIma_Model
    {
        //图片ID
        public int ID { get; set; }
        public int UserID { get; set; }
    }


    [Serializable]
    public class ProfileList_Model
    {
        public string UploadDate { get; set; }

        public List<CustomerProfile_Model> UploadData { get; set; }
    }
}
