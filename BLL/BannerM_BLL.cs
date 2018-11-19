using DAL;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BannerM_BLL
    {
        #region 构造类实例
        public static BannerM_BLL Instance
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
            internal static readonly BannerM_BLL instance = new BannerM_BLL();
        }

        #endregion

        public List<Banner_Model> getBannerList(int Status, int StartCount = 0, int EndCount = 999999999)
        {
            return BannerM_DAL.Instance.getBannerList(Status, StartCount, EndCount);
        }


        public Banner_Model getBannerDetail(int ID)
        {
            return BannerM_DAL.Instance.getBannerDetail(ID);
        }

        public int addBanner(Banner_Model model)
        {
            return BannerM_DAL.Instance.addBanner(model);
        }

        public int updateBanner(Banner_Model model)
        {
            return BannerM_DAL.Instance.updateBanner(model);
        }

        public int deleteBanner(Banner_Model model)
        {
            return BannerM_DAL.Instance.deleteBanner(model);
        }


        public int UpdateSort(int ID, int Sort)
        {
            return BannerM_DAL.Instance.UpdateSort(ID, Sort);

        }



    }
}
