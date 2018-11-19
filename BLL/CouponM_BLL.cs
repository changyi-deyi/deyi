using DAL;
using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class CouponM_BLL
    {
        #region 构造类实例
        public static CouponM_BLL Instance
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
            internal static readonly CouponM_BLL instance = new CouponM_BLL();
        }

        #endregion

        public List<Coupon_Model> getCouponList(int Type, int Status, string Name, int StartCount = 0, int EndCount = 999999999)
        {
            return CouponM_DAL.Instance.getCouponList( Type,  Status,  Name, StartCount, EndCount);
        }


        public Coupon_Model getCouponDetail(int ID)
        {
            return CouponM_DAL.Instance.getCouponDetail(ID);
        }

        public int addCoupon(Coupon_Model model)
        {
            return CouponM_DAL.Instance.addCoupon(model);
        }

        public int updateCoupon(Coupon_Model model)
        {
            return CouponM_DAL.Instance.updateCoupon(model);
        }

        public int deleteCoupon(Coupon_Model model)
        {
            return CouponM_DAL.Instance.deleteCoupon(model);
        }
    }
}
