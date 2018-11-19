using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Common.Safe
{
    public class CryptDES
    {
        #region DESEncrypt zDES加密

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <param name="changspc">true 替换 +为~</param>
        /// <returns>以Base64格式返回的加密字符串。</returns> 
        public static string DESEncrypt(string pToEncrypt, string sKey, bool changspc)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();

                //以上代码大部分时间运行是正常的，但是加密得出的字符串如果包含"+",用Request.QueryString接收,"+"字符会漏掉，
                //解密的时候就会报Invalid length for a Base-64 char array异常，所以加密后要替换下 
                if (changspc)
                {
                    str = str.Replace("+", "~");
                }

                return str;
            }
        }
        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToDecrypt">要加密的字符串</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string DESEncrypt(string pToDecrypt)
        {
            string sKey = "$#$56575";
            return DESEncrypt(pToDecrypt, sKey);
        }

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string DESEncrypt(string pToEncrypt, string sKey)
        {
            return DESEncrypt(pToEncrypt, sKey, false);
        }


        /// <summary>
        /// 进行DES加密:changspc 为true时对+进行替换
        /// </summary>
        /// <param name="pToDecrypt">要加密的字符串</param>
        /// <param name="changspc">true 替换 +为~</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string DESEncrypt(string pToDecrypt, bool changspc)
        {
            string sKey = "$#$56575";
            return DESEncrypt(pToDecrypt, sKey, changspc);
        }

        #endregion

        #region DESDecrypt DES解密
        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string DESDecrypt(string pToDecrypt, string sKey)
        {
            try
            {
                //以上代码大部分时间运行是正常的，但是加密得出的字符串如果包含"+",用Request.QueryString接收,"+"字符会漏掉，
                //解密的时候就会报Invalid length for a Base-64 char array异常，所以加密后要替换下 
                //解密时就要替换回来
                pToDecrypt = pToDecrypt.Replace("~", "+");

                byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                    des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                        }
                        string str = Encoding.UTF8.GetString(ms.ToArray());

                        return str;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <returns>已解密的字符串</returns>
        public static string DESDecrypt(string pToDecrypt)
        {
            string sKey = "^Sf2c9d#";
            return DESDecrypt(pToDecrypt, sKey);
        }

        public static string GetConnectionString(string strContext)
        {
            return DESDecrypt(strContext, "^Sf2c9d#");
        }
        #endregion 
    }
}
