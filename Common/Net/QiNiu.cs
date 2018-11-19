using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    public class QiNiu
    {

        public static int Status = -1;
        public static void UploadqiNiu(string filePath, string fileName,out int StatusResult)
        {
            string AK = System.Configuration.ConfigurationManager.AppSettings["QiNiuAK"];
            string SK = System.Configuration.ConfigurationManager.AppSettings["QiNiuSK"];

            // 目标空间名
            string bucket = System.Configuration.ConfigurationManager.AppSettings["QiNiubucket"];

            // 目标文件名
            string saveKey = fileName;

            UploadManager target = new UploadManager();
            Mac mac = new Mac(AK, SK);
            string key = fileName;


            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = bucket;
            putPolicy.SetExpires(3600);
            string token = Auth.createUploadToken(putPolicy, mac);
            UploadOptions uploadOptions = null;

            UpCompletionHandler upCompletionHandler = new UpCompletionHandler(delegate (string fileKey, ResponseInfo respInfo, string response)
            {
                Status = respInfo.StatusCode;
            });

            target.uploadFile(filePath, key, token, uploadOptions, upCompletionHandler);
            StatusResult = Status;

        }
    }
}
