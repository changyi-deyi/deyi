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
    public class InfCoupon_BLL
    {
        #region 构造类实例
        public static InfCoupon_BLL Instance
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
            internal static readonly InfCoupon_BLL instance = new InfCoupon_BLL();
        }
        #endregion
        public List<InfCoupon_Model> GetCoupon()
        {
            return InfCoupon_DAL.Instance.GetCoupon();
        }
        public InfCoupon_Model GetCouponInfo(int CouponID)
        {
            return InfCoupon_DAL.Instance.GetCouponInfo(CouponID);
        }
        public List<CouponMember_Model> UsingCouponMember(string CustomerCode, int LevelID)
        {
            return InfCoupon_DAL.Instance.UsingCouponMember(CustomerCode, LevelID);

        }
        public List<CouponMember_Model> UsingCouponService(string CustomerCode, string ServiceCode)
        {
            return InfCoupon_DAL.Instance.UsingCouponService(CustomerCode, ServiceCode);
        }
    }
}
