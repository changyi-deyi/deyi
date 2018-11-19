using Model.Manage_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebManager.Model
{
    [Serializable]
    public class MemberOrderList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<MemberOrder_Model> Data { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int PaymentStatus { get; set; }
        public int OrderStatus { get; set; }
    }

    [Serializable]
    public class MemberOrderDetail_Model {
        public MemberOrder_Model Data { get; set; }
        public string OrderCode { get; set; }
    }



    [Serializable]
    public class ServiceOrderList_Model
    {
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int RowsCount { get; set; }
        public int PageCount { get; set; }

        public List<ServiceOrder_Model> Data { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string DoctorName { get; set; }
        public string ServiceName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int PaymentStatus { get; set; }
        public int OrderStatus { get; set; }
        public int ServiceStatus { get; set; }
    }



    [Serializable]
    public class ServiceOrderDetail_Model
    {

        public ServiceOrder_Model Data { get; set; }
        public string OrderCode { get; set; }

        public List<Doctor_Model> DoctorList { get; set; }
        public List<Staff_Model> StaffList { get; set; }
    }


}