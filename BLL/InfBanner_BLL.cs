using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Table_Model;

namespace BLL
{
    public class InfBanner_BLL
    {
        #region 构造类实例
        public static InfBanner_BLL Instance
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
            internal static readonly InfBanner_BLL instance = new InfBanner_BLL();
        }
        #endregion
        public List<InfBanner_Model> GetInfoBanner()
        {
            return InfBanner_DAL.Instance.GetInfoBanner();
        }

        public List<InfBanner_Model> GetImage()
        {
            return InfBanner_DAL.Instance.GetImage();
        }
    }
}
