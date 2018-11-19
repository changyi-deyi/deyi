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
    public class InfCustomerProfile_BLL
    {
        #region 构造类实例
        public static InfCustomerProfile_BLL Instance
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
            internal static readonly InfCustomerProfile_BLL instance = new InfCustomerProfile_BLL();
        }
        #endregion
        public List<InfCustomerProfile_Model> GetCustomerProfile(string CustomerCode)
        {
            return InfCustomerProfile_DAL.Instance.GetCustomerProfile(CustomerCode);
        }
        public List<string> GetUploadDate(string CustomerCode, int type)
        {
            return InfCustomerProfile_DAL.Instance.GetUploadDate(CustomerCode, type);        
        }
        public List<CustomerProfile_Model> GetProfileAndImaCount(string CustomerCode, string UploadDate, int type)
        {
            return InfCustomerProfile_DAL.Instance.GetProfileAndImaCount(CustomerCode, UploadDate, type);
        }

        public int ProfileUpload(ProfileUpload_Model model, List<ImageURL_Model> list)
        {
            return InfCustomerProfile_DAL.Instance.ProfileUpload(model,list);
        }
        public int ProfileDelete(int ID, int UserID)
        {
            return InfCustomerProfile_DAL.Instance.ProfileDelete(ID, UserID);
        }
    }
}
