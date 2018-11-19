using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Net;
using DAL;
using Model.Operate_Model;
using Model.Table_Model;

namespace BLL
{
    public class InfMember_BLL
    {
        #region 构造类实例
        public static InfMember_BLL Instance
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
            internal static readonly InfMember_BLL instance = new InfMember_BLL();
        }
        #endregion

        public InfMember_Model GetMember(string CustomerCode)
        {
            return InfMember_DAL.Instance.GetMember(CustomerCode);
        }

        public InfMember_Model GetCustomerLevel(int UserID)
        {
            return InfMember_DAL.Instance.GetCustomerLevel(UserID);
        }

        public List<InfFamily_Model> GetMemberFamily(string CustomerCode)
        {
            return InfMember_DAL.Instance.GetMemberFamily(CustomerCode);
        }

        public int SetMember(ExchangeMember_Model model, int ChannelID, int LevelID)
        {
            return InfMember_DAL.Instance.SetMember(model, ChannelID, LevelID);
        }


        public AddMemberOrderResult_Model addMemberOrder(AddMemberOrder_Model model)
        {
            return InfMember_DAL.Instance.addMemberOrder(model);
        }


        public int UpdatePayMemberOrderResult(string NetTradeCode, string Data, int mode)
        {
            return InfMember_DAL.Instance.UpdatePayMemberOrderResult(NetTradeCode, Data, mode);
        }


        public int CancelPayMember(OpeMemberOrder_Model model)
        {
            return InfMember_DAL.Instance.CancelPayMember(model);
        }


    }
}
