using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class TitleM_BLL
    {
        #region 构造类实例
        public static TitleM_BLL Instance
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
            internal static readonly TitleM_BLL instance = new TitleM_BLL();
        }

        #endregion

        public List<Title_Model> getTitleList(string TitleName, int StartCount = 0, int EndCount = 99999999)
        {
            return TitleM_DAL.Instance.getTitleList(TitleName,  StartCount ,  EndCount);
        }


        public Title_Model getTitleDetail(int TitleID)
        {
            return TitleM_DAL.Instance.getTitleDetail(TitleID);
        }

        public int addTitle(Title_Model model)
        {
            return TitleM_DAL.Instance.addTitle(model);
        }

        public int updateTitle(Title_Model model)
        {
            return TitleM_DAL.Instance.updateTitle(model);
        }

        public int deleteTitle(Title_Model model)
        {
            return TitleM_DAL.Instance.deleteTitle(model);
        }
    }
}
