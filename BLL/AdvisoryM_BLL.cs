using DAL;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AdvisoryM_BLL
    {
        #region 构造类实例
        public static AdvisoryM_BLL Instance
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
            internal static readonly AdvisoryM_BLL instance = new AdvisoryM_BLL();
        }

        #endregion

        public List<Advisory_Model> getAdvisoryList(string CustomerCode, string CustomerName, int IsDone,  int StartCount = 0, int EndCount = 99999999)
        {
            return AdvisoryM_DAL.Instance.getAdvisoryList(CustomerCode, CustomerName, IsDone, StartCount, EndCount);
        }


        public List<Advisory_Model> getGroupAdvisoryList(int ID)
        {
            return AdvisoryM_DAL.Instance.getGroupAdvisoryList(ID);
        }


        public int AnswerAdvisory(Advisory_Model model)
        {
            return AdvisoryM_DAL.Instance.AnswerAdvisory(model);
        }

        }
}
