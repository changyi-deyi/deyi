using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Operate_Model;

namespace BLL
{
    public class InfFeedback_BLL
    {
        #region 构造类实例
        public static InfFeedback_BLL Instance
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
            internal static readonly InfFeedback_BLL instance = new InfFeedback_BLL();
        }
        #endregion
        public int SubmitFeedback(Feedback_Model model)
        {
            return InfFeedback_DAL.Instance.SubmitFeedback(model);
        }
    }
}
