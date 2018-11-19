using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Operate_Model
{
    public class UploadImage_Model
    {
        public string imageString { get; set; }

        public string suffix { get; set; }
        public int Type { get; set; }
    }

    public class ImageURL_Model
    {
        public int ID { get; set; }
        public string FileName { get; set; }

        public string ImageURL { get; set; }
    }

}
