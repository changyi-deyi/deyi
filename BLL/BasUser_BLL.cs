using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;
using Model.Operate_Model;

namespace BLL
{
    public class BasUser_BLL
    {
        #region 构造类实例
        public static BasUser_BLL Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested()
            {
            }
            internal static readonly BasUser_BLL instance = new BasUser_BLL();
        }
        #endregion
        public BasUser_Model GetUser(string LoginUserName)
        {
            return BasUser_DAL.Instance.GetUser(LoginUserName);
        }
        public BasUser_Model GetUserWithOpenID(string WechatOpenID)
        {
            return BasUser_DAL.Instance.GetUserWithOpenID(WechatOpenID);
        }
        public int GetUserCount(string LoginUserName)
        {
            return BasUser_DAL.Instance.GetUserCount(LoginUserName);
        }
        public int Login(BasUser_Model model, Register_Model info)
        {
            return BasUser_DAL.Instance.Login(model,info);
        }
        public int Register(BasUser_Model model, Register_Model info)
        {
            return BasUser_DAL.Instance.Register(model,info);
        }
        public int ChangeMobile(string Mobile, int UserID)
        {
            return BasUser_DAL.Instance.ChangeMobile(Mobile, UserID);
        }
    }
}
