using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Table_Model;

namespace Model.Operate_Model
{

    [Serializable]
    public class GetAdvisoryDetail_Model
    {
        //客户编码
        public string CustomerCode { get; set; }
        //组ID
        public int GroupID { get; set; }
    }

    [Serializable]
    public class AdvisoryDetail_Model
    {
        public int ID { get; set; }
        public string OpCode { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public int OpType { get; set; }
        public int GroupID { get; set; }
        public int IsDone { get; set; }
        public int Status { get; set; }
        public string CreatetTime { get; set; }

        //咨询图片
        public List<ImaAdvisory_Model> AdvisoryIma { get; set; }
    }

    [Serializable]
    public class DeleteAdvisoryDetail_Model
    {
        //客户ID
        public int UserID { get; set; }
        //图片ID
        public int ID { get; set; }
    }
    
    [Serializable]
    public class SubmitAdvisory_Model
    {
        //客户ID
        public int UserID { get; set; }
        //客户编码
        public string CustomerCode { get; set; }
        //咨询内容
        public string Content { get; set; }
        //组ID
        public int GroupID { get; set; }
        //是否继续咨询：1 是，2 不是
        public int ComtinueFlg { get; set; }
        //图片列表
        public List<ImageURL_Model> AdvisoryIma { get; set; }
    }
}
