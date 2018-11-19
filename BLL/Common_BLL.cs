using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Net;

namespace BLL
{
    public  class Common_BLL
    {
        #region 构造类实例
        public static Common_BLL Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested()
            {
            }
            internal static readonly Common_BLL instance = new Common_BLL();
        }
        #endregion

        public static int Status = -1;
        public bool saveImg(string imageString, string fileName, string fileFloder)
        {
            if (!Directory.Exists(fileFloder))
            {
                Directory.CreateDirectory(fileFloder);
            }

            if (imageString != null)
            {
                string fileUrl = fileFloder + fileName;
                Byte[] imageByte = Convert.FromBase64String(imageString);

         

                using (MemoryStream ms = new MemoryStream(imageByte))
                {
                    FileStream fs = new FileStream(fileUrl, FileMode.Create);
                    ms.WriteTo(fs);
                    ms.Close();
                    fs.Close();
                    fs = null;
                }
                if (System.IO.File.Exists(fileUrl))
                {
                    QiNiu.UploadqiNiu(fileUrl, fileName,out Status);
                }
                else
                {
                    return false;
                }

                if (Status == 200)
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    System.IO.File.Delete(fileUrl)
                    );
                }

            }
            return true;
        }
    }
}
