using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;
using Model.View_Model;
using Model.Operate_Model;

namespace BLL
{
    public class OpeAdvisory_BLL
    {
        #region 构造类实例
        public static OpeAdvisory_BLL Instance
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
            internal static readonly OpeAdvisory_BLL instance = new OpeAdvisory_BLL();
        }
        #endregion
        public List<OpeAdvisory_Model> GetAdvisoryList(string CustomerCode)
        {
            return OpeAdvisory_DAL.Instance.GetAdvisoryList(CustomerCode);
        }
        public List<AdvisoryDetail_Model> GetAdvisoryDetail(string CustomerCode, int GroupID)
        {
            return OpeAdvisory_DAL.Instance.GetAdvisoryDetail(CustomerCode, GroupID);
        }
        public int SubmitAdvisory(SubmitAdvisory_Model model)
        {
            return OpeAdvisory_DAL.Instance.SubmitAdvisory(model);
        }
    }
}
