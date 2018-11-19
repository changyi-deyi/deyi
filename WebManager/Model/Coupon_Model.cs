using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class CouponList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<Coupon_Model> Data { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }

    }

    [Serializable]
    public class CouponDetail_Model
    {

        public Coupon_Model Data { get; set; }

        public int ID { get; set; }

    }
}