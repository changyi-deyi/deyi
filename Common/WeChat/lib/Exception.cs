using System;
using System.Collections.Generic;
using System.Web;

namespace Common.WeChat.lib
{
    public class WxPayException : Exception 
    {
        public WxPayException(string msg) : base(msg) 
        {

        }
     }
}