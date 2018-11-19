using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{

    public class RamdonUtil
    {
        public static List<string> createPassword(int count, int width)
        {
            List<string> result = new List<string>();
            string chars = "23456789abcdefghijkmnpqrstuvwxyz";

            while (result.Count != count)
            {
                Random randrom = new Random((int)DateTime.Now.Ticks);
                string str = "";
                for (int i = 0; i < width; i++)
                {
                    str += chars[randrom.Next(chars.Length)];//randrom.Next(int i)返回一个小于所指定最大值的非负随机数
                }
                if (!IsNumber(str) && !IsLetter(str))//判断是否全是数字
                {
                    if (!result.Contains(str))
                    {
                        result.Add(str);
                    }
                }
            }
            return result;
        }

        //判断是否全是数字
        static bool IsNumber(string str)
        {
            if (str.Trim("0123456789".ToCharArray()) == "")
                return true;
            return false;
        }
        //判断是否全是字母
        static bool IsLetter(string str)
        {
            if (str.Trim("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray()) == "")
                return true;
            return false;
        }

        public static List<string> createNumber(int count, int width)
        {
            string chars = "0123456789";
            List<string> result = new List<string>();
            while (result.Count != count)
            {
                Random randrom = new Random((int)DateTime.Now.Ticks);
                string str = "";
                for (int i = 0; i < width; i++)
                {
                    str += chars[randrom.Next(chars.Length)];//randrom.Next(int i)返回一个小于所指定最大值的非负随机数
                }

                if (!result.Contains(str))
                {
                    result.Add(str);
                }
            }
            return result;
        }


    }
}
