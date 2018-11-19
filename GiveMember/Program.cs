using BLToolkit.Data;
using Common.Log;
using Model.Table_Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.IO.Compression;
using System.Net.Security;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiveMember
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("开始查询光瀚会员信息");
                //抽取处理对象
                List<InfCustomer_Model> model = GetCustomerInfo();

                Console.WriteLine("光瀚会员信息查询完毕");
                if (model != null && model.Count > 0)
                {

                    Console.WriteLine("开始处理光瀚会员信息");
                    foreach (InfCustomer_Model item in model)
                    {

                        string inData = Newtonsoft.Json.JsonConvert.SerializeObject(item);

                        string o = "";
                        string url = System.Configuration.ConfigurationManager.AppSettings["ApiUrl"] + item.IDNumber;
                        var a = GetResponse(url, inData, out o);
                        if (a != HttpStatusCode.OK)
                        {
                            LogUtil.Log("GiveMember", "response.StatusCode is not success,msg=[" + o + "],KEY=[" + item.IDNumber + "]");
                        }
                        else
                        {
                            if (int.Parse(o) > 0)
                            {
                                int result = GiveMember(item.CustomerCode, item.IDNumber, item.UserID);
                                if (result == 0)
                                {
                                    LogUtil.Log("光瀚会员信息", "光瀚会员信息处理失败");
                                    Console.WriteLine("光瀚会员信息处理失败");
                                    break;
                                }
                            }
                        }
                    }
                    Console.WriteLine("光瀚会员信息处理完毕");
                }
            }
            catch (Exception ex)
            {
                LogUtil.Log(ex, "光瀚会员信息处理失败");
                Console.WriteLine("光瀚会员信息处理失败");
                Console.ReadKey();

            }
        }


        /// <summary>
        /// 用于网络统一的请求
        /// </summary>
        /// <param name="Url">请求的URL地址</param>
        /// <param name="param">请求的参数(用于Post请求)</param>
        /// <param name="Data">响应的值</param>
        /// <param name="Headers">头信息</param>
        /// <param name="ContentType">网络格式</param>
        /// <param name="Method">请求的方法类型</param>
        /// <param name="UserAgent">用户自定义Agent</param>
        /// <param name="encoding">编码方式</param>
        /// <param name="Timeout">超时时间</param>
        /// <param name="proxy">网络代理</param>
        /// <param name="cert">请求证书(用于Https请求)</param>
        /// <returns>网络响应状态码</returns>
        public static HttpStatusCode GetResponse(string Url, string param, out string Data, NameValueCollection Headers = null, string ContentType = "application/json", string Method = "POST", string UserAgent = "", Encoding encoding = null, int Timeout = 120000, WebProxy proxy = null, X509Certificate cert = null)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback
                   += RemoteCertificateValidate;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.DefaultConnectionLimit = 100;
                ServicePointManager.Expect100Continue = false;
                if (!string.IsNullOrWhiteSpace(Url))
                {
                    var webRequest = System.Net.WebRequest.Create(Url) as System.Net.HttpWebRequest;
                    if (cert != null)
                    {
                        webRequest.ClientCertificates.Add(cert);
                    }
                    webRequest.Timeout = Timeout;
                    webRequest.ReadWriteTimeout = Timeout;
                    webRequest.ContentType = !string.IsNullOrWhiteSpace(ContentType) ? ContentType : "application/x-www-form-urlencoded;charset=utf-8";
                    if (Headers != null)
                    {
                        webRequest.Headers.Add(Headers);
                    }
                    webRequest.Method = !string.IsNullOrWhiteSpace(Method) ? Method : "POST";
                    if (!string.IsNullOrWhiteSpace(UserAgent))
                    {
                        webRequest.UserAgent = UserAgent;
                    }
                    if (!string.IsNullOrWhiteSpace(param))
                    {
                        byte[] bs;
                        if (webRequest.Headers["Content-Encoding"] != null && webRequest.Headers["Content-Encoding"].Contains("deflate"))
                        {
                            bs = CompressionObject(param);
                        }
                        else
                        {
                            bs = Encoding.UTF8.GetBytes(param);
                        }
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
                    if (proxy != null)
                    {
                        webRequest.Proxy = proxy;
                    }

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

        public static byte[] CompressionObject(object DataOriginal)
        {
            if (DataOriginal == null) return null;
            BinaryFormatter bFormatter = new BinaryFormatter();
            MemoryStream mStream = new MemoryStream();
            bFormatter.Serialize(mStream, DataOriginal);
            byte[] bytes = mStream.ToArray();
            MemoryStream oStream = new MemoryStream();
            DeflateStream zipStream = new DeflateStream(oStream, CompressionMode.Compress);
            zipStream.Write(bytes, 0, bytes.Length);
            zipStream.Flush();
            zipStream.Close();
            return oStream.ToArray();
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }

        public static List<InfCustomer_Model> GetCustomerInfo()
        {
            using (DbManager db = new DbManager())
            {
                string strSql = @" SELECT  `UserID`
                                          ,`MemberCode`
                                          ,`CustomerCode`
                                          ,`Name`
                                          ,`Mobile`
                                          ,`Gender`
                                          ,`Birthday`
                                          ,`Balance`
                                          ,`IsMember`
                                          ,`IDNumber`
                                          ,`LastSignInDate`
                                          ,`CycleDate`
                                          ,`ContinuousDate`
                                          ,`SignStatus`
                                          ,`Status`
                                     FROM  `inf_customer`
                                    WHERE  `Status` = 1
                                      AND  `IsMember` = 1
                                      AND  `IDNumber` IS NOT NULL ";
                List<InfCustomer_Model> result = db.SetCommand(strSql).ExecuteList<InfCustomer_Model>();
                return result;
            }
        }

        public static int GiveMember(string CustomerCode, string IDNumber, int UserID)
        {
            DateTime dt = DateTime.Now;
            using (DbManager db = new DbManager())
            {
                db.BeginTransaction();
                string strSqlMember = @" SELECT * FROM `inf_member` WHERE `CustomerCode`=@CustomerCode";

                InfMember_Model member = db.SetCommand(strSqlMember
                          , db.Parameter("@CustomerCode", CustomerCode, DbType.String)).ExecuteObject<InfMember_Model>();

                if (member == null)
                {
                    var outParmeter1 = db.OutputParameter("@Result", DbType.String);
                    var sf1 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "Family", DbType.String)
                                      , outParmeter1).ExecuteScalar<string>();
                    string FamilyCode = outParmeter1.Value.ToString();

                    if (string.IsNullOrEmpty(FamilyCode))
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    var outParmeter2 = db.OutputParameter("@Result", DbType.String);
                    var sf2 = db.SetCommand(CommandType.StoredProcedure, "GetSerialNo"
                                      , db.Parameter("@TN", "Inf_Member", DbType.String)
                                      , outParmeter2).ExecuteScalar<string>();
                    string MemberCode = outParmeter2.Value.ToString();

                    if (string.IsNullOrEmpty(MemberCode))
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                    string strSqlMemberInsert = @"INSERT INTO `inf_member` 
                                    (`CustomerCode`,`MemberCode`,`ChannelID`,`LevelID`,`Type`,`FamilyCode`,`MaxQty`,`FreeQty`,`ExpiredDate`,`IDNumber`,`Status`,`CreatetTime`,`Creator`) 
                                    VALUES
                                    (@CustomerCode,@MemberCode,@ChannelID,@LevelID,@Type,@FamilyCode,@MaxQty,@FreeQty,@ExpiredDate,@IDNumber,1,@CreatetTime,@Creator) ";


                    int rows = db.SetCommand(strSqlMemberInsert
                              , db.Parameter("@CustomerCode", CustomerCode, DbType.String)
                              , db.Parameter("@MemberCode", MemberCode, DbType.String)
                              , db.Parameter("@ChannelID", 2, DbType.Int32)
                              , db.Parameter("@LevelID", 1, DbType.Int32)
                              , db.Parameter("@Type", 1, DbType.Int32)
                              , db.Parameter("@FamilyCode", FamilyCode, DbType.String)
                              , db.Parameter("@MaxQty", 1, DbType.Int32)
                              , db.Parameter("@FreeQty", 0, DbType.Int32)
                              , db.Parameter("@ExpiredDate", dt.AddDays(364), DbType.DateTime)
                              , db.Parameter("@IDNumber", IDNumber, DbType.String)
                              , db.Parameter("@CreatetTime", dt, DbType.DateTime)
                              , db.Parameter("@Creator", UserID, DbType.Int32)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }


                    string strSqlCustomer = @"
                            UPDATE 
                              `Inf_Customer` 
                            SET
                              `MemberCode` =@MemberCode,
                              `IsMember` = 2,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater 
                            WHERE `CustomerCode` =@CustomerCode";

                    rows = db.SetCommand(strSqlCustomer
                                , db.Parameter("@MemberCode", MemberCode, DbType.String)
                                , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                                , db.Parameter("@Updater", UserID, DbType.Int32)
                                , db.Parameter("CustomerCode", CustomerCode, DbType.String)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                }
                else
                {
                    string strSqlUpdateMember = @" UPDATE 
                                          `inf_member` 
                                        set
                                          `LevelID` =@LevelID,
                                          `ExpiredDate` =@ExpiredDate,
                                          `UpdateTime` =@UpdateTime,
                                          `Updater` =@Updater 
                                        where `MemberCode` =@MemberCode";

                    int rows = db.SetCommand(strSqlUpdateMember
                            , db.Parameter("@LevelID", 1, DbType.Int32)
                            , db.Parameter("@ExpiredDate", dt.AddDays(364), DbType.DateTime)
                            , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                            , db.Parameter("@Updater", UserID, DbType.Int32)
                            , db.Parameter("@MemberCode", member.MemberCode, DbType.String)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }



                    string strSqlCustomer = @"
                            UPDATE 
                              `Inf_Customer` 
                            SET
                              `MemberCode` =@MemberCode,
                              `IsMember` = 2,
                              `UpdateTime` =@UpdateTime,
                              `Updater` =@Updater 
                            WHERE `CustomerCode` =@CustomerCode";

                    rows = db.SetCommand(strSqlCustomer
                                , db.Parameter("@MemberCode", member.MemberCode, DbType.String)
                                , db.Parameter("@UpdateTime", dt, DbType.DateTime)
                                , db.Parameter("@Updater", UserID, DbType.Int32)
                                , db.Parameter("CustomerCode", CustomerCode, DbType.String)).ExecuteNonQuery();

                    if (rows != 1)
                    {
                        db.RollbackTransaction();
                        return 0;
                    }

                }
                db.CommitTransaction();
                return 1;
            }
        }
    }
}
