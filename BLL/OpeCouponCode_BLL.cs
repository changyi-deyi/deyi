using DAL;
using Model.Table_Model;
using Model.Operate_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class OpeCouponCode_BLL
    {
        #region 构造类实例
        public static OpeCouponCode_BLL Instance
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
            internal static readonly OpeCouponCode_BLL instance = new OpeCouponCode_BLL();
        }
        #endregion
        public InfCoupon_Model GetCouponCodeID(string Code)
        {
            return OpeCouponCode_DAL.Instance.GetCouponCodeID(Code);
            
        }
    }
}
