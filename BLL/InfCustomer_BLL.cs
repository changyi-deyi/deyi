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
    public class InfCustomer_BLL
    {
        #region 构造类实例
        public static InfCustomer_BLL Instance
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
            internal static readonly InfCustomer_BLL instance = new InfCustomer_BLL();
        }
        #endregion
        public InfCustomer_Model GetMark(CustomerMessage_Model model)
        {
            return InfCustomer_DAL.Instance.GetMark(model);
        }

        public int SignIn(InfCustomer_Model model)
        {
            return InfCustomer_DAL.Instance.SignIn(model);
        }
        public InfCustomer_Model GetBalance(int UserID, decimal ExchangeAmount)
        {
            return InfCustomer_DAL.Instance.GetBalance(UserID, ExchangeAmount);
        }
        public InfCustomer_Model GetCustomerMember(string CustomerCode)
        {
            return InfCustomer_DAL.Instance.GetCustomerMember(CustomerCode);
        }
        public int NameAuthenticate(string CustomerCode, string Name, string IDNumber, int UserID)
        {
            return InfCustomer_DAL.Instance.NameAuthenticate(CustomerCode, Name, IDNumber, UserID);
        }
        public CustomerInfo_Model GetCustomerInfo(string CustomerCode)
        {
            return InfCustomer_DAL.Instance.GetCustomerInfo(CustomerCode);
        }

        public int CheckCustomer(string IDNumber, int LevelID, List<string> ChannelList)
        {
            return InfCustomer_DAL.Instance.CheckCustomer(IDNumber, LevelID, ChannelList);
        }
    }
}
