﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Table_Model
{
    [Serializable]
    public class OpeCustomerCoupon_Model
    {
        public string CouponCode { get; set; }
        public string CustomerCode { get; set; }
        public int ID { get; set; }
        public DateTime GotDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IsUsed { get; set; }
        public int Status { get; set; }
        public DateTime CreatetTime { get; set; }
        public int Creator { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int Updater { get; set; }
    }
}
