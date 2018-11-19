using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;

namespace Common.Util
{
    #region Check Type

    //"^\\d+$"　　//非负整数（正整数 + 0） 
    //"^[0-9]*[1-9][0-9]*$"　　//正整数 
    //"^((-\\d+)|(0+))$"　　//非正整数（负整数 + 0） 
    //"^-[0-9]*[1-9][0-9]*$"　　//负整数 
    //"^-?\\d+$"　　　　//整数
    //"^\\d+(\\.\\d+)?$"　　//非负浮点数（正浮点数 + 0） 
    //"^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$"　　//正浮点数 
    //"^((-\\d+(\\.\\d+)?)|(0+(\\.0+)?))$"　　//非正浮点数（负浮点数 + 0） 
    //"^(-(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*)))$"　　//负浮点数 
    //"^(-?\\d+)(\\.\\d+)?$"　　//浮点数 
    //"^[A-Za-z]+$"　　//由26个英文字母组成的字符串 
    //"^[A-Z]+$"　　//由26个英文字母的大写组成的字符串 
    //"^[a-z]+$"　　//由26个英文字母的小写组成的字符串 

    //"^[A-Za-z0-9]+$"　　//由数字和26个英文字母组成的字符串 
    //"^\\w+$"　　//由数字、26个英文字母或者下划线组成的字符串 
    //"^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$"　　　　//email地址 
    //"^[a-zA-z]+://(\\w+(-\\w+)*)(\\.(\\w+(-\\w+)*))*(\\?\\S*)?$"　　//url
    public enum CheckType
    {
        Nonnegative = 0,
        Positive = 1,
        NegativePlusZero = 2,
        Negative = 3,
        Integer = 4,

        PositiveFloatPlusZero = 5,
        PositiveFloat = 6,

        NegativeFloatPlusZero = 7,
        NegativeFloat = 8,

        Float = 9,

        EnglishChar = 10,
        EnglishCharLowerCase = 11,
        EnglishCharUpCase = 12,

        EnglishNumber = 13,
        EnglishNumberUnderline = 14,

        EmailAddress = 15,
        URL = 16,

        ChineseMobile = 17,//中国手机号
        ChineseIDNo = 18,//中国身份证号带字母
        IPAddress = 19,//IP地址
        NomalMoney = 20,//普通2位小数的价格
        ChinesePostCode = 21,//中国邮政编码
        TelPhone = 22,//电话号码
    }
    #endregion

    public class StringUtils
    {
        /// <summary>
        /// 根据正则表达式检查
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool CheckStringFormat(string input, CheckType type)
        {
            Regex regex;
            switch (type)
            {
                case CheckType.Nonnegative:
                    regex = new Regex("^\\d+$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.Positive:
                    regex = new Regex("^[0-9]*[1-9][0-9]*$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.NegativePlusZero:
                    regex = new Regex("^((-\\d+)|(0+))$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.Negative:
                    regex = new Regex("^-[0-9]*[1-9][0-9]*$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.Integer:
                    regex = new Regex("^-?\\d+$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.PositiveFloatPlusZero:
                    regex = new Regex("^\\d+(\\.\\d+)?$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.PositiveFloat:
                    regex = new Regex("^(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*))$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.NegativeFloatPlusZero:
                    regex = new Regex("^((-\\d+(\\.\\d+)?)|(0+(\\.0+)?))$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.NegativeFloat:
                    regex = new Regex("^(-(([0-9]+\\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\\.[0-9]+)|([0-9]*[1-9][0-9]*)))$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.Float:
                    regex = new Regex("^(-?\\d+)(\\.\\d+)?$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.EnglishChar:
                    regex = new Regex("^[A-Za-z]+$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.EnglishCharUpCase:
                    regex = new Regex("^[A-Z]+$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.EnglishCharLowerCase:
                    regex = new Regex("^[a-z]+$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.EnglishNumber:
                    regex = new Regex("^[A-Za-z0-9]+$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.EnglishNumberUnderline:
                    regex = new Regex("^\\w+$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.EmailAddress:
                    regex = new Regex("^[\\w-]+(\\.[\\w-]+)*@[\\w-]+(\\.[\\w-]+)+$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.URL:
                    regex = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.ChineseMobile:
                    regex = new Regex(@"^1\\d{10}$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.ChineseIDNo:
                    regex = new Regex(@"(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.IPAddress:
                    regex = new Regex(@"(\d+)\.(\d+)\.(\d+)\.(\d+)", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.NomalMoney:
                    regex = new Regex(@"^[0-9]+(\.[0-9]{2})?$", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.ChinesePostCode:
                    regex = new Regex(@"[1-9]\\d{5}(?!\d)", RegexOptions.Compiled);
                    return regex.IsMatch(input);

                case CheckType.TelPhone:
                    regex = new Regex(@"^\d{3,4}-\d{7,8}$|^\d{3,4}-\d{7,8}-\d{1,5}$", RegexOptions.Compiled);
                    return regex.IsMatch(input);
                default:
                    return false;
            }
        }
        /// <summary>
        /// 替换HTML标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string StripHtmlXmlTags(string content)
        {
            return Regex.Replace(content, "<[^>]+>", "", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///转全角的函数(SBC case)
        ///任意字符串
        ///全角字符串
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToSBC(String input)
        {
            // 半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new String(c);
        }

        /// <summary>
        ///转半角的函数(DBC case)
        ///任意字符串
        ///半角字符串
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }

        /// <summary>
        /// 判断字符串中是否存在全角字符，汉字
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public static int CheckExistSBC(string check)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = check.ToCharArray();

            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    return i;
                }
            }

            return 0;
        }

        /// <summary>
        /// 从首字符取被切字符串的定长字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetSubString(string input, int length)
        {
            //双字节中文字符集ASSI编码范围
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = input.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;

            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength >= length)
                {
                    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 取用掩码替换字符串指定位置以后字符
        /// 如手机，地址，邮件地址等在Page上的显示
        /// </summary>
        /// <param name="input"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetStringWithMark(string input, int fromPosition)
        {
            char[] stringChar = input.ToCharArray();
            //如果替换起始位置大于字符长度
            //返回原字符
            if (fromPosition >= stringChar.Length)
            {
                return input;
            }

            StringBuilder sb = new StringBuilder();
            int nLength = 0;

            for (int i = 0; i < stringChar.Length; i++)
            {
                if (nLength >= fromPosition)
                {
                    sb.Append("*");
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength++;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取URL内容　UTF8编码
        /// </summary>
        /// <param name="ContentURL">URL地址</param>
        /// <returns></returns>
        public static string GetContent(string ContentURL)
        {
            WebClient _client = new WebClient();
            _client.BaseAddress = ContentURL;
            _client.Headers.Add("Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*");
            _client.Headers.Add("Accept-Language", "zh-cn");
            _client.Headers.Add("UA-CPU", "x86");
            _client.Headers.Add("User-Agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)");
            System.IO.Stream objStream = _client.OpenRead(ContentURL);
            System.IO.StreamReader _read = new System.IO.StreamReader(objStream, System.Text.Encoding.UTF8);
            return _read.ReadToEnd();


            //try
            //{
            //    Encoding enc = Encoding.UTF8;
            //    //Encoding enc = Encoding.Default;
            //    Uri uri = new Uri(ContentURL);

            //    HttpWebRequest hwreq = (HttpWebRequest)WebRequest.Create(uri);
            //    hwreq.ContentType = "application/x-www-form-urlencoded";
            //    hwreq.Method = "GET";

            //    HttpWebResponse hwrsp = (HttpWebResponse)hwreq.GetResponse();

            //    if (hwrsp.ContentLength > 0)
            //    {
            //        byte[] bts = new byte[(int)hwrsp.ContentLength];

            //        Stream s = hwrsp.GetResponseStream();

            //        for (int i = 0; i < bts.Length; )
            //        {
            //            i += s.Read(bts, i, bts.Length - i);
            //        }

            //        string content = enc.GetString(bts);

            //        return content;
            //    }

            //    return "";
            //}
            //catch (Exception ex)
            //{                
            //    LogUtil.Log("StringUtil", (ex.InnerException == null ? ex.Message.ToString() : ex.InnerException.Message.ToString()));
            //    return "";
            //}
        }


        /// <summary>
        /// 编码　默认编码
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static string GBKUrlEncode(string k)
        {
            return System.Web.HttpUtility.UrlEncode(k, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 解码　默认编码
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public static string GBKUrlDecode(string k)
        {
            return System.Web.HttpUtility.UrlDecode(k, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 返回Bool类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetDbBool(object obj)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return false;
            }
            else
            {
                string tempd = obj.ToString().TrimEnd('%');
                return bool.Parse(tempd);
            }
        }

        /// <summary>
        /// 转为Bool类型
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool GetDbBool(string text)
        {
            return GetDbBool(text, false);
        }
        /// <summary>
        /// 转为时间类型
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static DateTime SafeDateTime(string text, DateTime defaultValue)
        {
            DateTime time;
            if (DateTime.TryParse(text, out time))
            {
                defaultValue = time;
            }
            return defaultValue;
        }

        /// <summary>
        /// 转为Int类型 
        /// </summary>
        /// <param name="text"></param> 
        /// <returns></returns>
        public static int SafeInt(string text)
        {
            return SafeInt(text, 0);
        }

        /// <summary>
        /// 转为Bool类型
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool GetDbBool(string text, bool defaultValue)
        {
            bool flag;
            if (bool.TryParse(text, out flag))
            {
                defaultValue = flag;
            }
            return defaultValue;
        }

        /// <summary>
        /// 替换不安全字符
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string SafeCode(string Str)
        {
            string text1 = "" + Str;
            if (text1 != "")
            {
                text1 = text1.Replace("<", "&lt");
                text1 = text1.Replace(">", "&gt");
                //text1 = text1.Replace(",", "，");
                text1 = text1.Replace("'", "‘");
                text1 = text1.Replace("\"", "＂");
                //text1 = text1.Replace("update", "");
                //text1 = text1.Replace("insert", "");
                //text1 = text1.Replace("delete", "");

                Regex reg = new Regex("update|insert|delete|alert|javascript", RegexOptions.IgnoreCase);
                text1 = reg.Replace(text1, "");


                text1 = text1.Replace("--", "");
                text1 = text1.Replace("%", "");
                text1 = text1.Replace(";", "");
                //text1 = text1.Replace(",", "");

                //text1 = text1.Replace("alert", "");
                //text1 = text1.Replace("javascript", "");
            }
            return text1;
        }


        /// <summary>
        /// 组合数组
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ToDelimitedString(ICollection collection, string delimiter)
        {
            if (collection == null)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            if (collection is Hashtable)
            {
                foreach (object obj2 in ((Hashtable)collection).Keys)
                {
                    builder.Append(obj2.ToString() + delimiter);
                }
            }
            if (collection is ArrayList)
            {
                foreach (object obj3 in (ArrayList)collection)
                {
                    builder.Append(obj3.ToString() + delimiter);
                }
            }
            if (collection is string[])
            {
                foreach (string str in (string[])collection)
                {
                    builder.Append(str + delimiter);
                }
            }
            if (collection is MailAddressCollection)
            {
                foreach (MailAddress address in (MailAddressCollection)collection)
                {
                    builder.Append(address.Address + delimiter);
                }
            }
            return builder.ToString().TrimEnd(new char[] { Convert.ToChar(delimiter, CultureInfo.InvariantCulture) });
        }

        /// <summary>
        /// 返回整数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetDbInt(object obj)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        /// <summary>
        /// 返回整数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetDbInt(string obj)
        {
            int temp;
            if (string.IsNullOrEmpty(obj))
            {
                return 0;
            }
            else if (!int.TryParse(obj.Replace(",", ""), out temp))
            {
                return 0;
            }
            return int.Parse(obj.Replace(",", ""));
        }

        /// <summary>
        /// 返回整数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetDbInt(string obj, int defaultValue)
        {
            int temp;
            if (string.IsNullOrEmpty(obj))
            {
                return defaultValue;
            }
            else if (!int.TryParse(obj.Replace(",", ""), out temp))
            {
                return defaultValue;
            }
            return int.Parse(obj.Replace(",", ""));
        }

        public static long GetDbLong(string obj, long defaultValue = 0)
        {
            long temp;
            if (string.IsNullOrEmpty(obj))
            {
                return defaultValue;
            }
            else if (!long.TryParse(obj.Replace(",", ""), out temp))
            {
                return defaultValue;
            }
            return long.Parse(obj.Replace(",", ""));
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDbString(object obj)
        {
            if (obj == null || obj == System.DBNull.Value || obj == "\0" || obj == "\\0")
            {
                return String.Empty;
            }
            else
            {
                return obj.ToString().Trim();
            }
        }

        /// <summary>
        /// 返回字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDbString(object obj, string defaultValue)
        {
            if (obj == null || obj == System.DBNull.Value || obj == "\0" || obj == "\\0")
            {
                return defaultValue;
            }
            else
            {
                return obj.ToString().Trim();
            }
        }

        /// <summary>
        /// 返回日期
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDbDateTime(object obj)
        {
            if (obj == null || obj == System.DBNull.Value)
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(obj);
            }
        }

        /// <summary>
        /// 返回日期
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDbDateTime(object obj, DateTime defaultValue)
        {
            if (obj == null || obj == System.DBNull.Value)
            {
                return defaultValue;
            }
            else
            {
                DateTime time;
                if (DateTime.TryParse(obj.ToString(), out time))
                {
                    defaultValue = time;
                }
                return defaultValue;
            }
        }

        /// <summary>
        /// 返回Decimal类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Decimal GetDbDecimal(object obj)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                string tempd = obj.ToString().TrimEnd('%');
                return decimal.Parse(tempd);
            }
        }

        /// <summary>
        /// 返回Decimal类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static Decimal GetDbDecimal(object obj, decimal defaultValue)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return defaultValue;
            }
            else
            {
                string tempd = obj.ToString();
                return decimal.Parse(tempd);
            }
        }

        /// <summary>
        /// 返回Decimal类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Decimal GetDbDecimal(string obj)
        {
            decimal dec;
            if (obj == null || !decimal.TryParse(obj, out dec))
            {
                return 0;
            }
            else
            {
                return dec;
            }
        }

        /// <summary>
        /// 返回double类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetDbDouble(object obj)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                string tempd = obj.ToString().TrimEnd('%');
                return double.Parse(tempd);
            }
        }

        /// <summary>
        /// 返回double类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static double GetDbDouble(object obj, double defaultValue)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return defaultValue;
            }
            else
            {
                string tempd = obj.ToString();
                return double.Parse(tempd);
            }
        }

        /// <summary>
        /// 返回double类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetDbDouble(string obj)
        {
            double dec;
            if (obj == null || !double.TryParse(obj, out dec))
            {
                return 0;
            }
            else
            {
                return dec;
            }
        }

        /// <summary>
        /// 返回float类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float GetDbFloat(object obj)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                string tempd = obj.ToString().TrimEnd('%');
                return float.Parse(tempd);
            }
        }

        /// <summary>
        /// 返回double类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static float GetDbFloat(object obj, float defaultValue)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return defaultValue;
            }
            else
            {
                string tempd = obj.ToString();
                return float.Parse(tempd);
            }
        }

        /// <summary>
        /// 返回float类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float GetDbFloat(string obj)
        {
            float dec;
            if (obj == null || !float.TryParse(obj, out dec))
            {
                return 0;
            }
            else
            {
                return dec;
            }
        }

        /// <summary>
        /// 返回Int16类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int16 GetDbInt16(object obj)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                string tempd = obj.ToString().TrimEnd('%');
                return Int16.Parse(tempd);
            }
        }

        /// <summary>
        /// 返回Int16类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static Int16 GetDbInt16(object obj, Int16 defaultValue)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return defaultValue;
            }
            else
            {
                string tempd = obj.ToString();
                return Int16.Parse(tempd);
            }
        }

        /// <summary>
        /// 返回Int16类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Int16 GetDbInt16(string obj)
        {
            Int16 dec;
            if (obj == null || !Int16.TryParse(obj, out dec))
            {
                return 0;
            }
            else
            {
                return dec;
            }
        }

        /// <summary>
        /// 返回byte类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte GetDbByte(object obj)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return 0;
            }
            else
            {
                string tempd = obj.ToString().TrimEnd('%');
                return byte.Parse(tempd);
            }
        }
        /// <summary>
        /// 返回Int16类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static byte GetDbByte(object obj, byte defaultValue)
        {
            if (obj == null || obj == System.DBNull.Value || obj.ToString() == "")
            {
                return defaultValue;
            }
            else
            {
                string tempd = obj.ToString();
                return byte.Parse(tempd);
            }
        }
        /// <summary>
        /// 返回Int16类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte GetDbByte(string obj)
        {
            byte dec;
            if (obj == null || !byte.TryParse(obj, out dec))
            {
                return 0;
            }
            else
            {
                return dec;
            }
        }
        /// <summary>
        /// 返回长整型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetDbLong(object obj)
        {
            if (obj == null || obj == System.DBNull.Value)
            {
                return 0;
            }
            else
            {
                long lId = 0;
                long.TryParse(obj.ToString(), out lId);
                return lId;
            }
        }
        /// <summary>
        /// 返回日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string date)
        {
            DateTime dt;
            if (DateTime.TryParse(date, out dt))
                return dt;
            return DateTime.MinValue;
        }
        /// <summary>
        /// 返回日期
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object obj)
        {
            DateTime dt;
            if (obj == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                if (DateTime.TryParse(obj.ToString(), out dt))
                {
                    return dt;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
        }
        /// <summary>
        /// 截取字符串　按字节计算
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="nBytes"></param>
        /// <param name="type">0 添加...</param>
        /// <returns></returns>
        public static string TrimByteStr(object obj, int nBytes, int type)
        {
            nBytes = nBytes * 2;
            if (obj == null)
            {
                return "";
            }
            if (nBytes <= 0)
                return "";

            if (nBytes % 2 != 0)
                nBytes++;

            byte[] blist = System.Text.Encoding.GetEncoding("Gb2312").GetBytes(obj.ToString());
            if (blist.Length > nBytes)
            {
                if (type == 0)
                {
                    return System.Text.Encoding.GetEncoding("Gb2312").GetString(blist, 0, nBytes).Replace("?", "") + "...";
                }
                else
                {
                    return System.Text.Encoding.GetEncoding("Gb2312").GetString(blist, 0, nBytes).Replace("?", "");
                }
            }
            else
            {
                return obj.ToString();
            }
            /*if (nBytes > blist.Length)
                nBytes = blist.Length;
            if (type == 0)
            {
                return System.Text.Encoding.GetEncoding("Gb2312").GetString(blist, 0, nBytes) + "...";
            }
            else
            {
                return System.Text.Encoding.GetEncoding("Gb2312").GetString(blist, 0, nBytes);
            }*/
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="len"></param>
        /// <param name="type">0 添加...</param>
        /// <returns></returns>
        public static string TrimStr(object obj, int len, int type)
        {
            if (obj != null)
            {
                if (obj.ToString().Length > len)
                {
                    if (type == 0)
                    {
                        return obj.ToString().Substring(0, len) + "...";
                    }
                    else
                    {
                        return obj.ToString().Substring(0, len);
                    }
                }
                else
                {
                    return obj.ToString();
                }
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 去除HTML代码
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string StripHT(object strHtml)
        {
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            string strOutput = regex.Replace(strHtml.ToString(), "");
            return strOutput.Replace("&nbsp;", "");
        }
        /// <summary>
        /// 检查字符串是否是电子邮件地址
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsEmail(string request)
        {
            if (request == null)
                return false;
            Regex regex = new Regex(@"^[_a-zA-Z0-9\-]+(\.[_a-zA-Z0-9\-]*)*@[a-zA-Z0-9\-]+([\.][a-zA-Z0-9\-]+)+$");
            return regex.IsMatch(request.Trim());
        }
        /// <summary>
        /// 值是否为手机号
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsMobile(string request)
        {
            //Regex reg = new Regex(@"^1(?:3|5|8)\d{9}$");
            Regex reg = new Regex(@"^1[3,4,5,8][0-9]{9,9}$");
            if (reg.IsMatch(request))
            {
                return true;
            }
            return false;
        }

        public static bool IsTelephone(string request)
        {
            //Regex reg = new Regex(@"^1(?:3|5|8)\d{9}$");
            Regex reg = new Regex(@"^\d{3,4}-\d{7,8}$|^\d{3,4}-\d{7,8}-\d{1,5}$");
            if (reg.IsMatch(request))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断输入的字符是否是英文字母
        /// </summary>
        public static bool IsLetter(string str, bool isAddr)
        {
            string strReg = string.Empty;
            if (isAddr)
            {
                //联系地址可以输入#
                strReg = @"^[\w\.\#\/\s-]+$";
            }
            else
            {
                strReg = @"^[\w\.\/\s]+$";
            }
            Regex regex = new Regex(strReg);
            return regex.IsMatch(str);
        }
        /// <summary>
        /// 判断国外联系电话是否合法
        /// </summary>
        public static bool IsEnPhone(string str)
        {
            Regex regex = new Regex(@"^[\d-]{6,20}$");
            return regex.IsMatch(str);
        }
        /// <summary>
        /// 判断国外邮编是否合法
        /// </summary>
        public static bool IsEnPostCode(string str)
        {
            Regex regex = new Regex(@"^[\d\w-\s]{5,20}$");
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 转为Int类型 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int SafeInt(string text, int defaultValue)
        {
            int num;
            if (int.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }
        /// <summary>
        /// 转为Decimal类型
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal SafeDecimal(string text, decimal defaultValue)
        {
            decimal num;
            if (decimal.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        /// <summary>
        /// 检查字符串是否是日期
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsDateTimeAvailable(string request)
        {
            return IsDateTimeAvailable(request, DateTime.MinValue, DateTime.MaxValue);
        }

        /// <summary>
        /// 检查字符串是否是指定的日期
        /// </summary>
        /// <param name="request"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static bool IsDateTimeAvailable(string request, DateTime minValue, DateTime maxValue)
        {
            DateTime req_Time;
            try
            {
                req_Time = DateTime.Parse(request);
            }
            catch
            {
                return false;
            }
            if (req_Time > maxValue || req_Time < minValue)
                return false;

            return true;
        }

        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SafeStr(string text)
        {
            return SafeStr(text, string.Empty);
        }
        /// <summary>
        /// 转为字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string SafeStr(string text, string defaultValue)
        {
            if (String.IsNullOrEmpty(text))
            {
                return defaultValue;
            }
            return text.ToString();
        }
        /// <summary>  
        /// 返回本对象的Json序列化  
        /// </summary>  
        /// <param name="obj"></param>  
        /// <returns></returns>  
        public static string ToJson(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj).Replace("\"", "'");
        }
        /// <summary>
        /// 验证身份证号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUserIdentityNumAvailable(string id)
        {
            if (id == null)
                return false;
            id = id.Trim();

            Match match = Regex.Match(id, @"\d{18}|\d{17}\w");
            if (!match.Success)
                match = Regex.Match(id, @"\d{15}");
            if (!match.Success)
                return false;
            if (match.Value != id)
                return false;

            return true;
        }

        /// <summary>
        /// 生成随机密码
        /// </summary>
        /// <param name="chars"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRadomPassword(string chars, int length)
        {
            string temp = "";
            int charIndex;
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                charIndex = rnd.Next(chars.Length);
                temp += chars[charIndex];
            }

            return temp;
        }

        /// <summary>
        /// 替换延时加载图片
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string GetLazyImgSrc(string Content)
        {
            return Regex.Replace(Content, "(?<=<img[^>]*?) src=", " src=\"http://i2.mbscss.com/global/grey.gif\" data-original=", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 替换部分延时加载图片
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string GetPartLazyImgSrc(string Content)
        {
            if (Content.IndexOf("<!--LazyBegin") > -1)
            {
                Content = Content.Replace("\n", "");
                string tempstr = "";
                string tempstrnew = "";
                Content = Content.Replace("\n", "");
                Regex regex = new Regex(@"<!--LazyBegin-->.*?<!--LazyEnd-->");
                MatchCollection matchList = regex.Matches(Content);

                if (matchList != null)
                {
                    foreach (Match m in matchList)
                    {
                        tempstr = m.Value;
                        tempstrnew = Regex.Replace(tempstr, "(?<=<img[^>]*?) src=", " src=\"http://i2.mbscss.com/global/grey.gif\" data-original=", RegexOptions.IgnoreCase);
                        Content = Content.Replace(tempstr, tempstrnew);
                    }
                }


                //Content = Regex.Replace(Content, "(?<=<!--LazyBegin(\\d?)-->.*?<img[^>]*?) src=(?=.*?<!--LazyEnd\\1-->)", " src=\"http://i2.mbscss.com/global/grey.gif\" data-original=", RegexOptions.IgnoreCase);
            }
            return Content;
        }
        /// <summary>
        /// 替换延时加载图片　前20张不替换
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string GetBottomLazyImgSrc(string Content)
        {
            return GetBottomLazyImgSrc(Content, 20);
        }
        /// <summary>
        /// 替换延时加载图片　前Count张不替换
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string GetBottomLazyImgSrc(string Content, int Count)
        {
            Content = Regex.Replace(Content, "(?<=<img[^>]*?) src=", " src=\"http://i2.mbscss.com/global/grey.gif\" data-original=", RegexOptions.IgnoreCase);

            Regex reg = new Regex("src=\"http://i2.mbscss.com/global/grey.gif\" data-original=", RegexOptions.IgnoreCase);
            Content = reg.Replace(Content, "src=", Count);
            if (Content.IndexOf("<!--NoLazyBegin") > -1)
            {
                Content = GetNoLazyImgSrc(Content);
            }
            return Content;
        }


        /// <summary>
        /// 替换没有延时加载图片
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string GetNoLazyImgSrc(string Content)
        {
            string tempstr = "";
            string tempstrnew = "";
            Content = Content.Replace("\n", "");
            Regex regex = new Regex(@"<!--NoLazyBegin-->.*?<!--NoLazyEnd-->");
            MatchCollection matchList = regex.Matches(Content);

            if (matchList != null)
            {
                foreach (Match m in matchList)
                {
                    tempstr = m.Value;
                    tempstrnew = Regex.Replace(tempstr, "(?<=<img[^>]*?) src=\"http://i2.mbscss.com/global/grey.gif\" data-original=", " src=", RegexOptions.IgnoreCase);
                    Content = Content.Replace(tempstr, tempstrnew);
                }
            }

            //Content = Regex.Replace(Content, "(?<=<!--NoLazyBegin(\\d?)-->.*?<img[^>]*?) src=\"http://i2.mbscss.com/global/grey.gif\" data-original=(?=.*?<!--NoLazyEnd\\1-->)", " src=", RegexOptions.IgnoreCase);
            return Content;
        }


        /// <summary>
        /// 左侧补0
        /// </summary>
        /// <param name="code"></param>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetPadLeftCode(object code, int len)
        {
            return GetPadLeftCode(code, len, '0');
        }
        /// <summary>
        /// 左侧补preChar字符
        /// </summary>
        /// <param name="code"></param>
        /// <param name="len"></param>
        /// <param name="preChar">替补字符</param>
        /// <returns></returns>
        public static string GetPadLeftCode(object code, int len, char preChar)
        {
            string tmpcode = StringUtils.GetDbString(code);
            if (tmpcode.Length < len)
            {
                tmpcode = tmpcode.PadLeft(len, preChar);
            }

            return tmpcode;
        }

        #region Xml转Json
        /// <summary>
        /// XmlDocument转换成Json格式
        /// </summary>
        /// <param name="xmlDoc">XmlDocument对象</param>
        /// <returns>Json字符串</returns>
        public static string XmlToJson(XmlDocument xmlDoc)
        {
            StringBuilder sbJSON = new StringBuilder();
            sbJSON.Append("{ ");
            XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, true);
            sbJSON.Append("}"); return sbJSON.ToString();
        }

        #region XML转换成Json格式 私有方法
        //  XmlToJSONnode:  Output an XmlElement, possibly as part of a higher array 
        private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName)
        {
            if (showNodeName)
                sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
            sbJSON.Append("{");
            // Build a sorted list of key-value pairs     
            //  where   key is case-sensitive nodeName    
            //          value is an ArrayList of string or XmlElement 
            //  so that we know whether the nodeName is an array or not.    
            SortedList childNodeNames = new SortedList();
            //  Add in all node attributes     
            if (node.Attributes != null)
                foreach (XmlAttribute attr in node.Attributes)
                    StoreChildNode(childNodeNames, attr.Name, attr.InnerText);
            //  Add in all nodes     
            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode is XmlText)
                    StoreChildNode(childNodeNames, "value", cnode.InnerText);
                else if (cnode is XmlElement)
                    StoreChildNode(childNodeNames, cnode.Name, cnode);
            }        // Now output all stored info 
            foreach (string childname in childNodeNames.Keys)
            {
                ArrayList alChild = (ArrayList)childNodeNames[childname];
                if (alChild.Count == 1)
                    OutputNode(childname, alChild[0], sbJSON, true);
                else
                {
                    sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                    foreach (object Child in alChild)
                        OutputNode(childname, Child, sbJSON, false);
                    sbJSON.Remove(sbJSON.Length - 2, 2);
                    sbJSON.Append(" ], ");
                }
            }
            sbJSON.Remove(sbJSON.Length - 2, 2);
            sbJSON.Append(" }");
        }
        //  StoreChildNode: Store data associated with each nodeName   
        //                  so that we know whether the nodeName is an array or not.   
        private static void StoreChildNode(SortedList childNodeNames, string nodeName, object nodeValue)
        {        // Pre-process contraction of XmlElement-s    
            if (nodeValue is XmlElement)
            {
                // Convert  <aa></aa> into "aa":null          
                //          <aa>xx</aa> into "aa":"xx"       
                XmlNode cnode = (XmlNode)nodeValue;
                if (cnode.Attributes.Count == 0)
                {
                    XmlNodeList children = cnode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && (children[0] is XmlText))
                        nodeValue = ((XmlText)(children[0])).InnerText;
                }
            }
            // Add nodeValue to ArrayList associated with each nodeName      
            // If nodeName doesn't exist then add it      
            object oValuesAL = childNodeNames[nodeName];
            ArrayList ValuesAL;
            if (oValuesAL == null)
            {
                ValuesAL = new ArrayList();
                childNodeNames[nodeName] = ValuesAL;
            }
            else
                ValuesAL = (ArrayList)oValuesAL;
            ValuesAL.Add(nodeValue);
        }
        private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName)
        {
            if (alChild == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                sbJSON.Append("null");
            }
            else if (alChild is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                string sChild = (string)alChild;
                sChild = sChild.Trim();
                sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
            }
            else
                XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName);
            sbJSON.Append(", ");
        }
        // Make a string safe for JSON   
        private static string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                { sbOut.Append('\\'); }

                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }
        #endregion
        #endregion

        #region Json转XML对象
        /// <summary>
        /// json字符串转换为Xml对象
        /// </summary>
        /// <param name="sJson">Json字符串</param>
        /// <returns>XmlDocument对象</returns>
        public static XmlDocument JsonToXml(string sJson)
        {
            //XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(sJson), XmlDictionaryReaderQuotas.Max);
            //XmlDocument doc = new XmlDocument();
            //doc.Load(reader);

            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            Dictionary<string, object> Dic = (Dictionary<string, object>)oSerializer.DeserializeObject(sJson);
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDec;
            xmlDec = doc.CreateXmlDeclaration("1.0", "gb2312", "yes");
            doc.InsertBefore(xmlDec, doc.DocumentElement);
            XmlElement nRoot = doc.CreateElement("root");
            doc.AppendChild(nRoot);
            foreach (KeyValuePair<string, object> item in Dic)
            {
                XmlElement element = doc.CreateElement(item.Key);
                KeyValue2Xml(element, item);
                nRoot.AppendChild(element);
            }
            return doc;
        }

        private static void KeyValue2Xml(XmlElement node, KeyValuePair<string, object> Source)
        {
            object kValue = Source.Value;
            if (kValue.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> item in kValue as Dictionary<string, object>)
                {
                    XmlElement element = node.OwnerDocument.CreateElement(item.Key);
                    KeyValue2Xml(element, item);
                    node.AppendChild(element);
                }
            }
            else if (kValue.GetType() == typeof(object[]))
            {
                object[] o = kValue as object[];
                for (int i = 0; i < o.Length; i++)
                {
                    XmlElement xitem = node.OwnerDocument.CreateElement("Item");
                    KeyValuePair<string, object> item = new KeyValuePair<string, object>("Item", o[i]);
                    KeyValue2Xml(xitem, item);
                    node.AppendChild(xitem);
                }

            }
            else
            {
                XmlText text = node.OwnerDocument.CreateTextNode(kValue.ToString());
                node.AppendChild(text);
            }
        }

        #endregion

        #region Xml对象转换String
        /// <summary>
        /// XmlDocument对象转换String
        /// </summary>
        /// <param name="xmlDoc">XmlDocument对象</param>
        /// <returns>String</returns>
        public static string XmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);

            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            StringBuilder sb = new StringBuilder();
            while (!sr.EndOfStream)
            {
                string temp = sr.ReadLine();
                sb.Append(temp == "<root>" ? "" : temp == "</root>" ? "" : temp);
            }

            sr.Close();
            stream.Close();

            return sb.ToString();
        }
        #endregion

        #region dataTable转换成Json格式
        /// <summary>  
        /// dataTable转换成Json格式  
        /// </summary>  
        /// <param name="dt"></param>  
        /// <returns></returns>  
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(dt.TableName);
            jsonBuilder.Append("\":[");
            jsonBuilder.Append("[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }

        #endregion dataTable转换成Json格式


        /// <summary>
        /// 阿拉伯数字转化位大写金额
        /// </summary>
        /// <param name="num">阿拉伯数字</param>
        /// <returns></returns>
        public static string convertToChineseMoney(decimal num)
        {
            string strChina = "零壹贰叁肆伍陆柒捌玖";    //0-9所对应的汉字
            string strUnit = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字
            string strSingleNum = "";    //从原num值中取出的值
            string strNum = ""; //数字的字符串形式
            string strResult = "";    //人民币大写金额形式
            string chChina = "";    //数字的汉语读法
            string chUnit = "";    //数字位的汉语读法

            int i;    //循环变量
            int lenth;    //num的值乘以100的字符串长度
            int nZero = 0;    //用来计算连续的零值是几个
            int temp;    //从原num值中取出的值
            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数
            strNum = ((long)(num * 100)).ToString();    //将num乘100并转换成字符串形式
            lenth = strNum.Length;    //找出最高位

            if (lenth > 15)
            {
                return "位数过大，无法转换！";
            }
            //取出对应位数的strUnit的值。如：200.55,lenth为5所以strUnit=佰拾元角分
            strUnit = strUnit.Substring(15 - lenth);
            //循环取出每一位需要转换的值
            for (i = 0; i < lenth; i++)
            {
                strSingleNum = strNum.Substring(i, 1);    //取出需转换的某一位的值
                temp = Convert.ToInt32(strSingleNum);    //转换为数字
                if (i != (lenth - 3) && i != (lenth - 7) && i != (lenth - 11) && i != (lenth - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时
                    if (strSingleNum == "0")
                    {
                        chChina = "";
                        chUnit = "";
                        nZero = nZero + 1;
                    }
                    else
                    {
                        if (strSingleNum != "0" && nZero != 0)
                        {
                            chChina = "零" + strChina.Substring(temp, 1);
                            chUnit = strUnit.Substring(i, 1);
                            nZero = 0;
                        }
                        else
                        {
                            chChina = strChina.Substring(temp, 1);
                            chUnit = strUnit.Substring(i, 1);
                            nZero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位
                    if (strSingleNum != "0" && nZero != 0)
                    {
                        chChina = "零" + strChina.Substring(temp, 1);
                        chUnit = strUnit.Substring(i, 1);
                        nZero = 0;
                    }
                    else
                    {
                        if (strSingleNum != "0" && nZero == 0)
                        {
                            chChina = strChina.Substring(temp, 1);
                            chUnit = strUnit.Substring(i, 1);
                            nZero = 0;
                        }
                        else
                        {
                            if (strSingleNum == "0" && nZero >= 3)
                            {
                                chChina = "";
                                chUnit = "";
                                nZero = nZero + 1;
                            }
                            else
                            {
                                if (lenth >= 11)
                                {
                                    chChina = "";
                                    nZero = nZero + 1;
                                }
                                else
                                {
                                    chChina = "";
                                    chUnit = strUnit.Substring(i, 1);
                                    nZero = nZero + 1;
                                }
                            }
                        }
                    }
                }

                if (i == (lenth - 11) || i == (lenth - 3))
                {
                    //如果该位是亿位或元位，则必须写上
                    chUnit = strUnit.Substring(i, 1);
                }

                strResult = strResult + chChina + chUnit;
                if (i == lenth - 1 && strSingleNum == "0")
                {
                    //最后一位（分）为0时，加上“整”
                    strResult = strResult + '整';
                }
            }
            if (num == 0)
            {
                strResult = "零元整";
            }
            return strResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="context"></param>
        public static void ObjectToJson(object obj, HttpContext context)
        {
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        /// <summary> 
        /// 输入Float格式数字，将其转换为货币表达方式 
        /// </summary> 
        /// <param name="ftype">货币表达类型：0=人民币；1=港币；2=美钞；3=英镑；4=不带货币;其它=不带货币表达方式</param> 
        /// <param name="fmoney">传入的int数字</param> 
        /// <returns>返回转换的货币表达形式</returns> 
        public static string ConvertCurrency(decimal fmoney, int ftype = 1)
        {
            CultureInfo cul = null;
            string _rmoney = string.Empty;
            try
            {
                switch (ftype)
                {
                    case 0:
                        cul = new CultureInfo("zh-CN");//中国大陆 
                        _rmoney = fmoney.ToString("c", cul);
                        break;
                    case 1:
                        cul = new CultureInfo("zh-HK");//香港 
                        _rmoney = fmoney.ToString("c", cul);
                        break;
                    case 2:
                        cul = new CultureInfo("zh-TW");//台湾 
                        _rmoney = fmoney.ToString("c", cul);
                        break;
                    case 3:
                        cul = new CultureInfo("en-US");//美国 
                        _rmoney = fmoney.ToString("c", cul);
                        break;
                    case 4:
                        cul = new CultureInfo("en-GB");//英国 
                        _rmoney = fmoney.ToString("c", cul);
                        break;
                    case 5:
                        cul = new CultureInfo("sq-AL");//英国 
                        _rmoney = fmoney.ToString("c", cul);
                        break;

                    default:
                        _rmoney = string.Format("{0:n}", fmoney);
                        break;
                }
            }
            catch
            {
                _rmoney = "";
            }
            return _rmoney;
        }

    }
}
