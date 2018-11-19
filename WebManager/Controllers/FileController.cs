using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebManager.Model;

namespace WebManager.Controllers
{
    public class FileController : BaseController
    {
        public static int Status = -1;
        public string FileUpload()
        {
            Upload_Model model = new Upload_Model();
            model.Code = "0";
            model.Data = null;
            model.Message = "上传失败";
            try
            {
                HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                System.Web.HttpContext.Current.Response.ContentType = "text/html";
               // int type = Common.Util.StringUtils.GetDbInt(System.Web.HttpContext.Current.Request["type"]);
                string fileFolder = "";

                string bx = "";
                if (hfc.Count > 0)
                {
                    if (hfc[0].ContentLength >= 4194304)
                    {

                    }


                    BinaryReader r = new BinaryReader(hfc[0].InputStream);
                    byte buffer = r.ReadByte();
                    bx = buffer.ToString();
                    buffer = r.ReadByte();
                    bx += buffer.ToString();

                    string suffix = "";
                    if (bx == "255216")
                    {
                        suffix = ".jpg";
                    }
                    else if (bx == "7173")
                    {
                        suffix = ".gif";
                    }
                    else if (bx == "6677")
                    {
                        suffix = ".bmp";
                    }
                    else if (bx == "13780")
                    {
                        suffix = ".png";
                    }

                    string fileName = getFileName(suffix);
                    string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + fileName;
                    if (!System.IO.Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "Temp\\"))
                    {
                        System.IO.Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "Temp\\");
                    }
                    hfc[0].SaveAs(filePath);
                    if (System.IO.File.Exists(filePath))
                    {
                        Common.Net.QiNiu.UploadqiNiu(filePath, fileName,out Status);
                    }
                    else
                    {
                        model.Code = "0";
                        model.Message = "上传失败";
                        return JsonConvert.SerializeObject(model);
                    }

                    
                    if (Status == 200)
                    {
                        string url = System.Configuration.ConfigurationManager.AppSettings["Domian"]  + fileName;
                        model.Code = "1";
                        model.Data = url;
                        model.Message = "上传成功";
                        model.FileName = fileName;
                        System.Threading.Tasks.Task.Factory.StartNew(() =>
                        System.IO.File.Delete(filePath)
                        );
                    }
                    
                    return JsonConvert.SerializeObject(model);

                }
                else
                {
                    return JsonConvert.SerializeObject(model);
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(model);
            }
        }

        private string getFileName(string suffix)
        {
            DateTime dt = DateTime.Now.ToLocalTime();
            string randomNumber = "";
            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random(seed);
            for (int j = 0; j < 5; j++)
            {
                randomNumber += random.Next(10).ToString();
            }
            string fileName = string.Format("{0:yyyyMMddHHmmssffff}", dt) + randomNumber + suffix;
            return fileName;
        }
    }
}