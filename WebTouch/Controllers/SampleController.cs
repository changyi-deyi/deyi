using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTouch.Controllers
{
    public class SampleController : BaseController
    {
        // GET: Sample
        public ActionResult UploadSample()
        {
            return View();
        }

        [HttpPost]
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
                int type = Common.Util.StringUtils.GetDbInt(System.Web.HttpContext.Current.Request["type"]);
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


                    UploadImage_Model image = new UploadImage_Model();
                    
                    byte[] arr = new byte[hfc[0].InputStream.Length];
                    hfc[0].InputStream.Position = 0;
                    hfc[0].InputStream.Read(arr, 0, (int)hfc[0].InputStream.Length);
                    image.imageString = Convert.ToBase64String(arr);
                    image.Type = type;
                    image.suffix = suffix;

                    string postJson = JsonConvert.SerializeObject(image);
                    string data = string.Empty;
                    bool success = GetPostResponseNoRedirect("Sample", "UploadImage", postJson, out data, true, false);

                    if (success)
                    {
                        model.Code = "1";
                        model.Data = data;
                        model.Message = "上传成功";
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
                //model.Status = 0;
                //return JsonConvert.SerializeObject(model);
                //return Json(model);
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
    [Serializable]
    public class Upload_Model
    {
        public string FileUrl { set; get; }
        public string Code { set; get; }
        public string Message { set; get; }
        public string FileName { set; get; }
        public string Data { set; get; }
    }
    public class UploadImage_Model
    {
        public string imageString { get; set; }
        public int Type { get; set; }
        public string suffix { get; set; }
        
    }

}
