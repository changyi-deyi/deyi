using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    [Serializable]
    public  class Feedback_Model
    {
        //客户ID
        public int UserID { get; set; }
        //反馈内容
        public string Content { get; set; }
        //图片1
        public string Image1 { get; set; }
        //图片2
        public string Image2 { get; set; }
        //图片3
        public string Image3 { get; set; }
    }
}
