using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Common.Net
{
    public class NetUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="param"></param>
        /// <param name="Data"></param>
        /// <param name="Headers"></param>
        /// <param name="ContentType"></param>
        /// <param name="Method"></param>
        /// <param name="UserAgent"></param>
        /// <param name="encoding"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public static HttpStatusCode GetResponse(string Url, string param, out string Data, NameValueCollection Headers = null, string ContentType = "application/json", string Method = "POST", string UserAgent = "", Encoding encoding = null, int Timeout = 120000, WebProxy proxy = null, X509Certificate cert = null)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback
                   += RemoteCertificateValidate;
                if (!string.IsNullOrWhiteSpace(Url))
                {
                    var webRequest = System.Net.WebRequest.Create(Url) as System.Net.HttpWebRequest;
                    if (cert != null)
                    {
                        webRequest.ClientCertificates.Add(cert);
                    }
                    webRequest.Timeout = Timeout;
                    webRequest.ReadWriteTimeout = Timeout;
                    webRequest.ContentType = !string.IsNullOrWhiteSpace(ContentType) ? ContentType : "application/x-www-form-urlencoded";
                    if (Headers != null)
                    {
                        webRequest.Headers.Add(Headers);
                    }
                    webRequest.Method = !string.IsNullOrWhiteSpace(Method) ? Method : "POST";
                    if (!string.IsNullOrWhiteSpace(UserAgent))
                    {
                        webRequest.UserAgent = UserAgent;
                    }

                    if (proxy != null)
                    {
                        webRequest.Proxy = proxy;
                    }

                    if (!string.IsNullOrWhiteSpace(param))
                    {
                        byte[] bs = Encoding.UTF8.GetBytes(param);
                        webRequest.ContentLength = bs.Length;
                        using (Stream reqStream = webRequest.GetRequestStream())
                        {
                            reqStream.Write(bs, 0, bs.Length);
                        }
                    }
                    else
                    {
                        if (webRequest.Method.ToUpper().Equals("POST"))
                        {
                            webRequest.ContentLength = 0;
                            webRequest.GetRequestStream();
                        }
                    }
                    //if (proxy!=null)
                    //{
                    //    webRequest.Proxy = proxy;
                    //}

                    using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
                    {
                        Stream responseStream = response.GetResponseStream();
                        string retString;
                        using (
                            StreamReader streamReader = new StreamReader(responseStream, encoding: encoding ?? Encoding.UTF8))
                        {
                            retString = streamReader.ReadToEnd();
                            streamReader.Close();
                            if (responseStream != null) responseStream.Close();
                        }
                        Data = retString;
                        return response.StatusCode;
                    }

                }
                else
                {
                    Data = "URL丢失";
                    return HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                if (ex is System.Net.WebException)
                {
                    WebException webEx = (WebException)ex;
                    Data = webEx.Message;
                    return ((System.Net.HttpWebResponse)(webEx.Response)).StatusCode;
                }
                else
                {
                    Data = ex.Message;
                    return HttpStatusCode.InternalServerError;
                }
            }
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }


    }
}
