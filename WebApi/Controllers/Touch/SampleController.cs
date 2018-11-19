using BLL;
using Common.Entity;
using Model.Operate_Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Authorize;

namespace WebApi.Controllers.Touch
{
    public class SampleController : BaseController
    {
        [HttpPost]
        [ActionName("UploadImage")]
        [HTTPBasicAuthorize]
        public HttpResponseMessage UploadImage(JObject obj)
        {
            ObjectResult<ImageURL_Model> res = new ObjectResult<ImageURL_Model>();
            res.Code = "0";
            res.Message = "上传失败";
            res.Data = null;

            ImageURL_Model result = new ImageURL_Model();
            if (obj == null)
            {
                res.Message = "不合法参数";
                return toJson(res);
            }

            string strSafeJson = Common.Util.StringUtils.GetDbString(obj);

            UploadImage_Model model = Newtonsoft.Json.JsonConvert.DeserializeObject<UploadImage_Model>(strSafeJson);

            if (string.IsNullOrEmpty(model.imageString))
            {
                res.Message = "不合法参数";
                return toJson(res);
            }
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["FileData"];

            string fileName = getFileName(model.suffix);

            if (Common_BLL.Instance.saveImg(model.imageString, fileName, filePath)) {
                result.FileName = fileName;
                result.ImageURL = System.Configuration.ConfigurationManager.AppSettings["Domian"] + fileName;

                res.Code = "1";
                res.Message = "上传成功";
                res.Data = result;
            }


            return toJson(res);
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
